using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelect : MonoBehaviour
    {
        private String _selectedLevel;
        public Button playButton;

        public void Awake()
        {
            _selectedLevel = null;
            playButton.interactable = false;
        }

        public void SelectLevel(String levelName)
        {
            _selectedLevel = levelName;
            playButton.interactable = true;
        }

        public void Play()
        {
            SceneManager.LoadScene(_selectedLevel);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
