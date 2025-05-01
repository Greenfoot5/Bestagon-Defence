using Gameplay;
using Godot;
using UI.Transition;

namespace UI
{
    /// <summary>
    /// A UI handler for the GameOver UI in a level
    /// </summary>
    public partial class GameOver : Control
    {
        /// <summary>
        /// The text to display the current round number on
        /// </summary>
        [Export]
        private Label roundsText;
        
        /// <summary>
        /// Sets the player's score display when enabled
        /// </summary>
        public void OnEnable()
        {
            roundsText.Text = GameStats.Rounds.ToString();
        }
        
        /// <summary>
        /// Restarts the current level
        /// </summary>
        public void Retry()
        {
            // TODO - Player Prefs & Scene Manager
            // PlayerPrefs.SetInt("LoadingLevel", 0);
            // TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().Name);
        }
        
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void Menu()
        {
            TransitionManager.Instance.LoadScene("MainMenu");
        }
    }
}
