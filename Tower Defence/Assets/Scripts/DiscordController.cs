using System;
using System.Collections;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    private Discord.Discord _discord;
    private Activity _richPresence;
    
    public const long ClientId = 927337431303352441;

    public static DiscordController instance;

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
            UpdateDiscord(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
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
    
    private void UpdatePresence(Scene next)
    {
        var timestamps = new ActivityTimestamps
        {
            Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()
        };
        var assets = new ActivityAssets()
        {
            LargeImage = "hexagon",
            LargeText = next.name,
            SmallImage = "bestagon_logo_square_large",
            SmallText = "Hexagons are the Bestagons!"
        };
        
        var activity = new Activity
        {
            State = "Wave 5",
            Details = "Hexagon Level",
            Timestamps = timestamps,
            Assets = assets
        };
        
        _discord.GetActivityManager().UpdateActivity(activity, HandleResult);
    }

    private static void HandleResult(Result res)
    {
        if (res == Result.NotRunning || res == Result.InternalError)
        {
            return;
        }

        throw new ResultException(res);
    }
}