using Abstract.Saving;
using Gameplay;
using UI.Transition;
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
        [Tooltip("The UI panel to enable when paused")]
        [SerializeField]
        private GameObject ui;
    
        private bool _hasBeenToggled;

        /// <summary>
        /// Allows the class to listen to the pause button press
        /// </summary>
        private void Start()
        {
            GameStats.controls.Game.Pause.performed += Toggle;
        }

        /// <summary>
        /// Disconnects the event from running when the level is closed
        /// </summary>
        private void OnDestroy()
        {
            GameStats.controls.Game.Pause.performed -= Toggle;
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
        /// Restarts the current level
        /// </summary>
        public void Retry()
        {
            GameStats.ClearStats();
            Toggle(new InputAction.CallbackContext());
            SaveManager.ClearSave(SceneManager.GetActiveScene().name);
            TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void Menu()
        {
            // Transition to the main menu
            Toggle(new InputAction.CallbackContext());
            TransitionManager.Instance.LoadScene("LevelSelect");
        }
    }
}
