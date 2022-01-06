using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
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

        public void Awake()
        {
            _selectedLevel = null;
            playButton.interactable = false;
            infoButton.GetComponent<Button>().interactable = false;
            Debug.Log(PlayerPrefs.GetString("Username"));
        }

        public void SelectLevel(string sceneName)
        {
            _selectedLevel = sceneName;
            playButton.interactable = true;
            infoButton.GetComponent<Button>().interactable = true;
        }

        public void Play()
        {
            StartCoroutine(Transition(_selectedLevel));
        }

        public async void DisplayLeaderboard()
        {
            if (levelSelect.activeSelf)
            {
                levelInfo.SetActive(true);
                levelSelect.SetActive(false);
                infoButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Levels";
                
                // Display the level info
                levelName.text = _selectedLevel.Substring(_selectedLevel.Length - 5) + " Scores";
                
                // Setup to display scores
                var bridge = levelInfo.GetComponent<LeaderboardServerBridge>();
                var leaderboardID =
                    System.Environment.GetEnvironmentVariable(_selectedLevel + "Leaderboard")?.Split(';')[0];
                // Check we've actually got a leaderboard
                if (leaderboardID == null)
                {
                    Debug.LogWarning("Could not get leaderboard");
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
                    leaderboardItem.transform.Find("Username").GetComponent<TMP_Text>().text = entry.name;
                    leaderboardItem.transform.Find("Score").GetComponent<TMP_Text>().text = entry.GetValueAsString();
                }
                
                // Display the player's high score
                var userScore = await bridge.RequestUserEntry(PlayerPrefs.GetString("Username"), leaderboardID);
                if (userScore != null)
                {
                    highScore.text = userScore.GetValueAsString();
                }
            }
            else
            {
                levelInfo.SetActive(false);
                levelSelect.SetActive(true);
                infoButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Info";
            }
        }

        public void MainMenu()
        {
            StartCoroutine(Transition("MainMenu"));
        }
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
