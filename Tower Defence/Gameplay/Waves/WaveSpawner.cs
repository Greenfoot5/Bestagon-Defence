using System.Collections;
using Abstract.Attributes;
using Abstract.Saving;
using Enemies;
using Godot;
using Levels._Nodes;
using Levels.Maps;

namespace Gameplay.Waves
{
    /// <summary>
    /// Handles the current wave and spawning of enemies
    /// </summary>
    public partial class WaveSpawner : BuildableTile
    {
        private enum State
        {
            // Countdown to next spawn
            Countdown,
            // Spawning Enemies
            Spawning,
            // Waiting for all enemies to die
            Waiting,
        }
        
        /// <summary>
        /// How many enemies are still alive in the level
        /// </summary>
        public static int enemiesAlive;
        
        /// <summary>
        /// The waves the level will loop through
        /// </summary>
        [Export]
        private Wave[] waves;
        
        /// <summary>
        /// How long to wait between waves
        /// </summary>
        [Export]
        private float timeBetweenWaves = 5f;
        /// <summary>
        /// The time at the beginning before the start of the game
        /// </summary>
        [Export]
        private float preparationTime = 8f;
        private float _countdown = 5f;
        /// <summary>
        /// The index of the wave to start from after the final wave
        /// </summary>
        [Export]
        private int waveRepeatIndex;
        
        /// <summary>
        /// The text to update when the countdown/spawning/enemies
        /// </summary>
        [Export]
        private RichTextLabel waveCountdownText;
        /// <summary>
        /// The Progress Graphic to display the wave progress
        // </summary>
        // TODO - Allow export
        // [Export]
        // private Progress waveProgress;
        /// <summary>
        /// The label to display the current wave
        /// </summary>
        [Export]
        private RichTextLabel waveText;
        
        private int _waveIndex;
        
        /// <summary>
        /// The location to spawn the enemies. Should be the first waypoint
        /// </summary>
        [Export]
        private Node2D spawnPoint;

        private State _currentState = State.Waiting;
        private float _totalEnemies;

        private LevelData _levelData;

        /// <summary>
        /// The text to show with the wave count
        /// </summary>
        [ExportGroup("Localization")]
        [Export]
        private string waveCountText;
        /// <summary>
        /// The text to show how many enemies are alive
        /// </summary>
        [Export]
        private string enemiesAliveText;
        /// <summary>
        /// The text to show when more enemies are being spawned
        /// </summary>
        [Export]
        private string spawningText;

        /// <summary>
        /// Sets the starting variables
        /// </summary>
        public override void _Ready()
        {
            enemiesAlive = 0;
            // TODO - GetComponent<GameManager>()
            // _levelData = this.GetComponent<GameManager>().levelData;
            _countdown = preparationTime;
            _waveIndex = GameStats.Rounds - 1;
            GameStats.OnRoundProgress += UpdateWaveText;
        }

        private void OnDestroy()
        {
            GameStats.OnRoundProgress -= UpdateWaveText;
        }
        
        /// <summary>
        /// Updates the countdown and checks if the game should start spawning the next wave.
        /// </summary>
        public override void _Process(double delta)
        {
            switch (_currentState)
            {
                case State.Spawning:
                    return;
                // If still waiting
                case State.Waiting when enemiesAlive > 0:
                    // waveProgress.percentage = enemiesAlive / _totalEnemies;
                    waveCountdownText.Text = enemiesAliveText;
                    return;
                // If done waiting
                case State.Waiting when enemiesAlive <= 0:
                    _currentState = State.Countdown;
                    _waveIndex++;
                    GameStats.Rounds = _waveIndex + 1;
                    waveText.Text = waveCountText + GameStats.Rounds;
                    // SaveJsonData(this.GetComponent<GameManager>());
                    break;
            }

            // If the countdown has finished, call the next wave
            if (_currentState == State.Countdown)
            {
                _countdown -= (float)delta;
                _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Inf);

                waveCountdownText.Text = $"{_countdown:0.00}";
                // waveProgress.percentage = _countdown / timeBetweenWaves;
                
                if (_countdown <= 0f)
                {
                    // Save the level
                    // SaveJsonData(this.GetComponent<GameManager>());
                    // Start spawning in the enemies
                    // TODO - StartCoroutine
                    // StartCoroutine(SpawnWave());
                }
            }
        }
    
        /// <summary>
        /// Spawns the enemies from an entire wave
        /// </summary>
        private IEnumerator SpawnWave()
        {
            _currentState = State.Spawning;
            waveCountdownText.Text = spawningText;
            Wave wave = waves[_waveIndex % waveRepeatIndex];
            if (GameStats.Rounds > waveRepeatIndex)
                wave = waves[(_waveIndex - waveRepeatIndex) % (waves.Length - waveRepeatIndex) + waveRepeatIndex];
            
            _totalEnemies = 0;

            for (var i = 0; i < wave.enemySets.Length; i++)
            {
                EnemySet set = wave.enemySets[i];
                // waveProgress.percentage = i/ (float) wave.enemySets.Length;
            
                // For all the enemies the enemySet will spawn,
                // spawn one, then wait timeBetweenEnemies seconds
                int setCount = Mathf.FloorToInt(set.count * _levelData.enemyCount.Value.Sample(_waveIndex + 1));
                for (var j = 0; j < setCount; j++)
                {
                    SpawnEnemy(set.enemy);
                    _totalEnemies++;

                    if (j + 1 != setCount)
                    {
                        // yield return new WaitForSeconds(set.rate);
                    }
                }

                if (i + 1 != wave.enemySets.Length)
                {
                    // yield return new WaitForSeconds(wave.setDelays[i]);
                }
            }
            
            _countdown = timeBetweenWaves;
            _currentState = State.Waiting;
            yield break;
        }
    
        /// <summary>
        /// Spawns an enemy and applies scaling based on wave number
        /// </summary>
        /// <param name="enemy">The enemy to spawn</param>
        private void SpawnEnemy(PackedScene enemy)
        {
            enemiesAlive++;
            
            // Spawn Enemy
            var spawnedEnemy = (Enemy) enemy.Instantiate();
            spawnedEnemy.Position = spawnPoint.Position;
            spawnedEnemy.Rotation = spawnPoint.Rotation;
            spawnedEnemy.Name = "_" + spawnedEnemy.Name;
            // TODO - Move to layer
            // spawnedEnemy.layer = LayerMask.NameToLayer("Enemies");
        
            // Apply scaling
            spawnedEnemy.Attributes[AttributeType.MaxHealth].Add("SpawnerScaling",
                new AttributeModifier(_levelData.health.Value.Sample(_waveIndex + 1), Operation.Multiplicative));
            spawnedEnemy.TakeDamage(spawnedEnemy.Attributes[AttributeType.MaxHealth].Value, this);
            
            spawnedEnemy.OnDeath += () => { enemiesAlive--; };
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
            waveText.Text = waveCountText + GameStats.Rounds;
        }
    }
}
