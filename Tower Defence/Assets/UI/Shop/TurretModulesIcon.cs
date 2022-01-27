using TMPro;
using UnityEngine;
using Upgrades.Modules;

namespace UI.Shop
{
    /// <summary>
    /// Sets the module icon for each module in a turret's upgrades on selection
    /// </summary>
    public class TurretModulesIcon : MonoBehaviour
    {
        [SerializeField]
        private ModuleIcon icon;
        [SerializeField]
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
