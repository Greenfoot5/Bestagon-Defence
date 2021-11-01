using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;

    public GameObject gameOverUI;

    public LevelData.LevelData levelData;
    private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

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

        Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
    }
    
    // Called when we reach 0 lives
    private void EndGame()
    {
        isGameOver = true;
        
        gameOverUI.SetActive(true);
    }
}
