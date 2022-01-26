using System.Collections;
using Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        public TMP_Text roundsText;
        
        public Animator transition;
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transition.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            
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
