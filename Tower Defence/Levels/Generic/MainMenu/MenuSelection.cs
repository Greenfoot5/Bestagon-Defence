using Godot;

namespace BestagonDefense.Levels.Generic.MainMenu;

/// <summary>
/// Displays the correct menu when the main menu loads
/// </summary>
public partial class MenuSelection : Control
{
    /// <summary>
    /// The canvas for the main menu
    /// </summary>
    [Export]
    private Control menuCanvas;
    /// <summary>
    /// The canvas for the login menu
    /// </summary>
    [Export]
    private Control loginCanvas;
    /// <summary>
    /// The canvas for the update menu
    /// </summary>
    [Export]
    private Control updateCanvas;

    /// <summary>
    /// Check to see which menu we should display
    /// </summary>
    public override void _Ready()
    {
        // if (!RemoteConfig.IsValidVersion())
        // {
        //     menuCanvas.Visible = false;
        //     loginCanvas.Visible = false;
        //     updateCanvas.Visible = true;
        // }
        if (!SetUsername.HasUsername())
        {
            menuCanvas.Visible = false;
            loginCanvas.Visible = true;
            updateCanvas.Visible = false;
        }
        else {
            menuCanvas.Visible = true;
            loginCanvas.Visible = false;
            updateCanvas.Visible = false;
        }
    }
        
    /// <summary>
    /// Closes the update menu
    /// </summary>
    public void ContinueWithoutUpdating()
    {
        if (!SetUsername.HasUsername())
        {
            menuCanvas.Visible = false;
            loginCanvas.Visible = true;
            updateCanvas.Visible = false;
        }
        else
        {
            menuCanvas.Visible = true;
            loginCanvas.Visible = false;
            updateCanvas.Visible = false;
        }
    }
        
    /// <summary>
    /// Sends the player off to download the update
    /// </summary>
    public void GetUpdate()
    {
        OS.ShellOpen("https://greenfoot5.itch.io/bestagon-defence");
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
    }
}