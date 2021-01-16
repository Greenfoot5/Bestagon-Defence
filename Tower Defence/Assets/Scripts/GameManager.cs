using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _gameEnded;
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
        Debug.Log("Game Over!");
        _gameEnded = true;
    }
}
