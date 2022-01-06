using TMPro;
using UnityEngine;

namespace UI
{
    public class SetUsername : MonoBehaviour
    {
        public MainMenu mainMenu;
        public TMP_Text input;
        public TMP_Text result;
        
        public void SaveUsername()
        {
            if (input.text.Length < 2)
            {
                result.text = "Please enter a name";
            }
            else if (input.text.Length > 20)
            {
                result.text = "Please enter a name less than 20 characters";
            }
            else
            {
                PlayerPrefs.SetString("Username", input.text);
                mainMenu.DisplayUsername();
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (PlayerPrefs.GetString("Username") != "")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
