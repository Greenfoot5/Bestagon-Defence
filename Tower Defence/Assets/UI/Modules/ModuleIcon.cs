using Abstract.Data;
using Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Modules
{
    /// <summary>
    /// Updates the module icon with data
    /// </summary>
    public class ModuleIcon : MonoBehaviour
    {
        [Tooltip("The Image object to set the icon of")]
        [SerializeField]
        private Image icon;
        [SerializeField]
        [Tooltip("The level text to set as the module's level")]
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
        
        /// <summary>
        /// Sets all values for the module icon
        /// </summary>
        /// <param name="handler">The module chain handler to display the icon for</param>
        public void SetData(ModuleChainHandler handler)
        {
            _module = handler.GetModule();
            SetSprite(handler.GetChain().icon);
            text.text = handler.GetTierDisplay();
        }
        
        /// <summary>
        /// Gets the module the icon is for
        /// </summary>
        /// <returns>The module the icon represents</returns>
        public Module GetModule()
        {
            return _module;
        }
    }
}
