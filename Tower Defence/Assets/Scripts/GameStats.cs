using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 200;

    public static int lives;
    public int startLives = 20;

    public static int rounds;

    public static Controls controls;

    void Awake()
    {
        controls = new Controls();
        controls.Enable();
        TouchSimulation.Enable();
    }

    void Start()
    {
        money = startMoney;
        lives = startLives;
        rounds = 0;
    }
}
