using Godot;

namespace BestagonDefence.Levels.Generic.MainMenu;

/// <summary>
/// Handles asking the player to input a username if they don't have one already
/// </summary>
public partial class SetUsername : Control
{
    /// <summary>
    /// The canvas for the main menu
    /// </summary>
    [Export]
    private Control mainMenuCanvas;
    /// <summary>
    /// The main menu in the scene
    /// </summary>
    [Export]
    private MainMenu mainMenu;
    /// <summary>
    /// The text the player's input
    /// </summary>
    [Export]
    private LineEdit input;
    /// <summary>
    /// The text to fill with the error message
    /// </summary>
    [Export]
    private Label errorText;

    /// <summary>
    /// Too short error message
    /// </summary>
    [Export]
    private string tooShortErrorMessage;
    /// <summary>
    /// Too long error message
    /// </summary>
    [Export]
    private string tooLongErrorMessage;
        
    /// <summary>
    /// Saves the username the player picked if valid
    /// </summary>
    public void SaveUsername()
    {
        switch (input.Text.Length)
        {
            case < 2:
                errorText.Text = tooShortErrorMessage;
                break;
            case > 20:
                errorText.Text = tooLongErrorMessage;
                break;
            default:
                // The username is valid and the game can save it
                // The game needs to remove some weird input character Unity adds
                // TODO - PlayerPrefs
                // PlayerPrefs.SetString("Username", input.Text.Replace("​", ""));
                mainMenu.DisplayUsername();
                mainMenuCanvas.Visible = true;
                this.Visible = false;
                break;
        }
    }
        
    /// <summary>
    /// Checks if the player has a username saved, and if not, forces them to input one
    /// </summary>
    public static bool HasUsername()
    {
        // TODO - PlayerPrefs
        // return PlayerPrefs.GetString("Username") != "";
        return false;
    }
}