using Godot;

namespace BestagonDefence.Abstract.Saving;

/// <summary>
/// Represents the settings data
/// </summary>
public partial class SaveSettings : Resource
{ 
    // public Locale Locale;
    public bool HasEnergyPickup = true;
        
    /// <summary>
    /// Translates the class into json format
    /// </summary>
    /// <returns>This class in json format</returns>
    public string ToJson()
    {
        return Json.Stringify(this);
    }
        
    /// <summary>
    /// Loads this class from jsom
    /// </summary>
    /// <param name="json">The json to load from</param>
    public void LoadFromJson(string json)
    {
        // JsonUtility.FromJsonOverwrite(json, this);
        Json.ParseString(json);
    }
}
    
/// <summary>
/// Handles the loading and populating of the save data
/// Designed to be implemented by MonoBehaviours
/// </summary>
public interface ISaveableSettings
{
    void PopulateSaveData(SaveSettings saveData);
    void LoadFromSaveData(SaveSettings saveData);
}