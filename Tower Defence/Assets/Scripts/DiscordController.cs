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

    public static DiscordController instance;
    
    public SceneState[] sceneStates;

    private IEnumerator CreateDiscord()
    {
        while (_discord == null)
        {
            Debug.Log("Attempting to find Discord");
            try
            {
                _discord = new Discord.Discord(ClientId, (ulong)CreateFlags.NoRequireDiscord);
                Debug.Log("Found Discord!");
            }
            catch (ResultException res)
            {
                Debug.LogWarning(res);
            }
            
            yield return new WaitForSeconds(30);
        }
    }

    // Use this for initialization
    private void Start ()
    {
        // Make sure we only ever have one DiscordController
        if (instance != null)
        {
            Debug.LogWarning("More than one Discord Controller in scene!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CreateDiscord());

        if (_discord != null)
        {
            Refresh();
        }

        SceneManager.activeSceneChanged += UpdateDiscord;
    }
	
    // Called anytime we want to update Discord
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

    public void Refresh()
    {
        UpdateDiscord(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }
    
    private void UpdatePresence(Scene next)
    {
        var timestamps = new ActivityTimestamps
        {
            Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()
        };
        var assets = GetAssets(next);
        var (state, desc) = GetGameState(next);

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

    private static ActivityAssets GetAssets(Scene next)
    {
        Debug.Log(next.name.Substring(0, next.name.Length - 5));
        var assets = new ActivityAssets()
        {
            LargeImage = "bestagon_logo_square_large",
            LargeText = "Main Menu",
            SmallImage = "bestagon_logo_square_large",
            SmallText = "Hexagons are the Bestagons!"
        };
        if (next.name.Substring(0, next.name.Length - 5) != "Level") return assets;
        
        assets.LargeText = next.name.Substring(0, next.name.Length - 5);
        assets.LargeImage = next.name.Substring(0, next.name.Length - 5).ToLower();

        return assets;
    }

    private (string, string) GetGameState(Scene next)
    {
        var result = (State : "Defending the Hexagons", Desc : "");
        // We're in a level
        if (next.name.Substring(0, next.name.Length - 5) == "Level")
        {
            result.Item1 = AddSpacesToSentence(next.name.Substring(next.name.Length - 5));
            if (GameStats.lives > 0)
            {
                result.State = "Wave " + GameStats.rounds;
            }
            else
            {
                result.Desc = "Game Over";
            }

            return result;
        }
        
        foreach (var sceneState in sceneStates)
        {
            if (next.name != sceneState.sceneName) continue;
            
            result.State = sceneState.state;
            result.Desc = sceneState.desc;
        }

        return result;
    }
    
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

    private void OnApplicationQuit()
    {
        Debug.Log("Quit the application");
        _discord.GetActivityManager().ClearActivity(result => {});
    }
}