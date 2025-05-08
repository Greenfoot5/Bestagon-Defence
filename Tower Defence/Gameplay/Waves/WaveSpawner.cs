using System.Threading;
using Abstract.Attributes;
using Abstract.Saving;
using Enemies;
using Godot;
using Timer = Godot.Timer;

namespace Gameplay.Waves
{
    /// <summary>
    /// Handles the current wave and spawning of enemies
    /// </summary>
    public partial class WaveSpawner : Node2D
    {
        public enum State
        {
            // Countdown to next spawn
            Countdown,
            // Spawning Enemies
            Spawning,
            // Waiting for all enemies to die
            Waiting,
        }
        /// <summary>
        /// Current stats of all spawners
        /// </summary>
        public static State SpawnerState = State.Waiting;
        private static int _activeSpawners;
        
        /// <summary>
        /// How many enemies are still alive in the level
        /// </summary>
        public static int EnemiesAlive;
        
        private WaveData _waveData;
        private Timer _spawnTimer;
        private int _waveIndex;
        private int _setIndex;
        private int _enemyIndex;

        private Vector2[] _points;
        
        /// <summary>
        /// The waves the level will loop through
        /// </summary>
        [Export]
        private Wave[] _waves;
        
        [ExportGroup("Scene References")]
        [Export]
        private WaveTimer _countdown;
        
        /// <summary>
        /// The text to update when the countdown/spawning/enemies
        /// </summary>
        // [Export]
        // private Label waveCountdownText;
        /// <summary>
        /// The Progress Graphic to display the wave progress
        // </summary>
        // TODO - Allow export
        // [Export]
        // private Progress waveProgress;
        /// <summary>
        /// The label to display the current wave
        /// </summary>
        // [Export]
        // private Label waveText;

        /// <summary>
        /// The text to show with the wave count
        /// </summary>
        // [ExportGroup("Localization")]
        // [Export]
        // private string waveCountText;
        /// <summary>
        /// The text to show how many enemies are alive
        /// </summary>
        // [Export]
        // private string enemiesAliveText;
        /// <summary>
        /// The text to show when more enemies are being spawned
        /// </summary>
        // [Export]
        // private string spawningText;

        /// <summary>
        /// Sets the starting variables
        /// </summary>
        public override void _Ready()
        {
            // TODO - We need to load the Rounds value somewhere
            GameStats.Rounds = 1;
            EnemiesAlive = 0;
            // TODO - Prep time?
            // _countdown = preparationTime;
            
            // Creates and adds all waypoints to the array
            _points = new Vector2[GetChildCount()];
            for (var i = 0; i < _points.Length; i++)
            {
                _points[i] = ((Node2D)GetChild(i)).GlobalPosition;
            }

            _waveData = _countdown.WaveData;
            GameStats.OnRoundProgress += UpdateWaveText;
            _countdown.Timeout += StartSpawning;
            _spawnTimer = new Timer();
            _spawnTimer.Timeout += SpawnNext;
            _spawnTimer.OneShot = true;
            AddChild(_spawnTimer);
        }

        private void OnDestroy()
        {
            GameStats.OnRoundProgress -= UpdateWaveText;
        }

        /// <summary>
        /// Begins the spawning sequence for this spawner for the current wave
        /// </summary>
        private void StartSpawning()
        {
            // Update State
            Interlocked.Increment(ref _activeSpawners);
            
            // Update the wave index and spawn the first enemy
            _waveIndex = GameStats.Rounds - 1;
            if (GameStats.Rounds > _waveData.Length)
                _waveIndex = (_waveIndex - _waveData.RepeatIndex) % (_waveData.Length - _waveData.RepeatIndex) + _waveData.RepeatIndex;
            SpawnNext();
        }

        /// <summary>
        /// Starts the spawn for the enemy, and loads the timer to spawn the one after
        /// </summary>
        private void SpawnNext()
        {
            GD.Print(_waveIndex);
            GD.Print("wave" + _waves[_waveIndex]);
            GD.Print("set" + _waves[_waveIndex].enemySets[_setIndex]);
            SpawnEnemy(_waves[_waveIndex].enemySets[_setIndex].enemy);

            _enemyIndex += 1;
            float timerLength = _waves[_waveIndex].enemySets[_setIndex].rate;
            
            // TODO - Is calculating this every time the best?
            int setCount = Mathf.FloorToInt(_waves[_waveIndex].enemySets[_setIndex].count * _waveData.EnemyCount.Sample(_setIndex + 1));
            
            // We've spawned the full count of the set
            if (_enemyIndex >= setCount)
            {
                _enemyIndex = 0;
                _setIndex += 1;
                
                // We've spawned all the sets for this wave
                if (_setIndex >= _waves[_waveIndex].enemySets.Length)
                {
                    _enemyIndex = 0;
                    _setIndex = 0;
                    
                    // This spawner is last to finish
                    if (Interlocked.Decrement(ref _activeSpawners) == 0)
                    {
                        SpawnerState = State.Waiting;
                        _spawnTimer.Stop();
                        
                        return;
                    }
                }
                else if (_setIndex < _waves[_waveIndex].setDelays.Length)
                {
                    timerLength = _waves[_waveIndex].setDelays[_setIndex];
                }
            }

            _spawnTimer.Start(timerLength);
        }
        
        /// <summary>
        /// Spawns an enemy and applies scaling based on wave number
        /// </summary>
        /// <param name="stats">The stats of the new enemy</param>
        private void SpawnEnemy(EnemyStats stats)
        {
            EnemiesAlive++;
            
            // Spawn Enemy
            var spawnedEnemy = (Enemy) _waveData.Enemy.Instantiate();
            spawnedEnemy.Stats = stats;
            spawnedEnemy.Position = Position;
            spawnedEnemy.Rotation = Rotation;
            spawnedEnemy.Name = "_" + spawnedEnemy.Name;
            spawnedEnemy.points = _points;
        
            // Apply scaling
            spawnedEnemy.Stats.Attributes[AttributeType.MaxHealth].Add("SpawnerScaling",
                new AttributeModifier(_waveData.Health.Sample(_setIndex + 1), Operation.Multiplicative));
            // spawnedEnemy.TakeDamage(spawnedEnemy.Stats.Attributes[AttributeType.MaxHealth].Value, this);
            
            spawnedEnemy.OnDeath += () => { EnemiesAlive--; };
            GetParent().AddChild(spawnedEnemy);
        }
        
        /// <summary>
        /// Saves the settings to json data
        /// </summary>
        private static void SaveJsonData(ISaveableLevel level)
        {
            var saveData = new SaveLevel();
            level.PopulateSaveData(saveData);
            
            // TODO - Save level with scene name
            // SaveManager.SaveLevel(level, SceneManager.GetActiveScene().Name);
        }

        private void UpdateWaveText()
        {
            // waveText.Text = waveCountText + GameStats.Rounds;
        }
    }
}
