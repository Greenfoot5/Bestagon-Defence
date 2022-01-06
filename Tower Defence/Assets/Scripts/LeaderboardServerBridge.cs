using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardServerBridge : MonoBehaviour
{
    public string serverEndpoint = "https://exploitavoid.com/leaderboards/v1/api";
    
    /// <summary>
    /// Gets some entries from a leaderboard
    /// </summary>
    /// <param name="count">The number of entries to obtain</param>
    /// <param name="leaderboardID">The id of the leaderboard to request data from</param>
    /// <param name="start">The position to start at, defaults to one</param>
    /// <returns>The leaderboard entries</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<List<LeaderboardEntry>> RequestEntries(int count, string leaderboardID, int start = 1)
    {
        var url = serverEndpoint + $"/get_entries?leaderboard_id={leaderboardID}&start={start}&count={count}";
        using var unityWebRequest = UnityWebRequest.Get(url);
        await unityWebRequest.SendWebRequestAsync();
        switch (unityWebRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(unityWebRequest.error);
                Debug.LogError(unityWebRequest.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                var scores = DeserializeJson<List<LeaderboardEntry>>(unityWebRequest.downloadHandler.text);
                return scores;
            case UnityWebRequest.Result.InProgress:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }
    
    /// <summary>
    /// Gets the user's 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="leaderboardID"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<LeaderboardEntry> RequestUserEntry(string username, string leaderboardID)
    {
        var url = serverEndpoint + $"/get_entries?leaderboard_id={leaderboardID}&start=1&count=1&search={username}";
        using var unityWebRequest = UnityWebRequest.Get(url);
        await unityWebRequest.SendWebRequestAsync();
        switch (unityWebRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(unityWebRequest.error);
                Debug.LogError(unityWebRequest.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                var scores = DeserializeJson<List<LeaderboardEntry>>(unityWebRequest.downloadHandler.text);
                if (scores != null && scores.Count > 0)
                {
                    return scores[0];
                }
                return null;
            case UnityWebRequest.Result.InProgress:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }
    
    /// <summary>
    /// Sends a value to the leaderboard
    /// </summary>
    /// <param name="username">The username to file the data under</param>
    /// <param name="value">The user's score</param>
    /// <param name="leaderboardID">The leaderboard id</param>
    /// <param name="leaderboardSecret">The secret</param>
    /// <returns></returns>
    public Task<bool> SendUserValue(string username, int value, string leaderboardID, string leaderboardSecret)
    {
        return SendUserValue(username, (IConvertible)value, leaderboardID, leaderboardSecret);
    }

    public Task<bool> SendUserValue(string username, float value, string leaderboardID, string leaderboardSecret)
    {
        return SendUserValue(username, (IConvertible)value, leaderboardID, leaderboardSecret);
    }

    public Task<bool> SendUserValue(string username, double value, string leaderboardID, string leaderboardSecret)
    {
        return SendUserValue(username, (IConvertible)value, leaderboardID, leaderboardSecret);
    }

    private async Task<bool> SendUserValue(string username, IConvertible value, string leaderboardID, string leaderboardSecret)
    {
        var url = serverEndpoint + "/update_entry";
        var valueString = value.ToString();
        var uploadJson = $"{{\"name\":\"{username}\", \"value\":{valueString}, \"leaderboard_id\":{leaderboardID}}}";
        var toHash = "/update_entry" + uploadJson + leaderboardSecret;

        var utfBytes = Encoding.UTF8.GetBytes(toHash);
        SHA256 shaM = new SHA256Managed();
        var result = shaM.ComputeHash(utfBytes);

        var hashString = BitConverter.ToString(result).Replace("-", "");

        var rawBytes = Encoding.UTF8.GetBytes(uploadJson + hashString);

        var d = new DownloadHandlerBuffer();
        var u = new UploadHandlerRaw(rawBytes);
        using var unityWebRequest = new UnityWebRequest(url, "POST", d, u);
        await unityWebRequest.SendWebRequestAsync();
        switch (unityWebRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(unityWebRequest.error);
                Debug.LogError(unityWebRequest.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                return true;
            case UnityWebRequest.Result.InProgress:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    private static T DeserializeJson<T>(string result)
    {
        var jsonSer = new DataContractJsonSerializer(typeof(T));
        using var ms = new MemoryStream(Encoding.UTF8.GetBytes(result))
        {
            Position = 0
        };
        return (T)jsonSer.ReadObject(ms);
    }
}

public static class WebRequestAsyncExtension
{
    public static Task<AsyncOperation> SendWebRequestAsync(this UnityWebRequest unityWebRequest)
    {
        var taskCompletionSource = new TaskCompletionSource<AsyncOperation>();
        unityWebRequest.SendWebRequest().completed += x => taskCompletionSource.SetResult(x);
        return taskCompletionSource.Task;
    }
}

[DataContract]
public class LeaderboardEntry
{
    [DataMember]
    public readonly string name;
    [DataMember(Name = "value")]
    private readonly string _value;
    [DataMember]
    public readonly int position;

    // If you want to parse the value yourself
    // The backend does not support strings as values
    public string GetValueAsString()
    {
        return _value;
    }

    public int GetValueAsInt()
    {
        if(int.TryParse(_value, out var result))
        {
            return result;
        }
        else
        {
            return (int)Math.Round(GetValueAsDouble());
        }
    }

    public float GetValueAsFloat()
    {
        return float.Parse(_value);
    }

    private double GetValueAsDouble()
    {
        return double.Parse(_value);
    }
}
