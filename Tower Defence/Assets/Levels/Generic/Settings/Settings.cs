using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Transition;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Levels.Generic.Settings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The dropdown to select language/locale ")]
        private TMP_Dropdown dropdown;

        private IEnumerator Start()
        {
            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;

            // Generate list of available Locales
            var options = new List<TMP_Dropdown.OptionData>();
            var selected = 0;
            for (var i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.name));
            }
            dropdown.options = options;

            dropdown.value = selected;
            dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        private static void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }

        /// <summary>
        /// Sends the player to the level select scene
        /// </summary>
        public void MainMenu()
        {
            TransitionManager.Instance.LoadScene("MainMenu");
        }
    }
}