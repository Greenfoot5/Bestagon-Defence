using System;
using Levels;
using TMPro;
using Turrets;
using UI.Shop;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Level
{
    /// <summary>
    /// Manages the UI that displays when you click on a node with a turret
    /// </summary>
    public class NodeUI : MonoBehaviour
    {
        public static NodeUI instance;
        
        public GameObject ui;
        public Levels.Shop shop;
    
        private Node _target;
    
        public Transform modules;
        public GameObject moduleIconPrefab;

        public TMP_Text stats;

        public GameObject cycleTargetingButton;
        
        /// <summary>
        /// Check there is only one NodeUI when loading in
        /// </summary>
        private void Awake()
        {
            // Make sure there is only ever have one NodeUI
            if (instance != null)
            {
                Debug.LogError("More than one NodeUI in scene!");
                return;
            }
            instance = this;
        }

        /// <summary>
        /// Called when selecting a new node
        /// </summary>
        /// <param name="node">The new node to display UI for</param>
        public void SetTarget(Node node)
        {
            _target = node;

            shop.EnableModuleInventory(node.turret.GetComponent<Turret>());
        
            // Move the UI to be above the node
            transform.position = _target.transform.position;
            
            modules.DetachChildren();
            
            // Add each Module as an icon
            foreach (var module in _target.turret.GetComponent<Turret>().modules)
            {
                var moduleIcon = Instantiate(moduleIconPrefab, modules);
                moduleIcon.GetComponent<ModuleIcon>().SetData(module);
                foreach (var image in moduleIcon.GetComponentsInChildren<Image>())
                {
                    image.raycastTarget = false;
                }

                moduleIcon.GetComponentsInChildren<Image>();
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
            

            // Rebuild the Modules and add the stats
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) modules);
            UpdateStats();
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
        /// Applies a module to the currently selected turret
        /// </summary>
        public void ModuleNode()
        {
            var module = shop.GetModule();
            if (module == null) return;
            
            var applied = _target.ModuleTurret(module);
            if (!applied) return;
            
            shop.RemoveModule();
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
        
        /// <summary>
        /// Updates the stats display when the turret is selected or upgraded
        /// </summary>
        public void UpdateStats()
        {
            if (_target.turret == null) return;
            var turret = _target.turret.GetComponent<Turret>();
            var color = ColorUtility.ToHtmlStringRGBA(stats.color);
            stats.text = $"<sprite=\"Stats\" name=\"damage\" color=#{color}> {turret.damage}\n" +
                         $"<sprite=\"Stats\" name=\"range\" color=#{color}> {turret.range}\n" +
                         $" <sprite=\"Stats\" name=\"rate\" color=#{color}> {turret.fireRate.ToString()}";
        }
        
        /// <summary>
        /// Gets the turret the NodeUI is targeting
        /// </summary>
        /// <returns>The turret the NodeUI is targeting</returns>
        public GameObject GetTurret()
        {
            return _target != null ? _target.turret : null;
        }
    }
}
