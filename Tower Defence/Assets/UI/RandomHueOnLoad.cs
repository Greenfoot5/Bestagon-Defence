using TMPro;
using UnityEngine;

namespace UI
{
    public class RandomHueOnLoad : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        private void Start()
        {
            // Load the old colours
            float h;
            float s;
            float v;
            Color.RGBToHSV(text.color, out h, out s, out v);
        
            // Randomise the hue and assign
            h = Random.value;
            text.color = Color.HSVToRGB(h, s, v);
        }
    }
}
