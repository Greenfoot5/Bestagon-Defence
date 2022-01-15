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
    
        [Header("Upgrades")]
        public GameObject upgradesSection;
        public GameObject upgradesLayout;
        public GameObject upgradeUI;

        [Header("Stats")]
        public TurretStat damage;
        public TurretStat rate;
        public TurretStat range;

        [Header("Colors")]
        public Hexagons bg;
        public Image upgradesBG;
        public TextMeshProUGUI upgradesTitle;

        /// <summary>
        /// Creates and sets up the Selection UI.
        /// </summary>
        /// <param name="turret">The turret the option selects</param>
        /// <param name="shop">The Shop (allows us to select the turret when we click the panel</param>
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
            
            // Turret's upgrades
            if (turret.upgrades.Count == 0)
            {
                upgradesSection.SetActive(true);
            }
            else
            {
                foreach (var upgrade in turret.upgrades)
                {
                    var up = Instantiate(upgradeUI, upgradesLayout.transform);
                    up.GetComponentInChildren<TurretUpgradesUpgrade>().SetData(upgrade);
                }
            }

            // Colors
            tagline.color = turret.accent;
            upgradesTitle.color = turret.accent;
            bg.color = turret.accent;
            upgradesBG.color = turret.accent * new Color(1, 1, 1, .16f);

            damage.SetColor(turret.accent);
            rate.SetColor(turret.accent);
            range.SetColor(turret.accent);

            bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        // Called when the user clicks on the button
        private void MakeSelection (Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewTurret(_turretBlueprint);
        }
    }
}
