using TMPro;
using UI.Transition;
using UnityEngine;
using UnityEngine.Localization;

namespace Levels.Generic.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        public GameObject tutorialMenu;
        public GameObject controlsMenu;
        public TMP_Text toggleButtonText;
        
        [SerializeField]
        [Tooltip("The button string to display to select the tutorial")]
        private LocalizedString tutorialText;
        [SerializeField]
        [Tooltip("The button string to display to select the controls")]
        private LocalizedString controlsText;
        
        public void MainMenu()
        {
            TransitionManager.Instance.LoadScene("MainMenu");
        }

        public void ToggleControls()
        {
            if (controlsMenu.activeSelf)
            {
                tutorialMenu.SetActive(true);
                controlsMenu.SetActive(false);
                toggleButtonText.text = controlsText.GetLocalizedString();
            }
            else
            {
                tutorialMenu.SetActive(false);
                controlsMenu.SetActive(true);
                toggleButtonText.text = tutorialText.GetLocalizedString();
            }
        }

        public void Wiki()
        {
            Application.OpenURL("https://greenfoot5.notion.site/Wiki-ba485298423447b89f491091ec1687a7");
        }
    }
}
