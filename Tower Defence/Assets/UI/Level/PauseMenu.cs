using System;
using System.Collections;
using Abstract;
using Abstract.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.Level
{
    /// <summary>
    /// Handles the pause menu in a level
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        public GameObject ui;
        
        public Animator transition;
    
        private bool _hasBeenToggled;
        
        public LeaderboardServerBridge bridge;
        
        /// <summary>
        /// Allows the class to listen to the pause button press
        /// </summary>
        private void Start()
        {
            GameStats.controls.Default.Pause.performed += Toggle;
        }

        /// <summary>
        /// Pauses/unpauses the game, and enables/disables the UI by input button press
        /// </summary>
        public void Toggle(InputAction.CallbackContext ctx)
        {
            ui.SetActive(!ui.activeSelf);

            Time.timeScale = ui.activeSelf ? 0f : 1f;
        }
        
        /// <summary>
        /// Pauses/unpauses the game and enables/disables the UI by UI button press
        /// </summary>
        public void Toggle()
        {
            ui.SetActive(!ui.activeSelf);

            Time.timeScale = ui.activeSelf ? 0f : 1f;
        }
        
        /// <summary>
        /// Starts the transition when changing scenes
        /// </summary>
        /// <param name="sceneName">The scene to transition to</param>
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
    
        /// <summary>
        /// Restarts the current level
        /// </summary>
        public void Retry()
        {
            Toggle(new InputAction.CallbackContext());
            StartCoroutine(Transition(SceneManager.GetActiveScene().name));
        }
    
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void Menu()
        {
            try
            {
                // Tell our leaderboard API to add the player
                var leaderboardData =
                    Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
                if (leaderboardData == null) return;
                var splitData = leaderboardData.Split(';');
                bridge.SendPlayerValue(PlayerPrefs.GetString("Username"), GameStats.rounds, splitData[0], splitData[1]);
            }
            catch (Exception)
            {
                // TODO - Now ignore the error
            }
            
            // Transition to the main menu
            Toggle(new InputAction.CallbackContext());
            StartCoroutine(Transition("MainMenu"));
        }
    }
}
