using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    private bool _hasBeenToggled = false;

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

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

    public void Update()
    {
        if (Input.GetAxis("Pause") > 0 && !_hasBeenToggled)
        {
            Toggle();
            _hasBeenToggled = true;
        }
        else if (Input.GetAxis("Pause") == 0)
        {
            _hasBeenToggled = false;
        }
    }
}
