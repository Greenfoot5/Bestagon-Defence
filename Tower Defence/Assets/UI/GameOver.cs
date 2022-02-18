using System.Collections;
using Gameplay;
using TMPro;
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
        
        [Tooltip("The animation to animate the transition back to the main menu")]
        [SerializeField]
        private Animator transition;
        private static readonly int AnimationTrigger = Animator.StringToHash("Start");

        /// <summary>
        /// Begins the transition to the new level
        /// </summary>
        /// <param name="sceneName">The scene to transition to</param>
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger(AnimationTrigger);

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
        
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
            StartCoroutine(Transition(SceneManager.GetActiveScene().name));
        }
        
        /// <summary>
        /// Returns the player to the main menu
        /// </summary>
        public void Menu()
        {
            StartCoroutine(Transition("MainMenu"));
        }
    }
}
