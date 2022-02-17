using System;
using System.Collections;
using Gameplay;
using Levels.Generic.LevelSelect;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
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
        private static readonly int Start1 = Animator.StringToHash("Start");

        /// <summary>
        /// Allows the class to listen to the pause button press
        /// </summary>
        private void Start()
        {
            GameStats.controls.Game.Pause.performed += Toggle;
        }

        /// <summary>
        /// Pauses/unpauses the game, and enables/disables the UI by input button press
        /// </summary>
        public void Toggle(InputAction.CallbackContext ctx)
        {
            // Check time isn't already paused
            if (!ui.activeSelf && Time.timeScale == 0f) return;
            
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
            transition.SetTrigger(Start1);

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
                string leaderboardData =
                    Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
                if (leaderboardData == null) return;
                string[] splitData = leaderboardData.Split(';');
                bridge.SendPlayerValue(PlayerPrefs.GetString("Username"), GameStats.Rounds, splitData[0], splitData[1]);
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
