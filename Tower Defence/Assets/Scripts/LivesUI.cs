using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    // The money counter
    public TMP_Text livesText;

    void Update()
    {
        livesText.text = " <sprite=\"EmojiOne\" name=\"1f60d\"> " + GameStats.lives;
    }
}