using TMPro;
using Turrets.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    /// <summary>
    /// Updates the module icon with data
    /// </summary>
    public class ModuleIcon : MonoBehaviour
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI text;

        private static readonly string[] Levels = { "-", "I", "II", "III", "IV", "V" };
        
        /// <summary>
        /// Sets the sprite for the module icon
        /// </summary>
        /// <param name="sprite"></param>
        private void SetSprite(Sprite sprite)
        {
            icon.sprite = sprite;
        }
        
        /// <summary>
        /// Sets the level text of the module icon
        /// </summary>
        /// <param name="level">The module tier</param>
        private void SetLevel(int level)
        {
            text.text = Levels[level];
        }
        
        /// <summary>
        /// Sets all values for the module icon
        /// </summary>
        /// <param name="module">The module to display the icon for</param>
        public void SetData(Module module)
        {
            SetSprite(module.icon);
            SetLevel(module.ModuleTier);
        }
    }
}
