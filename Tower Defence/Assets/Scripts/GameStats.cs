using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int money;
    public int startMoney = 200;

    void Start()
    {
        money = startMoney;
    }
}
