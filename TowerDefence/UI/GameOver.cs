using BestagonDefence.Gameplay;
using BestagonDefence.UI.Transition;
using Godot;

namespace BestagonDefence.UI;

/// <summary>
/// A UI handler for the GameOver UI in a level
/// </summary>
public partial class GameOver : Control
{
    /// <summary>
    /// The text to display the current round number on
    /// </summary>
    [Export]
    private Label roundsText;

    /// <summary>
    /// Initialises the listeners
    /// </summary>
    public override void _EnterTree()
    {
        VisibilityChanged += OnEnable;
    }

    /// <summary>
    /// Removes the listeners when destroyed
    /// </summary>
    public override void _ExitTree()
    {
        VisibilityChanged -= OnEnable;
    }
        
    /// <summary>
    /// Sets the player's score display when enabled
    /// </summary>
    private void OnEnable()
    {
        roundsText.Text = GameStats.Rounds.ToString();
    }
        
    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void Retry()
    {
        // TODO - Player Prefs & Scene Manager
        // PlayerPrefs.SetInt("LoadingLevel", 0);
        // TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().Name);
        GameStats.ClearStats();
        GetTree().ReloadCurrentScene();
    }
        
    /// <summary>
    /// Returns the player to the main menu
    /// </summary>
    public void Menu()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
        // TransitionManager.Instance.LoadScene("MainMenu");
    }
}