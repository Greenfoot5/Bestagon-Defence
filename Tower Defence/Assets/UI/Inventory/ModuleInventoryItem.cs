using Abstract.Data;
using MaterialLibrary.Hexagons;
using TMPro;
using UI.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ModuleInventoryItem : MonoBehaviour
    {
        private ModuleChainHandler _module;
        
        [Tooltip("The TMP text to display the module's display name")]
        [SerializeField]
        private TMP_Text displayName;
        [Tooltip("The TMP text to display the module's tagline")]
        [SerializeField]
        private TMP_Text tagline;
        [Tooltip("The text to set for the module's effect")]
        [SerializeField]
        private TMP_Text effectText;
        
        [Tooltip("The ModuleIcon for the module card")]
        [SerializeField]
        private ModuleIcon icon;
        
        [Header("Colors")]
        [Tooltip("The Hexagons shader background of the card")]
        [SerializeField]
        private Hexagons bg;
        [Tooltip("The background Image of the modules section")]
        [SerializeField]
        private Image modulesBg;

        /// <summary>
        /// Creates and setups the Selection UI.
        /// </summary>
        /// <param name="module">The module the option selects</param>
        public void Init (ModuleChainHandler module)
        {
            _module = module;
            
            // Module text
            displayName.text = module.GetDisplayName();
            tagline.text = module.GetChain().tagline.GetLocalizedString();
            effectText.text = module.GetChain().description.GetLocalizedString();
            
            // Icon
            icon.SetData(module);

            // Colors
            bg.color = module.GetChain().accentColor;
            modulesBg.color = module.GetChain().accentColor * new Color(1, 1, 1, .16f);
        }
    }
}
