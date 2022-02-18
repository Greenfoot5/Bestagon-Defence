using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles all UI actions for the main menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [Tooltip("The animation for the transition")]
        [SerializeField]
        private Animator transition;
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
            StartCoroutine(Transition("LevelSelect"));
        }
        
        /// <summary>
        /// Transitions the user to the next scene
        /// </summary>
        /// <param name="sceneName">The scene to transition the user to</param>
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger(AnimationTrigger);

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
        
        /// <summary>
        /// Transition the user to the settings scene
        /// </summary>
        public void Settings()
        {
            StartCoroutine(Transition("Settings"));
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
