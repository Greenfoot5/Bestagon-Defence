using Godot;
using UI.Transition;

namespace Levels.Generic.MainMenu
{
    /// <summary>
    /// Handles all UI actions for the main menu
    /// </summary>
    public partial class MainMenu : Control
    {
        /// <summary>
        /// The wordmark/logo on the main screen to colour based on version
        /// </summary>
        [ExportGroup("Wordmark")]
        [Export]
        private Sprite2D wordmark;

        /// <summary>
        /// The colour to make the wordmark if it's a dev build
        /// </summary>
        [Export]
        private Color devColor;
        /// <summary>
        /// The colour to make the wordmark if it's an alpha build
        /// </summary>
        [Export]
        private Color alphaColor;
        /// <summary>
        /// The colour to make the wordmark if it's a beta build
        /// </summary>
        [Export]
        private Color betaColor;
        /// <summary>
        /// The colour to make the wordmark if it's a release build
        /// </summary>
        [Export]
        private Color releaseColor;
        
        /// <summary>
        /// The text that displays the username of the player
        /// </summary>
        [ExportGroup("Username")]
        [Export]
        private RichTextLabel loggedInAs;

        /// <summary>
        /// The saved username
        /// </summary>
        public string username;

        /// <summary>
        /// Does the bits and bobs needed when the game starts
        /// </summary>
        private void Awake()
        {
            DisplayUsername();
            ColourWordmark();
            
            // TODO - Using Runner
            // Runner.Run(SaveManager.InitialLoad());
        }
        
        /// <summary>
        /// Display the user's current username
        /// </summary>
        public void DisplayUsername()
        {
            // TODO - PlayerPrefs
            // username = PlayerPrefs.GetString("Username");
            loggedInAs.Text = "Logged in as \n" + username;
        }
        
        /// <summary>
        /// Sends the player to the level select scene
        /// </summary>
        public void Play()
        {
            TransitionManager.Instance.LoadScene("LevelSelect");
        }
        
        /// <summary>
        /// Transition the user to the tutorial scene
        /// </summary>
        public void Tutorial()
        {
            TransitionManager.Instance.LoadScene("Tutorial");
        }

        public void Settings()
        {
            TransitionManager.Instance.LoadScene("Settings");
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void Quit()
        {
            GD.Print("Exiting...");
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
            GetTree().Quit();
        }
        
        /// <summary>
        /// Opens a web browser of the url
        /// </summary>
        /// <param name="url">The url to send the player to</param>
        public void OpenUrl(string url)
        {
            OS.ShellOpen(url);
        }
        
        /// <summary>
        /// Changes the colour of the wordmark based on the version type
        /// </summary>
        private void ColourWordmark()
        {
            if (ProjectSettings.GetSettingWithOverride("application/config/version").AsString().Contains("dev"))
            {
                wordmark.SelfModulate = devColor;
            }
            else if (ProjectSettings.GetSettingWithOverride("application/config/save_version").AsString().Contains("alpha"))
            {
                wordmark.SelfModulate = alphaColor;
            }
            else if (ProjectSettings.GetSettingWithOverride("application/config/save_version").AsString().Contains("beta"))
            {
                wordmark.SelfModulate = betaColor;
            }
            else
            {
                wordmark.SelfModulate = releaseColor;
            }
        }
    }
}
    