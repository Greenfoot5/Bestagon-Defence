using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    // The money counter
    public TMP_Text waveCountdownText;

    void Update()
    {
        waveCountdownText.text = "<sprite=\"UI\" name=\"Coin\">" + GameStats.money;
    }
}
