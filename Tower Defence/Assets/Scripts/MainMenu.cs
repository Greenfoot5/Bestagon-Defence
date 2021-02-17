using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(String levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Settings()
    {
        
    }

    public void Quit()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }
}
