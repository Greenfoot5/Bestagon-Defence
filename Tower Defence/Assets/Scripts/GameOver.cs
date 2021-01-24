using System;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TMP_Text roundsText;

    public void OnEnable()
    {
        roundsText.text = "<size=4em><b>" + GameStats.rounds + "</b></size>";
    }
}
