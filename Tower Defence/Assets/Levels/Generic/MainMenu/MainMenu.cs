using System.Collections;
using TMPro;
using UI.Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        private static readonly int AnimationTrigger = Animator.StringToHash("Start");

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
        /// Transition the user to the settings scene
        /// </summary>
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
    }
}
    