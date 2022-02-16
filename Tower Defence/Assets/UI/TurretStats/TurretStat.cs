using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Displays a single stat on a turret's shop card
    /// </summary>
    public class TurretStat : MonoBehaviour
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI text;
        
        /// <summary>
        /// Sets the colour of the icon
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            icon.color = color;
        }
        
        /// <summary>
        /// Sets the text's content for the stat
        /// </summary>
        /// <param name="data"></param>
        private void SetData(string data)
        {
            text.text = data;
        }
        
        /// <summary>
        /// Sets all data for the stat
        /// </summary>
        /// <param name="data">The data to set</param>
        public void SetData(object data) => SetData(data.ToString());
    }
}
