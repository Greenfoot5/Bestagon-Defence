using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject ui;
        
        public Animator transition;
    
        private bool _hasBeenToggled;

        // Pauses/unpauses the game, and toggles the UI
        public void Toggle()
        {
            ui.SetActive(!ui.activeSelf);

            if (ui.activeSelf)
            {
                Time.timeScale = 0f;
                Input.ResetInputAxes();
            }
            else
            {
                Time.timeScale = 1f;
            }
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
            Toggle();
            StartCoroutine(Transition(SceneManager.GetActiveScene().name));
        }
    
        // The Main Menu button that returns us to the main menu
        // TODO - return to level select, not main menu
        public void Menu()
        {
            Toggle();
            StartCoroutine(Transition("MainMenu"));
        }
    
        // Called each frame
        public void Update()
        {
            // On press, pause the game
            if (Input.GetAxis("Pause") > 0 && !_hasBeenToggled)
            {
                Toggle();
                _hasBeenToggled = true;
            }
            // Set's _hasBeenToggled on release
            else if (Input.GetAxis("Pause") == 0)
            {
                _hasBeenToggled = false;
            }
        }
    }
}
