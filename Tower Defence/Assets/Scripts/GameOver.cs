using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text roundsText;

    public void OnEnable()
    {
        roundsText.text = "<size=4em><b>" + GameStats.rounds + "</b></size>";
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        // TODO - add menu
        Debug.LogError("Menu not created");
        SceneManager.LoadScene("LevelSelect");
    }
}
