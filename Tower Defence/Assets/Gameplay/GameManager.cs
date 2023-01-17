using Abstract.Saving;
using Levels._Nodes;
using Levels.Generic.LevelSelect;
using Levels.Maps;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    /// <summary>
    /// Manages the current game's state
    /// </summary>
    public class GameManager : MonoBehaviour, ISaveableLevel
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

        [Tooltip("The parent of all the nodes")]
        public GameObject nodeParent;
        
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

        public void PopulateSaveData(SaveLevel saveData)
        {
            saveData.lives = GameStats.Lives;
            saveData.money = GameStats.money;
            saveData.waveIndex = GameStats.Rounds - 1;
            saveData.nodes = nodeParent.GetComponentsInChildren<Node>();
            saveData.random = Random.state;
        }

        public void LoadFromSaveData(SaveLevel saveData)
        {
            GameStats.Lives = saveData.lives;
            GameStats.money = saveData.money;
            GameStats.Rounds = saveData.waveIndex;
            // TODO - Deal with UUIDs to restore Node data
            Random.state = saveData.random;
        }
    }
}
