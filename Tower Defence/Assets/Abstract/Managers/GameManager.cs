using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abstract.Managers
{
    /// <summary>
    /// Manages the current game's state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static bool isGameOver;

        public GameObject gameOverUI;

        public LevelData.LevelData levelData;

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
        
            if (GameStats.lives <= 0)
            {
                EndGame();
            }
        }
    
        /// <summary>
        /// Ends the game.
        /// Displays the game over screen and saves the user's score.
        /// </summary>
        private void EndGame()
        {
            isGameOver = true;
        
            gameOverUI.SetActive(true);
        
            // Tell our leaderboard API to add the player
            var leaderboardData =
                System.Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
            if (leaderboardData == null) return;
            var splitData = leaderboardData.Split(';');
            bridge.SendPlayerValue(PlayerPrefs.GetString("Username"), GameStats.rounds, splitData[0], splitData[1]);
        }
    }
}
