using System.Collections;
using Abstract.Saving;
using Enemies;
using Levels.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Waves
{
    /// <summary>
    /// Handles the current wave and spawning of enemies
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        [Tooltip("How many enemies are still alive in the level")]
        public static int enemiesAlive;
        
        [Tooltip("The waves the level will loop through")]
        public Wave[] waves;
        
        
        [Tooltip("How long to wait in between waves")]
        public float timeBetweenWaves = 5f;
        [Tooltip("The time at the beginning before the start of the game")]
        public float preparationTime = 8f;
        private float _countdown = 5f;
        
        [Tooltip("The text to update with the countdown")]
        public TMP_Text waveCountdownText;
    
        // The current wave the player is on -1 (as it's indexed from 0)
        private int _waveIndex;
        
        [Tooltip("The location to spawn the enemies. Should be the first waypoint")]
        public Transform spawnPoint;

        private bool _isSpawning;

        private LevelData _levelData;

        /// <summary>
        /// Sets the starting variables
        /// </summary>
        private void Start()
        {
            enemiesAlive = 0;
            _levelData = gameObject.GetComponent<GameManager>().levelData;
            _countdown = preparationTime;
            _waveIndex = GameStats.Rounds;
        }
        
        /// <summary>
        /// Updates the countdown and checks if the game should start spawning the next wave.
        /// </summary>
        private void Update()
        {
            // Only reduce the countdown if there are enemies remaining
            // TODO - Update countdown text to enemies remaining
            if (enemiesAlive > 0 || _isSpawning)
            {
                return;
            }

            if (Mathf.Abs(_countdown - timeBetweenWaves) < 0.00000001f)
            {
                // Save the level
                SaveJsonData(gameObject.GetComponent<GameManager>());
            }

            // If the countdown has finished, call the next wave
            if (_countdown <= 0f)
            {
                // Save the level
                SaveJsonData(gameObject.GetComponent<GameManager>());
                // Start spawning in the enemies
                StartCoroutine(SpawnWave());
                // Reset the timer
                _countdown = timeBetweenWaves;

                return;
            }

            _countdown -= Time.deltaTime;
            _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        
            waveCountdownText.text = $"<sprite=\"UI-Icons\" name=\"Clock\">{_countdown:0.00}";
        
            if (GameStats.Lives > 0)
                GameStats.Rounds = _waveIndex + 1;
        }
    
        /// <summary>
        /// Spawns the enemies from an entire wave
        /// </summary>
        private IEnumerator SpawnWave()
        {
            _isSpawning = true;
            Wave wave = waves[_waveIndex % waves.Length];

            for (var i = 0; i < wave.enemySets.Length; i++)
            {
                EnemySet set = wave.enemySets[i];
            
                // For all the enemies the enemySet will spawn,
                // spawn one, then wait timeBetweenEnemies seconds
                int setCount = Mathf.FloorToInt(set.count * _levelData.enemyCount.Value.Evaluate(_waveIndex + 1));
                for (var j = 0; j < setCount; j++)
                {
                    SpawnEnemy(set.enemy);

                    if (j + 1 != setCount)
                    {
                        yield return new WaitForSeconds(set.rate);
                    }
                }

                if (i + 1 != wave.enemySets.Length)
                {
                    yield return new WaitForSeconds(wave.setDelays[i]);
                }
            }

            _waveIndex++;
            _isSpawning = false;

            //if (_waveIndex % waves.Length != 0) yield break;
            //Debug.Log("Level complete!");
        }
    
        /// <summary>
        /// Spawns an enemy and applies scaling based on wave number
        /// </summary>
        /// <param name="enemy">The enemy to spawn</param>
        private void SpawnEnemy(GameObject enemy)
        {
            // Spawn Enemy
            GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            spawnedEnemy.name = "_" + spawnedEnemy.name;
            spawnedEnemy.layer = LayerMask.NameToLayer("Enemies");
        
            // Apply scaling
            spawnedEnemy.GetComponent<Enemy>().maxHealth *= _levelData.health.Value.Evaluate(_waveIndex + 1);
            spawnedEnemy.GetComponent<Enemy>().health = spawnedEnemy.GetComponent<Enemy>().maxHealth;
        
            enemiesAlive++;
        }
        
        /// <summary>
        /// Saves the settings to json data
        /// </summary>
        private static void SaveJsonData(ISaveableLevel level)
        {
            var saveData = new SaveLevel();
            level.PopulateSaveData(saveData);
            
            SaveManager.SaveLevel(level, SceneManager.GetActiveScene().name);
        }
    }
}
