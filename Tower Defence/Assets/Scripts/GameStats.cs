using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int gold;
    public int startGold = 200;

    void Start()
    {
        gold = startGold;
    }
}
