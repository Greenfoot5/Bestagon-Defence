using System.Collections;
using Abstract.Data;
using Enemies;
using TMPro;
using UnityEngine;

namespace Abstract.Managers
{
    /// <summary>
    /// Handles the current wave and spawning of enemies
    /// </summary>
    public class WaveSpawner : MonoBehaviour
    {
        public static int enemiesAlive;
    
        // All the waves in the level
        public Wave[] waves;
    
        // Times before sending waves
        public float timeBetweenWaves = 5f;
        [Tooltip("The time at the beginning before the start of the game")]
        public float preparationTime = 5f;
        // Set to first countdown
        private float _countdown = 5f;
    
        // The countdown text timer
        public TMP_Text waveCountdownText;
    
        // The current wave the player is on -1 (as it's indexed from 0)
        private int _waveIndex;
    
        // The point to spawn in the enemy.
        // Should be waypoint 0
        public Transform spawnPoint;

        private bool _isSpawning;

        private LevelData.LevelData _levelData;

        /// <summary>
        /// Sets the starting variables
        /// </summary>
        private void Start()
        {
            enemiesAlive = 0;
            _levelData = gameObject.GetComponent<GameManager>().levelData;
            _countdown = preparationTime;
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

            // If the countdown has finished, call the next wave
            if (_countdown <= 0f)
            {
                // Start spawning in the enemies
                StartCoroutine(SpawnWave());
                // Reset the timer
                _countdown = timeBetweenWaves;

                return;
            }

            _countdown -= Time.deltaTime;
            _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        
            waveCountdownText.text = $"<sprite=\"UI-Icons\" name=\"Clock\">{_countdown:0.00}";
        
            if (GameStats.lives > 0)
                GameStats.rounds = _waveIndex + 1;
        }
    
        /// <summary>
        /// Spawns the enemies from an entire wave
        /// </summary>
        private IEnumerator SpawnWave()
        {
            _isSpawning = true;
            var wave = waves[_waveIndex % waves.Length];

            for (var i = 0; i < wave.enemySets.Length; i++)
            {
                var set = wave.enemySets[i];
            
                // For all the enemies the enemySet will spawn,
                // spawn one, then wait timeBetweenEnemies seconds
                var setCount = Mathf.FloorToInt(set.count * Mathf.Pow(_levelData.enemyCount, _waveIndex));
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

            if (_waveIndex % waves.Length != 0) yield break;
            Debug.Log("Level complete!");
        }
    
        /// <summary>
        /// Spawns an enemy and applies scaling based on wave number
        /// </summary>
        /// <param name="enemy">The enemy to spawn</param>
        private void SpawnEnemy(GameObject enemy)
        {
            // Spawn Enemy
            var spawnedEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            spawnedEnemy.layer = LayerMask.NameToLayer("Enemies");
        
            // Apply scaling
            spawnedEnemy.GetComponent<Enemy>().maxHealth *= Mathf.Pow(_levelData.health, _waveIndex);
            spawnedEnemy.GetComponent<Enemy>().health = spawnedEnemy.GetComponent<Enemy>().maxHealth;
        
            enemiesAlive++;
        }
    }
}
