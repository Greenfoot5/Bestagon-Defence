using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels.Generic.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        public GameObject tutorialMenu;
        public GameObject controlsMenu;
        public TMP_Text toggleButtonText;
        
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ToggleControls()
        {
            if (controlsMenu.activeSelf)
            {
                tutorialMenu.SetActive(true);
                controlsMenu.SetActive(false);
                toggleButtonText.text = "Controls";
            }
            else
            {
                tutorialMenu.SetActive(false);
                controlsMenu.SetActive(true);
                toggleButtonText.text = "Tutorial";
            }
        }
    }
}
