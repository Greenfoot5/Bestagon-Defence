using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        public TMP_Text roundsText;
        
        public Animator transition;
        public float transitionTime;
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);
            
            SceneManager.LoadScene(sceneName);
        }

        public void OnEnable()
        {
            roundsText.text = "<size=4em><b>" + GameStats.rounds + "</b></size>";
        }

        public void Retry()
        {
            StartCoroutine(Transition(SceneManager.GetActiveScene().name));;
        }

        public void Menu()
        {
            StartCoroutine(Transition("MainMenu"));;
        }
    }
}
