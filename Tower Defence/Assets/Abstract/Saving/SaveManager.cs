using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Abstract.Saving
{
    /// <summary>
    /// Handles saving and loading of persistent data,
    /// including which file it's kept in
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// Used to load any persistent data that needs to be loaded at the start of the game,
        /// </summary>
        public static IEnumerator InitialLoad()
        {
            // A "delay" to make sure files are loaded correctly
            yield return LocalizationSettings.InitializationOperation;
            
            // Load Locale
            if (FileManager.LoadFromFile("Settings.dat", out string json))
            {
                var sd = new SaveSettings();
                sd.LoadFromJson(json);
                LocalizationSettings.SelectedLocale = sd.locale;
            }
            
            // TODO - Check for a level save
        }
        
        /// <summary>
        /// Saves the settings
        /// </summary>
        /// <param name="saveable">The settings to save to the file</param>
        public static void SaveSettings(ISaveableSettings saveable)
        {
            var sd = new SaveSettings();
            saveable.PopulateSaveData(sd);
            
            if (FileManager.WriteToFile("Settings.dat", sd.ToJson()))
            {
                Debug.Log("Saving Settings successful");
            }
        }
        
        /// <summary>
        /// Loads the settings
        /// </summary>
        /// <param name="saveable">The settings to load</param>
        public static void LoadSettings(ISaveableSettings saveable)
        {
            if (!FileManager.LoadFromFile("Settings.dat", out string json)) return;
        
            var sd = new SaveSettings();
            sd.LoadFromJson(json);
            
            saveable.LoadFromSaveData(sd);

            Debug.Log("Loading settings complete");
        }
        
        /// <summary>
        /// Saves a level's current progress
        /// </summary>
        /// <param name="saveable">The level data to save to file</param>
        public static void SaveLevel(ISaveableLevel saveable)
        {
            var sd = new SaveLevel();
            saveable.PopulateSaveData(sd);
            
            if (FileManager.WriteToFile("LevelSave.dat", sd.ToJson()))
            {
                Debug.Log("Saving Level successful");
            }
        }
        
        /// <summary>
        /// Loads a level save
        /// </summary>
        /// <param name="saveable">The level save to load</param>
        public static void LoadLevel(ISaveableLevel saveable)
        {
            if (!FileManager.LoadFromFile("LevelSave.dat", out string json)) return;
        
            var sd = new SaveLevel();
            sd.LoadFromJson(json);
            
            saveable.LoadFromSaveData(sd);

            Debug.Log("Loading level complete");
        }
    }
}