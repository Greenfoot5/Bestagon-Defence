using Godot;
using UI.Transition;

namespace Levels.Generic.Tutorial;

/// <summary>
/// Allows players to navigate the tutorial scene
/// </summary>
public partial class Tutorial : Control
{
    /// <summary>
    /// The tutorial viewport
    /// </summary>
    [Export]
    private Control tutorialMenu;
    /// <summary>
    /// The controls viewport
    /// </summary>
    [Export]
    private Control controlsMenu;
    /// <summary>
    /// The button to cycle between the different viewports
    /// </summary>
    [Export]
    private Button toggleButtonText;
        
    /// <summary>
    /// The button string to display to select the tutorial
    /// </summary>
    [Export]
    private string tutorialText;
    /// <summary>
    /// The button string to display to select the controls
    /// </summary>
    [Export]
    private string controlsText;
        
    public void MainMenu()
    {
        TransitionManager.Instance.LoadScene("MainMenu");
    }

    public void ToggleControls()
    {
        if (controlsMenu.Visible)
        {
            tutorialMenu.Visible = true;
            controlsMenu.Visible = false;
            toggleButtonText.Text = controlsText;
        }
        else
        {
            tutorialMenu.Visible = false;
            controlsMenu.Visible = true;
            toggleButtonText.Text = tutorialText;
        }
    }
        
    /// <summary>
    /// Sends the player to the dev wiki if it's the dev version
    /// </summary>
    public void Wiki()
    {
        OS.ShellOpen(ProjectSettings.GetSettingWithOverride("application/config/version").AsString().Contains("dev")
            ? "https://greenfoot5.notion.site/Nightly-Wiki-90094b3bcf284ae9834a828d4a4bfede"
            : "https://greenfoot5.notion.site/Wiki-ba485298423447b89f491091ec1687a7");
    }
}