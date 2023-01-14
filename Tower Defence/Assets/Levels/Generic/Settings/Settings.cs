using System.Collections;
using System.Collections.Generic;
using Abstract.Saving;
using TMPro;
using UI.Transition;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Levels.Generic.Settings
{
    public class Settings : MonoBehaviour, ISaveableSettings
    {
        [SerializeField]
        [Tooltip("The dropdown to select language/locale ")]
        private TMP_Dropdown dropdown;

        private IEnumerator Start()
        {
            // Wait for the localization system to initialize
            yield return LocalizationSettings.InitializationOperation;
            
            // Load the current settings
            LoadJsonData(this);

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

        private void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            SaveJsonData(this);
        }

        /// <summary>
        /// Sends the player to the level select scene
        /// </summary>
        public void MainMenu()
        {
            TransitionManager.Instance.LoadScene("MainMenu");
        }

        private static void SaveJsonData(ISaveableSettings settings)
        {
            var saveData = new SaveSettings();
            settings.PopulateSaveData(saveData);
            
            SaveManager.SaveSettings(settings);
        }

        public void PopulateSaveData(SaveSettings saveData)
        {
            saveData.locale = LocalizationSettings.SelectedLocale;
        }
        
        private static void LoadJsonData(ISaveableSettings settings)
        {
            SaveManager.LoadSettings(settings);
        }

        public void LoadFromSaveData(SaveSettings saveData)
        {
            LocalizationSettings.SelectedLocale = saveData.locale;
        }
    }
}