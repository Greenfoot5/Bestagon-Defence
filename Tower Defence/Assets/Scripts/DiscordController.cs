using System;
using System.Collections;
using System.Text;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    private Discord.Discord _discord;
    private Activity _richPresence;

    private const long ClientId = 927337431303352441;

    private static DiscordController _instance;
    
    public SceneState[] sceneStates;
    
    /// <summary>
    /// If we can't find Discord, check every 30s to see if the user opened it
    /// </summary>
    private IEnumerator CreateDiscord()
    {
        while (_discord == null)
        {
            try
            {
                _discord = new Discord.Discord(ClientId, (ulong)CreateFlags.NoRequireDiscord);
            }
            catch (ResultException res)
            {
                Debug.LogWarning(res);
            }
            
            yield return new WaitForSeconds(30);
        }
    }

    /// <summary>
    /// Attempts to connect to discord, and sets up the rest of the stuff
    /// </summary>
    private void Start ()
    {
        // Make sure we only ever have one DiscordController
        if (_instance != null)
        {
            Debug.LogWarning("More than one Discord Controller in scene!");
            if (this != _instance)
            {
                Destroy(gameObject);
            }

            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CreateDiscord());

        if (_discord != null)
        {
            Refresh();
        }

        SceneManager.activeSceneChanged += UpdateDiscord;
    }
	
    /// <summary>
    /// Called when we need to update the presence for the user
    /// Note, current and next may be the same if we aren't changing scenes (e.g. the user beat the level)
    /// </summary>
    /// <param name="current">The scene we're leaving</param>
    /// <param name="next">The scene we're loading next</param>
    private void UpdateDiscord (Scene current, Scene next)
    {
        try
        {
            _discord.RunCallbacks();
            UpdatePresence(next);
        }
        catch (ResultException res)
        {
            if (res.Result != Result.NotRunning)
            {
                throw;
            }
        }
        catch (NullReferenceException)
        {
            if (_discord != null)
            {
                throw;
            }
        }
    }
    
    /// <summary>
    /// Called in other scripts to update the rich presence
    /// </summary>
    public static void Refresh()
    {
        _instance.UpdateDiscord(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }
    
    /// <summary>
    /// Used to actually update the presence
    /// </summary>
    /// <param name="scene">The scene the user is in</param>
    private void UpdatePresence(Scene scene)
    {
        var timestamps = new ActivityTimestamps
        {
            Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()
        };
        var assets = GetAssets(scene);
        var (state, desc) = GetGameState(scene);

        var activity = new Activity
        {
            State = state,
            Details = desc,
            Timestamps = timestamps,
            Assets = assets
        };
        
        _discord.GetActivityManager().UpdateActivity(activity, HandleResult);
    }

    private static void HandleResult(Result res)
    {
        if (res == Result.NotRunning || res == Result.InternalError || res == Result.Ok)
        {
            return;
        }

        throw new ResultException(res);
    }
    
    /// <summary>
    /// Gets the ActivityAssets to display
    /// </summary>
    /// <param name="scene">The scene the player is currently in</param>
    /// <returns>The assets to display in the rich presence</returns>
    private static ActivityAssets GetAssets(Scene scene)
    {
        var assets = new ActivityAssets()
        {
            LargeImage = "bestagon_logo_square_large",
            LargeText = "Main Menu",
            SmallImage = "bestagon_logo_square_large",
            SmallText = "Hexagons are the Bestagons!"
        };
        if (scene.name.Substring(scene.name.Length - 5) != "Level") return assets;
        
        assets.LargeText = scene.name.Substring(0, scene.name.Length - 5);
        assets.LargeImage = scene.name.Substring(0, scene.name.Length - 5).ToLower();

        return assets;
    }
    
    /// <summary>
    /// Gets the current state of the game, so what the user is doing
    /// </summary>
    /// <param name="scene">The scene the user is currently in</param>
    /// <returns>The State and Details to display in the rich presence</returns>
    private (string, string) GetGameState(Scene scene)
    {
        var result = (State : "Defending the Hexagons", Desc : "");
        // We're in a level
        if (scene.name.Substring(scene.name.Length - 5) == "Level")
        {
            result.Desc = "Playing on " + AddSpacesToSentence(scene.name.Substring(0, scene.name.Length - 5));
            
            result.State = (GameStats.lives > 0) switch
            {
                true when GameStats.rounds == 0 => "Preparation Phase",
                true => "Wave " + GameStats.rounds,
                _ => "Game Over"
            };

            return result;
        }
        // We're got custom details for the scene
        foreach (var sceneState in sceneStates)
        {
            if (scene.name != sceneState.sceneName) continue;
            
            result.State = sceneState.state;
            result.Desc = sceneState.desc;
        }

        return result;
    }
    
    /// <summary>
    /// Adds a space before every capital if it doesn't have one already.
    /// Useful for translating scene names into displayed text
    /// </summary>
    /// <param name="text">The text to add spaces to</param>
    /// <returns>The string with spaces</returns>
    private static string AddSpacesToSentence(string text)
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

    // Clean up when we finish
    private void OnDestroy() => _discord.GetActivityManager().ClearActivity(result => {});
    private void OnApplicationQuit() => _discord.Dispose();
}