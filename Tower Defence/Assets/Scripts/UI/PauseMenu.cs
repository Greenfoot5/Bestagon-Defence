using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject ui;
        
        public Animator transition;
    
        private bool _hasBeenToggled;
        
        public LeaderboardServerBridge bridge;

        private void Start()
        {
            GameStats.controls.Default.Pause.performed += Toggle;
        }

        // Pauses/unpauses the game, and toggles the UI
        public void Toggle(InputAction.CallbackContext ctx)
        {
            ui.SetActive(!ui.activeSelf);

            Time.timeScale = ui.activeSelf ? 0f : 1f;
        }
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
    
        // The retry button, reloads the current scene
        public void Retry()
        {
            Toggle(new InputAction.CallbackContext());
            StartCoroutine(Transition(SceneManager.GetActiveScene().name));
        }
    
        // The Main Menu button that returns us to the main menu
        public void Menu()
        {
            try
            {
                // Tell our leaderboard API to add the user
                var leaderboardData =
                    Environment.GetEnvironmentVariable(SceneManager.GetActiveScene().name + "Leaderboard");
                if (leaderboardData == null) return;
                var splitData = leaderboardData.Split(';');
                bridge.SendUserValue(PlayerPrefs.GetString("Username"), GameStats.rounds, splitData[0], splitData[1]);
            }
            catch (Exception)
            {
                // TODO - Now ignore the error
            }

            Toggle(new InputAction.CallbackContext());
            StartCoroutine(Transition("MainMenu"));
        }
    }
}
