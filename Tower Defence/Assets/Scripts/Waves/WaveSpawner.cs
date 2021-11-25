using System.Collections;
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

    void Start()
    {
        enemiesAlive = 0;
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
        var wave = waves[_waveIndex];

        // for (var i = 0; i < wave.waveSets.Length; i++)
        // {
        //     var set = wave.waveSets[i];
        //     
        //     // For all the enemies we will spawn,
        //     // spawn one, then wait timeBetweenEnemies seconds
        //     for (var j = 0; j < set.count; j++)
        //     {
        //         SpawnEnemy(set.enemy);
        //
        //         if (j + 1 != set.count)
        //         {
        //             yield return new WaitForSeconds(set.rate);
        //         }
        //     }
        //
        //     if (i + 1 != wave.waveSets.Length)
        //     {
        //         yield return new WaitForSeconds(wave.setDelays[i]);
        //     }
        // }

        _waveIndex++;
        isSpawning = false;

        if (_waveIndex != waves.Length) yield break;
        Debug.Log("Level complete!");
        enabled = false;
    }
    
    // Spawns an enemy.
    // What else would it do!?
    private void SpawnEnemy(GameObject enemy)
    {
        var spawnedEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemy.layer = LayerMask.NameToLayer("Enemies");
        enemiesAlive++;
    }
}
