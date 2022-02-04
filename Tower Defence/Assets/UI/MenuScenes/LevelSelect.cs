using System.Collections;
using Abstract.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MenuScenes
{
    /// <summary>
    /// Handles any UI actions on the level select screen
    /// </summary>
    public class LevelSelect : MonoBehaviour
    {
        public Animator transition;
        
        private string _selectedLevel;
        public Button playButton;
        
        public GameObject levelInfo;
        public GameObject levelSelect;
        public GameObject infoButton;

        [Header("Level Info")]
        public TMP_Text levelName;
        public GameObject leaderboardEntry;
        public Transform leaderboardContent;
        public TMP_Text highScore;
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
            StartCoroutine(Transition(_selectedLevel));
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
                var leaderboardID =
                    System.Environment.GetEnvironmentVariable(_selectedLevel + "Leaderboard")?.Split(';')[0];
                // Check the level has a leaderboard
                if (leaderboardID == null)
                {
                    Debug.LogWarning("Could not get leaderboard for level " + _selectedLevel);
                    return;
                }

                while (leaderboardContent.childCount > 0) {
                    DestroyImmediate(leaderboardContent.GetChild(0).gameObject);
                }
                
                // Display leaderboard
                var scores = await bridge.RequestEntries(10, leaderboardID);
                foreach (var entry in scores)
                {
                    var leaderboardItem = Instantiate(leaderboardEntry, leaderboardContent);
                    leaderboardItem.transform.Find("UsernameBackground").GetComponentInChildren<TMP_Text>().text = entry.name;
                    leaderboardItem.transform.Find("ScoreBackground").GetComponentInChildren<TMP_Text>().text = entry.GetValueAsString();
                }
                
                // Display the player's high score
                var playerScore = await bridge.RequestPlayerEntry(PlayerPrefs.GetString("Username"), leaderboardID);
                highScore.text = playerScore != null ? playerScore.GetValueAsString() : "N/A";
            }
            else
            {
                // Display the level selection menu
                levelInfo.SetActive(false);
                levelSelect.SetActive(true);
                infoButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Info";
            }
        }
        
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void MainMenu()
        {
            StartCoroutine(Transition("MainMenu"));
        }
        
        /// <summary>
        /// Transition to the next scene
        /// </summary>
        /// <param name="sceneName">The scene to transition to</param>
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger(Start);

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
