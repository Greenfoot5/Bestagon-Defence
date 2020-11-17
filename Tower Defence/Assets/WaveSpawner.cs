﻿using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public float timeBetweenWaves = 5f;
    private float _countdown = 2f;

    public TMPro.TMP_Text waveCountdownText;

    private int _waveIndex;

    public Transform spawnPoint;
    
    [SerializeField]
    private float timeBetweenEnemies = 0.5f;

    private void Update()
    {
        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
        }

        _countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(_countdown).ToString();
    }

    private IEnumerator SpawnWave()
    {
        _waveIndex++;
        
        for (var i = 0; i < _waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}