using System;
using Gameplay;
using Godot;
using UI.Transition;

namespace UI
{
    /// <summary>
    /// Handles the pause menu in a level
    /// </summary>
    public partial class SpeedControls : Control
    {
        /// <summary>
        /// The UI panel to enable when paused
        /// </summary>
        [Export]
        private Control pausedUI;

        // TODO - Could just place timer on top?
        /// <summary>
        /// Timer to hide when paused
        /// </summary>
        [Export]
        private Control timer;
        /// <summary>
        /// Timer to show when paused
        /// </summary>
        [Export]
        private Control pausedTimer;
        /// <summary>
        /// Text showing the current wave on pause
        /// </summary>
        [Export]
        private Label pausedWaveText;
        /// <summary>
        /// Prefix to wave
        /// </summary>
        [Export]
        private string waveText;

        /// <summary>
        /// Allows the class to listen to the pause button press
        /// </summary>
        public override void _Ready()
        {
            // GameStats.controls.Game.Pause.performed += ToggleMenu;
        }

        /// <summary>
        /// Disconnects the event from running when the level is closed
        /// </summary>
        private void OnDestroy()
        {
            // GameStats.controls.Game.Pause.performed -= ToggleMenu;
        }

        public void SetSpeed(float speed)
        {
            // Pause
            if (speed == 0f)
            {
                // Resume
                if (Engine.TimeScale == 0d)
                {
                    Engine.TimeScale = 1d;
                    UpdateTimer();
                    return;
                }

                
                Engine.TimeScale = 0d;
                UpdateTimer();
                return;
            }
            
            // Toggle current speed
            if (Math.Abs(Engine.TimeScale - speed) < 0.001)
            {
                Engine.TimeScale = 0f;
                UpdateTimer();
                return;
            }

            Engine.TimeScale = speed;
            UpdateTimer();
        }

        /// <summary>
        /// Pauses/unpauses the game, and enables/disables the UI by input button press
        // </summary>
        // TODO - Input
        // private void ToggleMenu(InputAction.CallbackContext ctx)
        // {
        //     pausedUI.SetActive(!pausedUI.activeSelf);
        //     
        //     Engine.TimeScale = pausedUI.activeSelf ? 0f : 1f;
        //     UpdateTimer();
        // }
        
        /// <summary>
        /// Pauses/unpauses the game and enables/disables the UI by UI button press
        /// </summary>
        public void ToggleMenu()
        {
            pausedUI.Visible = !pausedUI.Visible;
            
            Engine.TimeScale = pausedUI.Visible ? 0f : 1f;
            UpdateTimer();
        }
    
        /// <summary>
        /// Restarts the current level
        /// </summary>
        public void Retry()
        {
            GameStats.ClearStats();
            // TODO - SceneManager
            // SaveManager.ClearSave(SceneManager.GetActiveScene().Name);
            // TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().Name);
        }

        private void UpdateTimer()
        {
            if (Engine.TimeScale != 0f)
            {
                // timer.Visible = true;
                // pausedTimer.Visible = false;
            }
            else
            {
                // timer.Visible = false;
                // pausedTimer.Visible = true;
                // pausedWaveText.Text = waveText + GameStats.Rounds;
            }
        }
    
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void Menu()
        {
            // Transition to the main menu
            TransitionManager.Instance.LoadScene("LevelSelect");
        }
    }
}
