using _WIP;
using UnityEngine;

namespace Levels.Generic.MainMenu
{
    public class MenuSelection : MonoBehaviour
    {
        [Tooltip("The canvas for the main menu")]
        [SerializeField]
        private GameObject menuCanvas;
        [Tooltip("The canvas for the login menu")]
        [SerializeField]
        private GameObject loginCanvas;
        [Tooltip("The canvas for the update menu")]
        [SerializeField]
        private GameObject updateCanvas;

        // Start is called before the first frame update
        private void Start()
        {
            if (!RemoteConfig.IsValidVersion())
            {
                menuCanvas.SetActive(false);
                loginCanvas.SetActive(false);
                updateCanvas.SetActive(true);
                Debug.Log("Outdated");
            }
            else if (!SetUsername.HasUsername())
            {
                menuCanvas.SetActive(false);
                loginCanvas.SetActive(true);
                updateCanvas.SetActive(false);
                Debug.Log("No Username!r");
            }
            else
            {
                menuCanvas.SetActive(true);
                loginCanvas.SetActive(false);
                updateCanvas.SetActive(false);
            }
        }

        public void ContinueWithoutUpdating()
        {
            if (!SetUsername.HasUsername())
            {
                menuCanvas.SetActive(false);
                loginCanvas.SetActive(true);
                updateCanvas.SetActive(false);
            }
            else
            {
                menuCanvas.SetActive(true);
                loginCanvas.SetActive(false);
                updateCanvas.SetActive(false);
            }
        }
        
        public void GetUpdate()
        {
            Application.OpenURL("https://greenfoot5.itch.io/bestagon-defence");
            Application.Quit();
        }
    }
}
