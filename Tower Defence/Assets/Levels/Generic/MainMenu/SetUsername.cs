using TMPro;
using UnityEngine;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles asking the player to input a username if they don't have one already
    /// </summary>
    public class SetUsername : MonoBehaviour
    {
        public MainMenu mainMenu;
        public TMP_Text input;
        public TMP_Text result;
        
        /// <summary>
        /// Saves the username the player picked if valid
        /// </summary>
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
                // The username is valid and the game can save it
                // The game needs to remove some weird input character Unity adds
                PlayerPrefs.SetString("Username", input.text.Replace("​", ""));
                mainMenu.DisplayUsername();
                gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Checks if the player has a username saved, and if not, forces them to input one
        /// </summary>
        public static bool HasUsername()
        {
            return PlayerPrefs.GetString("Username") != "";
        }
    }
}
