using Abstract.Data;
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
        /// <param name="handler">The module chain handler the icon is for</param>
        public void SetData(ModuleChainHandler handler)
        {
            icon.SetData(handler);
            text.text = handler.GetDisplayName();
        }
    }
}
