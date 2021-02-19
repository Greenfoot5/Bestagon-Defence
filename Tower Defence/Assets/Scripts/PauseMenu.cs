using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    
    private bool _hasBeenToggled;

    // Pauses/unpauses the game, and toggles the UI
    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
            Input.ResetInputAxes();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    
    // The retry button, reloads the current scene
    public void Retry()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // The Main Menu button that returns us to the main menu
    // TODO - return to level select, not main menu
    public void Menu()
    {
        Toggle();
        SceneManager.LoadScene("MainMenu");
    }
    
    // Called each frame
    public void Update()
    {
        if (Input.GetAxis("Pause") != 0)
            Debug.Log(Input.GetAxis("Pause"));
        
        // On press, pause the game
        if (Input.GetAxis("Pause") > 0 && !_hasBeenToggled)
        {
            Toggle();
            _hasBeenToggled = true;
        }
        // Set's _hasBeenToggled on release
        else if (Input.GetAxis("Pause") == 0)
        {
            _hasBeenToggled = false;
        }
    }
}
