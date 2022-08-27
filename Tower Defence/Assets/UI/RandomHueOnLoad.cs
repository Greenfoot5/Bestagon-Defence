using TMPro;
using UnityEngine;

namespace UI
{
    public class RandomHueOnLoad : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text[] texts;

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
