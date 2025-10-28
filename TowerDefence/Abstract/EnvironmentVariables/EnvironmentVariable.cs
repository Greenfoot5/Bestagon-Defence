using System;

namespace BestagonDefence.Abstract.EnvironmentVariables;

/// <summary>
/// Allows us to store data and get it depending on if the game is in the editor, stable or development build.
/// </summary>
[Serializable]
// TODO - Probably a better way to do this?
public struct EnvironmentVariable
{
    // [Export]
    // private string name;
    // [Export]
    // private string stable;
    // [Export]
    // private string betaBuild;
    // [Export]
    // private string alphaBuild;
    // [Export]
    // [FormerlySerializedAs("nightlyBuild")] private string devBuild;
    // [Export]
    // private string editor;
        
    /// <summary>
    /// Gets the data for the correct version (Editor/Development Build/Release)
    /// </summary>
    /// <returns>The data for the version</returns>
    public string GetData()
    {
// #if UNITY_EDITOR
//             return editor;
// #else
//             if (Application.version.ToLower().Contains("alpha")) 
//                 return alphaBuild;
//             else if (Application.version.ToLower().Contains("dev"))
//                 return devBuild;
//             else if (Application.version.ToLower().Contains("beta"))
//                 return betaBuild;
//             return stable;
// #endif
        return "1.0.0";
    }
        
    /// <summary>
    /// Sets the data for the current version
    /// </summary>
    public void SetData()
    {
        // Environment.SetEnvironmentVariable(name, GetData());
    }
}