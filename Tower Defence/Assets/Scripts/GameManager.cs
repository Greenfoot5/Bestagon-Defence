﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;

    public GameObject gameOverUI;

    public LevelData.LevelData levelData;

    public LeaderboardServerBridge bridge;

    private void Start()
    {
        isGameOver = false;
        if (levelData == null)
        {
            Debug.LogError("No level data set!", this);
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        
        if (GameStats.lives <= 0)
        {
            EndGame();
        }
    }
    
    // Called when we reach 0 lives
    private void EndGame()
    {
        isGameOver = true;
        
        gameOverUI.SetActive(true);
        DiscordController.Refresh();
        
        // Tell our leaderboard API to add the user
        var leaderboardData =
            System.Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
        if (leaderboardData == null) return;
        var splitData = leaderboardData.Split(';');
        bridge.SendUserValue(PlayerPrefs.GetString("Username"), GameStats.rounds, splitData[0], splitData[1]);
    }
}
