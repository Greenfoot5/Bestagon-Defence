using System.Text;
using Godot;

namespace UI;

/// <summary>
/// Displays the version text so the player can know what version they're on
/// </summary>
public partial class VersionText : Label
{
    private static bool _exists;

    /// <summary>
    /// Generates the VersionText and displays it for the player
    /// </summary>
    private void Awake()
    {
        if (_exists)
            Free();

        // TODO - DDOL
        // DontDestroyOnLoad(this);

        _exists = true;

        var text = new StringBuilder();

        text.AppendLine("Bestagon Defence");
        text.Append($"{ProjectSettings.GetSettingWithOverride("application/config/version").AsString()}\n");
        text.Append($"Godot {Engine.Singleton.GetVersionInfo()["string"]}\n");
        text.Append($"{OS.GetName()} {OS.GetVersion()}");

        Text = text.ToString();
    }
}