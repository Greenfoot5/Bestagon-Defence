using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemSingleton : MonoBehaviour
{
    private static GameSystemSingleton _instance;

    /// <summary>
    /// Singleton pattern. Marks the object with <see cref="Object.DontDestroyOnLoad"/><br/>
    /// Deletes the <b>@System</b> game object if one already exists
    /// </summary>
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;
    }
}
