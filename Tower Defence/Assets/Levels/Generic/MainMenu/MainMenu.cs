using Abstract;
using Abstract.Saving;
using TMPro;
using UI.Transition;
using UnityEngine;
using UnityEngine.UI;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles all UI actions for the main menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Header("Wordmark")]
        [Tooltip("The wordmark/logo on the main screen to colour based on version")]
        public Image wordmark;

        [Tooltip("The colour to make the wordmark if it's a nightly build")]
        public Color nightlyColor;
        [Tooltip("The colour to make the wordmark if it's a alpha build")]
        public Color alphaColor;
        [Tooltip("The colour to make the wordmark if it's a beta build")]
        public Color betaColor;
        [Tooltip("The colour to make the wordmark if it's a release build")]
        public Color releaseColor;
        
        [Header("Username")]
        [Tooltip("The text that displays the username of the player")]
        [SerializeField]
        private TMP_Text loggedInAs;

        [Tooltip("The saved username")]
        [HideInInspector]
        public string username;

        /// <summary>
        /// Does the bits and bobs needed when the game starts
        /// </summary>
        private void Awake()
        {
            DisplayUsername();
            ColourWordmark();
            
            Runner.Run(SaveManager.InitialLoad());
        }
        
        /// <summary>
        /// Display the user's current username
        /// </summary>
        public void DisplayUsername()
        {
            username = PlayerPrefs.GetString("Username");
            loggedInAs.text = "Logged in as \n" + username;
        }
        
        /// <summary>
        /// Sends the player to the level select scene
        /// </summary>
        public void Play()
        {
            TransitionManager.Instance.LoadScene("LevelSelect");
        }
        
        /// <summary>
        /// Transition the user to the tutorial scene
        /// </summary>
        public void Tutorial()
        {
            TransitionManager.Instance.LoadScene("Tutorial");
        }

        public void Settings()
        {
            TransitionManager.Instance.LoadScene("Settings");
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void Quit()
        {
            Debug.Log("Exiting...");
            Application.Quit();
        }

        public void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }

        private void ColourWordmark()
        {
            if (Application.version.Contains("nightly"))
            {
                wordmark.color = nightlyColor;
            }
            else if (Application.version.Contains("alpha"))
            {
                wordmark.color = alphaColor;
            }
            else if (Application.version.Contains("beta"))
            {
                wordmark.color = betaColor;
            }
            else
            {
                wordmark.color = releaseColor;
            }
        }
    }
}
    