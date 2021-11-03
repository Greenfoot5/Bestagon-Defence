using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime;

        public void Play()
        {
            StartCoroutine(Transition("LevelSelect"));
        }
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);
            
            SceneManager.LoadScene(sceneName);
        }

        public void Settings()
        {
            StartCoroutine(Transition("Settings"));
        }

        public void Quit()
        {
            Debug.Log("Exiting...");
            Application.Quit();
        }
    }
}
