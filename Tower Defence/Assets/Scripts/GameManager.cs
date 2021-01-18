using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _gameEnded;

    public GameObject gameOverUI;
    
    // Update is called once per frame
    void Update()
    {
        if (_gameEnded)
        {
            return;
        }
        
        if (GameStats.lives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        _gameEnded = true;
        
        gameOverUI.SetActive(true);
    }
}
