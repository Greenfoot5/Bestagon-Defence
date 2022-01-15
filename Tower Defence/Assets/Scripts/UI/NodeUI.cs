using System;
using TMPro;
using Turrets;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Manages the UI that displays when you click on a node with a turret
    /// </summary>
    public class NodeUI : MonoBehaviour
    {
        public GameObject ui;
        public Shop shop;
    
        private Node _target;
    
        public Transform upgrades;
        public GameObject upgradeIconPrefab;

        public TMP_Text stats;

        public GameObject cycleTargetingButton;

        /// <summary>
        /// Called when selecting a new node
        /// </summary>
        /// <param name="node">The new node to display UI for</param>
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
                foreach (var image in upgradeIcon.GetComponentsInChildren<Image>())
                {
                    image.raycastTarget = false;
                }

                upgradeIcon.GetComponentsInChildren<Image>();
            }
            
            // Display the radius of the turret
            _target.turret.GetComponent<Turret>().Selected();

            // Enable the UI
            ui.SetActive(true);
            
            // Enable/Disable Targeting types cycle button if it's (not) a dynamic turret.
            if (_target.turret.GetComponent<Turret>() is DynamicTurret)
            {
                cycleTargetingButton.SetActive(true);
                cycleTargetingButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "<b>Targeting:</b>\n" +
                    _target.turret.GetComponent<DynamicTurret>().targetingMethod;
            }
            else
            {
                cycleTargetingButton.SetActive(false);
            }
            

            // Rebuild the upgrades and add the stats
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) upgrades);
            AddStats();
        }
    
        /// <summary>
        /// Hides the UI
        /// Is called when deselecting a node
        /// </summary>
        public void Hide()
        {
            ui.SetActive(false);
            shop.EnableTurretInventory();
        }
    
        /// <summary>
        /// Upgrades the currently selected turret
        /// </summary>
        public void UpgradeNode()
        {
            var upgrade = shop.GetUpgrade();
            if (upgrade == null) return;
            
            var applied = _target.UpgradeTurret(upgrade);
            if (!applied) return;
            
            shop.RemoveUpgrade();
        }
        
        /// <summary>
        /// Turret's targeting method increments once through the cycle of targeting methods
        /// </summary>
        public void CycleTargeting()
        {
            var types = Enum.GetValues(typeof(TargetingMethod));
            var currentMethod = (int)_target.turret.GetComponent<DynamicTurret>().targetingMethod;
            _target.turret.GetComponent<DynamicTurret>().targetingMethod = (TargetingMethod)( (currentMethod + 1) % types.Length);
            
            // Update our button text
            cycleTargetingButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "<b>Targeting:</b>\n" +
                _target.turret.GetComponent<DynamicTurret>().targetingMethod;
        }

        private void AddStats()
        {
            if (_target.turret == null) return;
            var turret = _target.turret.GetComponent<Turret>();
            var color = ColorUtility.ToHtmlStringRGBA(stats.color);
            stats.text = $"<sprite=\"Stats\" name=\"damage\" color=#{color}> {turret.damage}\n" +
                         $"<sprite=\"Stats\" name=\"range\" color=#{color}> {turret.range}\n" +
                         $" <sprite=\"Stats\" name=\"rate\" color=#{color}> {turret.fireRate}";
        }
    }
}
