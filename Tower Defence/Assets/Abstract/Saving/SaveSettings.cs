using UnityEngine;
using UnityEngine.Localization;

namespace Abstract.Saving
{
    public class SaveSettings
    { 
        public Locale locale;

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public interface ISaveableSettings
    {
        void PopulateSaveData(SaveSettings saveData);
        void LoadFromSaveData(SaveSettings saveData);
    }
}