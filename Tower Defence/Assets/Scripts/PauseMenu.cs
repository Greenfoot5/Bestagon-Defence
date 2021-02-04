using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    public void Awake()
    {
        GameStats.controls.Misc.TogglePauseMenu.performed += ctx => Toggle();
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        Debug.Log("Pause Toggled");

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Debug.LogError("No Menu created yet!");
    }
}
