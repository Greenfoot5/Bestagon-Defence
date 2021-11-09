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
    
        [Header("Upgrades")]
        public GameObject noneText;
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

        // Called when creating the UI
        public void Init (TurretBlueprint turret, Shop shop)
        {
            _turretBlueprint = turret;

            displayName.text = turret.displayName;
            tagline.text = turret.tagline;

            icon.sprite = turret.shopIcon;

            var turretPrefab = turret.prefab.GetComponent<Turret>();
            damage.SetData(turretPrefab.damage);
            rate.SetData(turretPrefab.fireRate);
            range.SetData(turretPrefab.range);

            if (turret.upgrades.Count == 0)
            {
                noneText.SetActive(true);
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
