using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelect : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime;
        
        private string _selectedLevel;
        public Button playButton;

        public void Awake()
        {
            _selectedLevel = null;
            playButton.interactable = false;
        }

        public void SelectLevel(string levelName)
        {
            _selectedLevel = levelName;
            playButton.interactable = true;
        }

        public void Play()
        {
            StartCoroutine(Transition(_selectedLevel));
        }

        public void MainMenu()
        {
            StartCoroutine(Transition("MainMenu"));
        }
        
        private IEnumerator Transition(string sceneName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
