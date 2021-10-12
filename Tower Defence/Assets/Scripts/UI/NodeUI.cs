using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NodeUI : MonoBehaviour
    {
        public GameObject ui;
    
        private Node _target;
    
        public TMP_Text upgradeText;
        public Button upgradeButton;
        public TMP_Text sellText;

        // Called when we select a node
        public void SetTarget(Node node)
        {
            _target = node;
        
            // Move the UI to be above the node
            transform.position = _target.transform.position;
        
            // Check if the turret is already upgraded and if we need to enable the upgrade button
            if (_target.isUpgraded)
            {
                upgradeText.text = "<b>Upgrade\nPurchased</b>";
                upgradeButton.interactable = false;
            }
            else
            {
                upgradeText.text = "<b>Upgrade</b>";
                upgradeButton.interactable = true;
            }
        
            // Set sell amount
            sellText.text = "<b>Sell</b>\n<sprite=\"UI-Icons\" name=\"Coin\"> " + _target.turretBlueprint.GetSellAmount();
        
            // Enable the UI
            ui.SetActive(true);
        }
    
        // Hide's the UI
        // Called when we deselect a node
        public void Hide()
        {
            ui.SetActive(false);
        }
    
        // Upgrades the turret
        public void Upgrade()
        {
            _target.UpgradeTurret(null);
        }
    
        // Sells the turret
        public void Sell()
        {
            _target.SellTurret();
        }
    }
}
