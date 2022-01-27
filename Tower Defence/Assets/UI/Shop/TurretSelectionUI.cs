using Abstract.Data;
using Levels;
using Shaders.Hexagons;
using TMPro;
using Turrets;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurretSelectionUI : MonoBehaviour
    {
        private TurretBlueprint _turretBlueprint;
    
        // Content
        public TextMeshProUGUI displayName;
        public TextMeshProUGUI tagline;

        public Image icon;
        public Image glyph;
    
        [Header("Modules")]
        public GameObject modulesSection;
        public GameObject modulesLayout;
        public GameObject moduleUI;

        [Header("Stats")]
        public TurretStat damage;
        public TurretStat rate;
        public TurretStat range;

        [Header("Colors")]
        public Hexagons bg;
        public Image modulesBg;
        public TextMeshProUGUI modulesTitle;

        /// <summary>
        /// Creates and setups the Selection UI.
        /// </summary>
        /// <param name="turret">The turret the option selects</param>
        /// <param name="shop">The Shop (allows the game to select the turret when the player clicks the panel)</param>
        /// <param name="lookup">The turret type to glyph lookup</param>
        public void Init (TurretBlueprint turret, Shop shop, TypeSpriteLookup lookup)
        {
            _turretBlueprint = turret;
            
            // Turret text
            displayName.text = turret.displayName;
            tagline.text = turret.tagline;
            
            // Icon and Glyph
            icon.sprite = turret.shopIcon;
            var glyphSo = lookup.GetForType(turret.prefab.GetComponent<Turret>().GetType());
            glyph.sprite = glyphSo.glyph;
            glyph.color = glyphSo.body;
            
            // Turret stats
            var turretPrefab = turret.prefab.GetComponent<Turret>();
            damage.SetData(turretPrefab.damage);
            rate.SetData(turretPrefab.fireRate);
            range.SetData(turretPrefab.range);
            
            // Turret's Modules
            if (turret.Modules.Count == 0)
            {
                modulesSection.SetActive(true);
            }
            else
            {
                foreach (var module in turret.Modules)
                {
                    var mod = Instantiate(moduleUI, modulesLayout.transform);
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

            bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        // Called when the player clicks on the button
        private void MakeSelection (Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewTurret(_turretBlueprint);
        }
    }
}
