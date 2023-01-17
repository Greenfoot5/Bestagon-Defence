using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Allows a piece of text to have a random hue when loading the scene
    /// </summary>
    public class RandomHueOnLoad : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text[] texts;
        
        /// <summary>
        /// Generates a random hue for the text
        /// </summary>
        private void Start()
        {
            float hue = Random.value;
            foreach (TMP_Text text in texts)
            {
                // Load the old colours
                Color.RGBToHSV(text.color, out float h, out float s, out float v);

                // Randomise the hue and assign
                h = hue;
                text.color = Color.HSVToRGB(h, s, v);
            }
        }
    }
}
