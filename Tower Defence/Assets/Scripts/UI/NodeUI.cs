using Turrets;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NodeUI : MonoBehaviour
    {
        public GameObject ui;
        public Shop shop;
    
        private Node _target;
    
        public Transform upgrades;
        public GameObject upgradeIconPrefab;

        // Called when we select a node
        public void SetTarget(Node node)
        {
            _target = node;

            shop.EnableUpgradeInventory();
        
            // Move the UI to be above the node
            transform.position = _target.transform.position;
            
            upgrades.DetachChildren();
            
            // Add each upgrade as an icon
            foreach (var upgrade in _target.turret.GetComponent<Turret>().upgrades)
            {
                var upgradeIcon = Instantiate(upgradeIconPrefab, upgrades);
                upgradeIcon.GetComponent<UpgradeIcon>().SetData(upgrade);
            }
            
            // Display the radius of the turret
            _target.turret.GetComponent<Turret>().Selected();

            // Enable the UI
            ui.SetActive(true);
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) upgrades);
        }
    
        // Hide's the UI
        // Called when we deselect a node
        public void Hide()
        {
            ui.SetActive(false);
            shop.EnableTurretInventory();
        }
    
        // Upgrades the turret
        public void UpgradeNode()
        {
            var upgrade = shop.GetUpgrade();
            if (upgrade == null) return;
            
            var applied = _target.UpgradeTurret(upgrade);
            if (!applied) return;
            
            shop.RemoveUpgrade();
            }
    
        // Sells the turret
        public void Sell()
        {
            _target.SellTurret();
        }
    }
}
