using TMPro;
using UI.Transition;
using UnityEngine;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles all UI actions for the main menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Tooltip("The text that displays the username of the player")]
        [SerializeField]
        private TMP_Text loggedInAs;

        /// <summary>
        /// Sets the username text
        /// </summary>
        private void Start()
        {
            DisplayUsername();
        }
        
        /// <summary>
        /// Display the user's current username
        /// </summary>
        public void DisplayUsername()
        {
            loggedInAs.text = "Logged in as \n" + PlayerPrefs.GetString("Username");
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
    }
}
    