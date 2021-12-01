﻿using TMPro;
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
            }
            
            // Display the radius of the turret
            _target.turret.GetComponent<Turret>().Selected();

            // Enable the UI
            ui.SetActive(true);
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

        private void AddStats()
        {
            if (_target.turret == null) return;
            var turret = _target.turret.GetComponent<Turret>();
            stats.text = "<sprite=\"Stats\" name=\"damage\"> " + turret.damage + "\n" +
                         "<sprite=\"Stats\" name=\"range\"> " + turret.range +
                         " <sprite=\"Stats\" name=\"rate\"> " + turret.fireRate;
        }
    }
}
