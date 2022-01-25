using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public Animator transition;
        public TMP_Text loggedInAs;

        private void Start()
        {
            DisplayUsername();
        }

        public void DisplayUsername()
        {
            loggedInAs.text = "Logged in as \n" + PlayerPrefs.GetString("Username");
        }

        public void Play()
        {
            StartCoroutine(Transition("LevelSelect"));
        }
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
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
