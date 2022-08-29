using Abstract;
using Levels.Generic.LevelSelect;
using Levels.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    /// <summary>
    /// Manages the current game's state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // If the game has actually finished yet
        public static bool isGameOver;
        
        [Tooltip("The UI to display when the player loses")]
        public GameObject gameOverUI;

        [Tooltip("The UI that displays the shop")]
        [SerializeField]
        private GameObject shop;
        
        [Tooltip("The levelData to use for the current level")]
        public LevelData levelData;
        
        [Tooltip("The leaderboard bridge")]
        public LeaderboardServerBridge bridge;
        
        /// <summary>
        /// Makes sure the level has some data to run with
        /// Makes sure that the game isn't over.
        /// </summary>
        private void Start()
        {
            isGameOver = false;
            if (levelData == null)
            {
                Debug.LogError("No level data set!", this);
            }
        }
    
        /// <summary>
        /// Checks if the game is over yet
        /// </summary>
        private void Update()
        {
            if (isGameOver)
            {
                return;
            }
        
            if (GameStats.Lives <= 0)
            {
                EndGame();
            }
        }
    
        /// <summary>
        /// Ends the game.
        /// Displays the game over screen and saves the player's score.
        /// </summary>
        private void EndGame()
        {
            isGameOver = true;
        
            gameOverUI.SetActive(true);
            shop.SetActive(false);
        
            // Tell our leaderboard API to add the player
            string leaderboardData =
                System.Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
            if (leaderboardData == null) return;
            string[] splitData = leaderboardData.Split(';');
            bridge.SendPlayerValue(PlayerPrefs.GetString("Username"), GameStats.Rounds, splitData[0], splitData[1]);
        }
    }
}
