using System.Collections;
using Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Level
{
    /// <summary>
    /// A UI handler for the GameOver UI in a level
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        public TMP_Text roundsText;
        
        public Animator transition;
        
        /// <summary>
        /// Begins the transition to the new level
        /// </summary>
        /// <param name="sceneName">The scene to transition to</param>
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
            SceneManager.LoadScene(sceneName);
        }
        
        /// <summary>
        /// Sets the player's score display when enabled
        /// </summary>
        public void OnEnable()
        {
            roundsText.text = "<size=4em><b>" + GameStats.rounds + "</b></size>";
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
