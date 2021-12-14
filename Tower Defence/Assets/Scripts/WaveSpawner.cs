using System.Collections;
using Enemies;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive;
    
    // All the waves in the level
    public Wave[] waves;

    // Times before sending waves
    public float timeBetweenWaves = 5f;
    // Set to first countdown
    private float _countdown = 5f;
    
    // The countdown text timer
    public TMP_Text waveCountdownText;
    
    // The current wave we're on
    private int _waveIndex;
    
    // The point to spawn in the enemy.
    // Should be waypoint 0
    public Transform spawnPoint;

    public bool isSpawning;

    private LevelData.LevelData _levelData;

    void Start()
    {
        enemiesAlive = 0;
        _levelData = gameObject.GetComponent<GameManager>().levelData;
    }

    private void Update()
    {
        // Only reduce the countdown if there are enemies remaining
        // TODO - Update countdown text to enemies remaining
        if (enemiesAlive > 0 || isSpawning)
        {
            return;
        }

        // If we've reached the end of the countdown,
        // call the next wave
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
        
        waveCountdownText.text = string.Format("<sprite=\"UI-Icons\" name=\"Clock\">{0:0.00}", _countdown);

        GameStats.rounds = _waveIndex + 1;
    }
    
    // Spawns in our enemies
    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        var wave = waves[_waveIndex % waves.Length];

        for (var i = 0; i < wave.waveSets.Length; i++)
        {
            var set = wave.waveSets[i];
            
            // For all the enemies we will spawn,
            // spawn one, then wait timeBetweenEnemies seconds
            var setCount = Mathf.FloorToInt(set.count * Mathf.Pow(_levelData.enemyCount, _waveIndex));
            for (var j = 0; j < setCount; j++)
            {
                SpawnEnemy(set.enemy);

                if (j + 1 != set.count)
                {
                    yield return new WaitForSeconds(set.rate);
                }
            }

            if (i + 1 != wave.waveSets.Length)
            {
                yield return new WaitForSeconds(wave.setDelays[i]);
            }
        }

        _waveIndex++;
        isSpawning = false;

        if (_waveIndex % waves.Length != 0) yield break;
        Debug.Log("Level complete!");
    }
    
    /// <summary>
    /// Spawns enemies and applies scaling
    /// </summary>
    /// <param name="enemy">The enemy to spawn</param>
    private void SpawnEnemy(GameObject enemy)
    {
        // Spawn Enemy
        var spawnedEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemy.layer = LayerMask.NameToLayer("Enemies");
        
        // Apply scaling
        spawnedEnemy.GetComponent<Enemy>().maxHealth *= Mathf.Pow(_levelData.health, _waveIndex);
        
        enemiesAlive++;
    }
}
