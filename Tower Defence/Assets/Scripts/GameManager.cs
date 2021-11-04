using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;

    public GameObject gameOverUI;

    public LevelData.LevelData levelData;

    void Start()
    {
        isGameOver = false;
        if (levelData == null)
        {
            Debug.LogError("No level data set!", this);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        
        if (GameStats.lives <= 0)
        {
            EndGame();
        }
    }
    
    // Called when we reach 0 lives
    private void EndGame()
    {
        isGameOver = true;
        
        gameOverUI.SetActive(true);
    }
}
