using BestagonDefense.Abstract.Saving;
using BestagonDefense.UI.Transition;
using Godot;

namespace BestagonDefense.Levels.Generic.LevelSelect;

/// <summary>
/// Handles any UI actions on the level select screen
/// </summary>
public partial class LevelSelect : Control
{
    private string _selectedLevel;
    /// <summary>
    /// The button to play the level
    /// </summary>
    [Export]
    private Button playButton;
        
    /// <summary>
    /// The level info/leaderboard panel
    /// </summary>
    [Export]
    private Control levelInfo;
    /// <summary>
    /// The level select panel
    /// </summary>
    [Export]
    private Control levelSelect;
    /// <summary>
    /// The button that toggles between the info/leaderboard and the level select panels
    /// </summary>
    [Export]
    private Button infoButton;

    /// <summary>
    /// The continue button to enable/disable if there's a save
    /// </summary>
    [Export]
    private Button continueButton;

    /// <summary>
    /// The level name title
    /// </summary>
    [ExportGroup("Level Info")]
    [Export]
    private Label levelName;
    /// <summary>
    /// The PackedScene for an entry on the leaderboard
    /// </summary>
    [Export]
    private PackedScene leaderboardEntry;
    /// <summary>
    /// The content of the scroll view to place the leaderboard entries
    /// </summary>
    [Export]
    private Container leaderboardContent;
    /// <summary>
    /// The high score label
    /// </summary>
    [Export]
    private Label highScore;
        
    /// <summary>
    /// The text for translating to for leaderboards
    /// </summary>
    [ExportGroup("Localization Strings")]
    [Export]
    private string leaderboardButtonText;
    /// <summary>
    /// The text for translating to for level select
    /// </summary>
    [Export]
    private string levelSelectButtonText;
    /// <summary>
    /// To display after the level name above the scoreboard
    /// </summary>
    [Export]
    private string scoresText;

    /// <summary>
    /// Disables play and info buttons
    /// </summary>
    public LevelSelect()
    {
        _selectedLevel = null;
        playButton.Disabled = true;
        infoButton.Disabled = true;
    }
        
    /// <summary>
    /// Selects a level
    /// </summary>
    /// <param name="sceneName">The name of the level's scene to select</param>
    public void SelectLevel(string sceneName)
    {
        _selectedLevel = sceneName;
        playButton.Disabled = false;
        infoButton.Disabled = false;

        bool saveExists = SaveManager.SaveExists(sceneName);
        continueButton.Visible = saveExists;
            
    }
        
    /// <summary>
    /// Starts the selected level
    /// </summary>
    /// <param name="loadingLevel">0 if the level is not being loaded from the save</param>
    public void Play(int loadingLevel)
    {
        // TODO - PlayerPrefs
        // PlayerPrefs.SetInt("LoadingLevel", loadingLevel);
        TransitionManager.Instance.LoadScene(_selectedLevel);
    }

    /// <summary>
    /// Toggles the leaderboard display for the selected level
    /// </summary>
    public async void DisplayLeaderboard()
    {
        if (levelSelect.Visible)
        {
            // Display the leaderboard
            levelInfo.Visible = true;
            levelSelect.Visible = false;
            infoButton.GetChild<Label>(0).Text = levelSelectButtonText;
                
            // Display the level info
            // string levelNameLocalized = 
            //     new stringDatabase().Getstring(TransitionManager.Instance.tableReference,
            //         (TableEntryReference)_selectedLevel, LocalizationSettings.SelectedLocale);
            // levelName.Text = levelNameLocalized + scoresText;
                
            // Setup to display scores
            // var bridge = new LeaderboardServerBridge();
            // string leaderboardID =
            //     System.Environment.GetEnvironmentVariable(_selectedLevel + "Leaderboard")?.Split(';')[0];
            // // Check the level has a leaderboard
            // if (leaderboardID == null)
            // {
            //     Debug.LogWarning("Could not get leaderboard for level " + _selectedLevel);
            //     return;
            // }

            for (int c = leaderboardContent.GetChildCount() - 1; c >= 0; c--)
            {
                leaderboardContent.GetChild(c).QueueFree();
            }    
                
            // Display leaderboard
            // List<LeaderboardEntry> scores = await bridge.RequestEntries(10, leaderboardID);
            // foreach (LeaderboardEntry entry in scores)
            // {
            //     GodotObject leaderboardItem = Instantiate(leaderboardEntry, leaderboardContent);
            //     leaderboardItem.Name = "_" + leaderboardItem.Name;
            //     leaderboardItem.transform.GetChild(0).GetChild(0).GetComponent<Label>().Text = entry.Name;
            //     leaderboardItem.transform.GetChild(0).GetChild(1).GetComponent<Label>().Text = entry.GetValueAsString();
            // }
                
            // Display the player's high score
            // LeaderboardEntry playerScore = await LeaderboardServerBridge.RequestPlayerEntry(PlayerPrefs.GetString("Username"), leaderboardID);
            // highScore.Text = playerScore != null ? playerScore.GetValueAsString() : "0";
        }
        else
        {
            // Display the level selection menu
            levelInfo.Visible = false;
            levelSelect.Visible = true;
            infoButton.GetChild<Label>(0).Text = leaderboardButtonText;
        }
    }
        
    /// <summary>
    /// Returns the player to the main menu
    /// </summary>
    public void MainMenu()
    {
        TransitionManager.Instance.LoadScene("MainMenu");
    }
}