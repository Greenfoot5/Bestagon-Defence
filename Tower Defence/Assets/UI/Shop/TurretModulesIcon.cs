using Modules;
using TMPro;
using UI.Modules;
using UnityEngine;

namespace UI.Shop
{
    /// <summary>
    /// Sets the module icon for each module in a turret's upgrades on selection
    /// </summary>
    public class TurretModulesIcon : MonoBehaviour
    {
        [Tooltip("The ModuleIcon of the module")]
        [SerializeField]
        private ModuleIcon icon;
        [SerializeField]
        [Tooltip("The TMP text to display the display name of the module")]
        private TextMeshProUGUI text;
        
        /// <summary>
        /// Sets the data
        /// </summary>
        /// <param name="module">The module the icon is for</param>
        public void SetData(Module module)
        {
            icon.SetData(module);
            text.text = module.displayName;
        }
    }
}
