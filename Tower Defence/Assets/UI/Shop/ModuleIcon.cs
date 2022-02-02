using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades.Modules;

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

        private Module _module;

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
        /// <param name="newModule">The module to display the icon for</param>
        public void SetData(Module newModule)
        {
            _module = newModule;
            SetSprite(newModule.icon);
            SetLevel(newModule.moduleTier);
        }

        public Module GetModule()
        {
            return _module;
        }
    }
}
