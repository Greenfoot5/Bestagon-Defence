using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles asking the player to input a username if they don't have one already
    /// </summary>
    public class SetUsername : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The canvas for the main menu")]
        private GameObject mainMenuCanvas;
        [SerializeField]
        [Tooltip("The main menu in the scene")]
        private MainMenu mainMenu;
        [SerializeField]
        [Tooltip("The Text the player's input")]
        private TMP_Text input;
        [SerializeField]
        [Tooltip("The text to fill with the error message")]
        [FormerlySerializedAs("result")]
        private TMP_Text errorText;

        [SerializeField]
        [Tooltip("Too short error message")]
        private LocalizedString tooShortErrorMessage;
        [SerializeField]
        [Tooltip("Too long error message")]
        private LocalizedString tooLongErrorMessage;
        
        /// <summary>
        /// Saves the username the player picked if valid
        /// </summary>
        public void SaveUsername()
        {
            switch (input.text.Length)
            {
                case < 2:
                    errorText.text = tooShortErrorMessage.GetLocalizedString();
                    break;
                case > 20:
                    errorText.text = tooLongErrorMessage.GetLocalizedString();
                    break;
                default:
                    // The username is valid and the game can save it
                    // The game needs to remove some weird input character Unity adds
                    PlayerPrefs.SetString("Username", input.text.Replace("​", ""));
                    mainMenu.DisplayUsername();
                    mainMenuCanvas.SetActive(true);
                    gameObject.SetActive(false);
                    break;
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
