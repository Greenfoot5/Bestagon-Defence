using Abstract;
using MaterialLibrary.Hexagons;
using Modules;
using TMPro;
using Turrets;
using UI.Glyphs;
using UI.TurretStats;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    /// <summary>
    /// Displays the data for a turret shop card
    /// </summary>
    public class TurretSelectionUI : MonoBehaviour
    {
        private TurretBlueprint _turretBlueprint;
    
        // Content
        [Tooltip("The TMP text to display the turret's display name")]
        [SerializeField]
        private TextMeshProUGUI displayName;
        [Tooltip("The TMP text to display the turret's tagline")]
        [SerializeField]
        private TextMeshProUGUI tagline;
        
        [Tooltip("The Image to place the turret's icon")]
        [SerializeField]
        private Image icon;
        [Tooltip("The Image to place the turret's glyph")]
        [SerializeField]
        private Image glyph;
    
        [Header("Modules")]
        [Tooltip("The selection of modules to enable if the turret has any modules")]
        [SerializeField]
        private GameObject modulesSection;
        [Tooltip("The parent of any module icons to display")]
        [SerializeField]
        private GameObject modulesLayout;
        [Tooltip("The prefab of a generic module icon to instantiate under the modulesLayout")]
        [SerializeField]
        private GameObject moduleUI;

        [Header("Stats")]
        [Tooltip("The TurretStat used to display the damage")]
        [SerializeField]
        private TurretStat damage;
        [Tooltip("The TurretStat used to display the fire rate")]
        [SerializeField]
        private TurretStat rate;
        [Tooltip("The TurretStat used to display the range")]
        [SerializeField]
        private TurretStat range;

        [Header("Colors")]
        [Tooltip("The Hexagons shader background of the card")]
        [SerializeField]
        private Hexagons bg;
        [Tooltip("The background Image of the modules section")]
        [SerializeField]
        private Image modulesBg;
        [Tooltip("The title of the module section (so we can set the colour to match the turret)")]
        [SerializeField]
        private TextMeshProUGUI modulesTitle;

        /// <summary>
        /// Creates and setups the Selection UI.
        /// </summary>
        /// <param name="turret">The turret the option selects</param>
        /// <param name="shop">The Shop (allows the game to select the turret when the player clicks the panel)</param>
        /// <param name="lookup">The turret type to glyph lookup</param>
        public void Init (TurretBlueprint turret, Gameplay.Shop shop, TypeSpriteLookup lookup)
        {
            _turretBlueprint = turret;
            
            // Turret text
            displayName.text = turret.displayName;
            tagline.text = turret.tagline;
            
            // Icon and Glyph
            icon.sprite = turret.shopIcon;
            TurretGlyphSo glyphSo = lookup.GetForType(turret.prefab.GetComponent<Turret>().GetType());
            glyph.sprite = glyphSo.glyph;
            glyph.color = glyphSo.body;
            
            // Turret stats
            var turretPrefab = turret.prefab.GetComponent<Turret>();
            damage.SetData(turretPrefab.damage);
            rate.SetData(turretPrefab.fireRate);
            range.SetData(turretPrefab.range);
            
            // Turret's Modules
            if (turret.modules.Count == 0)
            {
                modulesSection.SetActive(false);
            }
            else
            {
                foreach (Module module in turret.modules)
                {
                    GameObject mod = Instantiate(moduleUI, modulesLayout.transform);
                    mod.GetComponentInChildren<TurretModulesIcon>().SetData(module);
                }
            }

            // Colors
            tagline.color = turret.accent;
            modulesTitle.color = turret.accent;
            bg.color = turret.accent;
            modulesBg.color = turret.accent * new Color(1, 1, 1, .16f);

            damage.SetColor(turret.accent);
            rate.SetColor(turret.accent);
            range.SetColor(turret.accent);
            
            // Adds the click event to the card
            bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        /// <summary>
        /// Called when a player clicks the card,
        /// selecting it and closing the shop
        /// </summary>
        /// <param name="shop"></param>
        private void MakeSelection (Gameplay.Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewTurret(_turretBlueprint);
        }
    }
}
