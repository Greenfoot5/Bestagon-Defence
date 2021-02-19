using TMPro;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text livesText;
    public TMP_Text waveCountText;

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "<sprite=\"UI-Icons\" name=\"Coin\"> " + GameStats.money;
        livesText.text = "<sprite=\"UI-Icons\" name=\"Heart\"> " + GameStats.lives;
        waveCountText.text = " # " + GameStats.rounds;
    }
}
