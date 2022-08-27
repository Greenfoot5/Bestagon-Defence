using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Transition;
using UI.Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Levels.Generic.LevelSelect
{
    /// <summary>
    /// Handles any UI actions on the level select screen
    /// </summary>
    public class LevelSelect : MonoBehaviour
    {
        private string _selectedLevel;
        [Tooltip("The button to play the level")]
        [SerializeField]
        private Button playButton;
        
        [Tooltip("The level info/leaderboard panel")]
        [SerializeField]
        private GameObject levelInfo;
        [Tooltip("The level select panel")]
        [SerializeField]
        private GameObject levelSelect;
        [Tooltip("The button that toggles between the info/leaderboard and the level select panels")]
        [SerializeField]
        private GameObject infoButton;

        [Header("Level Info")]
        [Tooltip("The level name title")]
        [SerializeField]
        private TMP_Text levelName;
        [Tooltip("The prefab for an entry on the leaderboard")]
        [SerializeField]
        private GameObject leaderboardEntry;
        [Tooltip("The content of the scroll view to place the leaderboard entries")]
        [SerializeField]
        private Transform leaderboardContent;
        [Tooltip("The high score text")]
        [SerializeField]
        private TMP_Text highScore;
        private static readonly int Start = Animator.StringToHash("Start");

        /// <summary>
        /// Disables play and info buttons
        /// </summary>
        public void Awake()
        {
            _selectedLevel = null;
            playButton.interactable = false;
            infoButton.GetComponent<Button>().interactable = false;
        }
        
        /// <summary>
        /// Selects a level
        /// </summary>
        /// <param name="sceneName">The name of the level's scene to select</param>
        public void SelectLevel(string sceneName)
        {
            _selectedLevel = sceneName;
            playButton.interactable = true;
            infoButton.GetComponent<Button>().interactable = true;
        }
        
        /// <summary>
        /// Starts the selected level
        /// </summary>
        public void Play()
        {
            TransitionManager.Instance.LoadScene(_selectedLevel);
        }
        
        /// <summary>
        /// Toggles the leaderboard display for the selected level
        /// </summary>
        public async void DisplayLeaderboard()
        {
            if (levelSelect.activeSelf)
            {
                // Display the leaderboard
                levelInfo.SetActive(true);
                levelSelect.SetActive(false);
                infoButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Levels";
                
                // Display the level info
                levelName.text = _selectedLevel.Substring(0, _selectedLevel.Length - 5) + " Scores";
                
                // Setup to display scores
                var bridge = levelInfo.GetComponent<LeaderboardServerBridge>();
                string leaderboardID =
                    System.Environment.GetEnvironmentVariable(_selectedLevel + "Leaderboard")?.Split(';')[0];
                // Check the level has a leaderboard
                if (leaderboardID == null)
                {
                    Debug.LogWarning("Could not get leaderboard for level " + _selectedLevel);
                    return;
                }

                for (int c = leaderboardContent.childCount - 1; c >= 0; c--)
                {
                    Destroy(leaderboardContent.GetChild(c).gameObject);
                }    
                
                // Display leaderboard
                List<LeaderboardEntry> scores = await bridge.RequestEntries(10, leaderboardID);
                foreach (LeaderboardEntry entry in scores)
                {
                    GameObject leaderboardItem = Instantiate(leaderboardEntry, leaderboardContent);
                    leaderboardItem.name = "_" + leaderboardItem.name;
                    leaderboardItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = entry.name;
                    leaderboardItem.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = entry.GetValueAsString();
                }
                
                // Display the player's high score
                LeaderboardEntry playerScore = await bridge.RequestPlayerEntry(PlayerPrefs.GetString("Username"), leaderboardID);
                highScore.text = playerScore != null ? playerScore.GetValueAsString() : "N/A";
            }
            else
            {
                // Display the level selection menu
                levelInfo.SetActive(false);
                levelSelect.SetActive(true);
                infoButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Leaderboard";
            }
        }
        
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void MainMenu()
        {
            TransitionManager.Instance.LoadScene("MainMenu");
        }
    }
}
