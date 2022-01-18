using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 200;

    public static int lives;
    public int startLives = 20;

    public static int rounds;
    
    public static GameControls controls;

    private void Awake()
    {
        money = startMoney;
        lives = startLives;
        rounds = 0;
        
        // Controls
        controls = new GameControls();
        controls.Enable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }
}
