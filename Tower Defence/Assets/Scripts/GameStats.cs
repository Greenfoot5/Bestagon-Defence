using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 200;

    public static int lives;
    public int startLives = 20;

    public static int rounds;

    private void Start()
    {
        money = startMoney;
        lives = startLives;
        rounds = 0;
    }
}
