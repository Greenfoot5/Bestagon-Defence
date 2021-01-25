using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    // The money counter
    public TMP_Text livesText;

    void Update()
    {
        livesText.text = "<sprite=\"UI-Icons\" name=\"Heart\"> " + GameStats.lives;
    }
}