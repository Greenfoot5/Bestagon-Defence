using Gameplay;
using TMPro;
using UI.Transition;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// A UI handler for the GameOver UI in a level
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        [Tooltip("The text to display the current round number on")]
        [SerializeField]
        private TMP_Text roundsText;
        
        /// <summary>
        /// Sets the player's score display when enabled
        /// </summary>
        public void OnEnable()
        {
            roundsText.text = "<size=4em><b>" + GameStats.Rounds + "</b></size>";
        }
        
        /// <summary>
        /// Restarts the current level
        /// </summary>
        public void Retry()
        {
            TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
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
