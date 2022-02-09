using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;
using System;
using System.Text;

namespace Abstract
{
    /// <summary>
    /// Discord Rich Presence activity controller.
    /// </summary>
    public class DiscordController : MonoBehaviour
    {
        private Discord.Discord _discord;

        private Activity _activity;

        private Scene _currentScene;

        public static readonly long applicationId = 927337431303352441;

        /// <summary>
        /// Prepares the bridge to Discord and ensures a singleton pattern.
        /// </summary>
        void Awake()
        {
            // Singleton pattern
            if (_discord != null)
            {
                Destroy(gameObject);
                return;
            }

            // Get Discord instance if it is running
            // Catch the error if Discord was not detected
            try
            {
                _discord = new Discord.Discord(applicationId, (ulong)CreateFlags.NoRequireDiscord);
            }
            catch (ResultException)
            {
                return;
            }

            _activity = new Activity() { Name = "Bestagon Defence" };

            DontDestroyOnLoad(this);

            // Assign all events for Rich Presence updates
            SceneManager.activeSceneChanged += OnSceneChange;
            GameStats.roundProgress += UpdateActivity;
            GameStats.gameOver += UpdateActivity;
        }

        /// <summary>
        /// Event called when a new scene/level is loaded.
        /// It saves the new scene and updates the "Elapsed" timer.<br/>
        /// Follows the UnityAction&lt;Scene, Scene> pattern.
        /// </summary>
        /// <param name="current">Previous scene</param>
        /// <param name="next">New scene</param>
        private void OnSceneChange(Scene current, Scene next)
        {
            _currentScene = next;
            _activity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            UpdateActivity();
        }

        /// <summary>
        /// Updates the activity to display new stats, such as wave number, remaining lives.
        /// </summary>
        public void UpdateActivity()
        {
            (_activity.Details, _activity.State) = GetState(_currentScene.name);
            _discord.GetActivityManager().UpdateActivity(_activity, (res) => { });
        }

        /// <summary>
        /// Creates 2 strings for use in the Rich Presence display based on the scene name and game state.
        /// </summary>
        /// <param name="sceneName">The scene name for processing</param>
        /// <returns>The <see cref="Activity.Details"/> and <see cref="Activity.State"/> strings</returns>
        private (string, string) GetState(string sceneName)
        {
            // Hard coded scenes
            switch (sceneName)
            {
                case "MainMenu":
                    return ("𝗠𝗮𝗶𝗻 Menu", "");

                case "LevelSelect":
                    return ("𝗟𝗲𝘃𝗲𝗹 Select", "");
            }

            // Any other scene that isn't a level too
            if (sceneName.Substring(sceneName.Length - 5) != "Level")
                return ("Somewhere far away", "");

            // A playable level
            return (
                "𝗣𝗹𝗮𝘆𝗶𝗻𝗴 " + AddSpacesToSentence(sceneName.Substring(0, sceneName.Length - 5)),
                GameStats.lives > 0 ? string.Format("𝗪𝗮𝘃𝗲 {0}｜𝗟𝗶𝘃𝗲𝘀 {1}", GameStats.rounds, GameStats.lives) : "𝗚𝗮𝗺𝗲 Over"
                );
        }

        /// <summary>
        /// Adds spaces before capital letters to a string.
        /// </summary>
        /// <param name="text">String to space</param>
        /// <returns>Spaced <see cref="string"/></returns>
        public static string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        /// <summary>
        /// Runs Discord Rich Presence callbacks every frame.
        /// </summary>
        void Update()
        {
            if (_discord != null)
                _discord.RunCallbacks();
        }

        /// <summary>
        /// Closes down the bridge and clears the Rich Presence Activity of the user.
        /// </summary>
        private void OnDestroy()
        {
            if (_discord == null)
                return;
            _discord.GetActivityManager().ClearActivity((res) => { });
            _discord.RunCallbacks();
            _discord.Dispose();
        }
    }
}
