using System;
using Abstract.Data;
using Gameplay;
using Levels._Nodes;
using TMPro;
using Turrets;
using Turrets.Lancer;
using UI.Modules;
using UI.TurretStats;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class TurretInfo : MonoBehaviour
    {
        public static TurretInfo instance;
        private Node _target;

        [Tooltip("The Shop component of the scene")]
        [SerializeField]
        private Gameplay.Shop shop;
        [SerializeField]
        [Tooltip("The title for the inventory")]
        private TMP_Text inventoryTitle;
        
        [Header("Turret Inventory")]
        [SerializeField]
        [Tooltip("The inventory show/hide for the turrets")]
        private GameObject turretInventoryPage;
        [SerializeField]
        [Tooltip("The text to display for the title")]
        private LocalizedString turretInventoryTitle;
        
        [Header("Module Inventory")]
        [SerializeField]
        [Tooltip("The inventory to place turret buttons")]
        private GameObject applyModuleButton;
        [SerializeField]
        [Tooltip("The inventory to show/hide for the modules")]
        private GameObject moduleInventoryPage;
        [SerializeField]
        [Tooltip("The text to display for the title")]
        private LocalizedString moduleInventoryTitle;
        
        [Header("Turret Info")]
        [SerializeField]
        [Tooltip("The page show/hide for the turret info")]
        private GameObject turretInfoPage;
        [SerializeField]
        [Tooltip("The text to display for the title")]
        private LocalizedString turretInfoTitle;
        
        [Space(10)]
        [Tooltip("The TurretStat used to display the damage")]
        [SerializeField]
        private TurretStat damage;
        [Tooltip("The TurretStat used to display the fire rate")]
        [SerializeField]
        private TurretStat rate;
        [Tooltip("The TurretStat used to display the range")]
        [SerializeField]
        private TurretStat range;
        
        [Space(10)]
        [Tooltip("The GameObject to spawn the module icons as a child of")]
        [SerializeField]
        private Transform modules;
        [Tooltip("The prefab of a module icon to instantiate to display the turret's modules")]
        [SerializeField]
        private GameObject moduleIconPrefab;
        
        [Header("Buttons")]
        [Tooltip("The button that changes the targeting method of the turret")]
        [SerializeField]
        private GameObject cycleTargetingButton;
        [Tooltip("The text on the button that sells the turret")]
        [SerializeField]
        private TMP_Text sellButtonText;
        
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
            
            UpdateModules();
            
            // Display the radius of the turret
            _target.turret.GetComponent<Turret>().Selected();

            // Enable/Disable Targeting types cycle button if it's (not) a dynamic turret.
            if (_target.turret.GetComponent<Turret>() is DynamicTurret)
            {
                cycleTargetingButton.SetActive(true);
                cycleTargetingButton.transform.GetComponentInChildren<TMP_Text>().text = "<b>Targeting:</b>\n" +
                    _target.turret.GetComponent<DynamicTurret>().targetingMethod;
                cycleTargetingButton.GetComponent<Button>().onClick.RemoveAllListeners();
                cycleTargetingButton.GetComponent<Button>().onClick.AddListener(CycleTargeting);
            }
            else if (_target.turret.GetComponent<Turret>() is Lancer)
            {
                cycleTargetingButton.SetActive(true);
                cycleTargetingButton.transform.GetComponentInChildren<TMP_Text>().text = "<b>Rotate</b>";
                cycleTargetingButton.GetComponent<Button>().onClick.RemoveAllListeners();
                cycleTargetingButton.GetComponent<Button>().onClick.AddListener(RotateLancer);
            }
            else
            {
                cycleTargetingButton.SetActive(false);
            }
            
            sellButtonText.text = "<b>Sell:</b>\n" + shop.GetSellAmount() + " <sprite=\"UI-Gold\" name=\"gold\">";

            // Rebuild the Modules and add the stats
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform) modules);
            UpdateStats();
            OpenTurretInfo();
        }
        
        /// <summary>
        /// Gets the turret the NodeUI is targeting
        /// </summary>
        /// <returns>The turret the NodeUI is targeting</returns>
        public GameObject GetTurret()
        {
            return _target != null ? _target.turret : null;
        }
        
        /// <summary>
        /// Applies a module to the currently selected turret
        /// </summary>
        public void ApplyModule()
        {
            ModuleChainHandler handler = shop.GetModuleChainHandler();

            bool isApplied = _target.ApplyModuleToTurret(handler);
            if (!isApplied) return;
            
            shop.RemoveModule();
        }
        
        /// <summary>
        /// Turret's targeting method increments once through the cycle of targeting methods
        /// </summary>
        public void CycleTargeting()
        {
            Array types = Enum.GetValues(typeof(TargetingMethod));
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
            // Stats
            damage.SetData(turret.damage);
            rate.SetData(turret.fireRate);
            range.SetData(turret.range);
            // Display the radius of the turret
            turret.Selected();
            Debug.Log(turret);
            Debug.Log(turret.rangeDisplay);
            Color color = turret.rangeDisplay.GetComponent<SpriteRenderer>().color;
            damage.SetColor(color);
            rate.SetColor(color);
            range.SetColor(color);
        }

        public void UpdateSelection()
        {
            UpdateStats();
            UpdateModules();
        }
        
        /// <summary>
        /// Sells the turret
        /// </summary>
        public void SellTurret()
        {
            _target.SellTurret(shop.GetSellAmount());
        }
        
        /// <summary>
        /// Rotates Lancer Turret
        /// </summary>
        private void RotateLancer()
        {
            _target.turret.GetComponent<Lancer>().partToRotate.Rotate(0, 0, -60);
        }
        
        /// <summary>
        /// Updates the render of the modules for a turret
        /// </summary>
        private void UpdateModules()
        {
            // Removes module icons created from the previously selected turret
            for (var i = 0; i < modules.childCount; i++)
                Destroy(modules.GetChild(i).gameObject);
            
            // Add each Module as an icon
            foreach (ModuleChainHandler handle in _target.turret.GetComponent<Turret>().moduleHandlers)
            {
                GameObject moduleIcon = Instantiate(moduleIconPrefab, modules);
                moduleIcon.name = "_" + moduleIcon.name;
                moduleIcon.GetComponent<ModuleIcon>().SetData(handle);
                foreach (Image image in moduleIcon.GetComponentsInChildren<Image>())
                {
                    image.raycastTarget = false;
                }

                moduleIcon.GetComponentsInChildren<Image>();
            }
            
            // Display the radius of the turret
            _target.turret.GetComponent<Turret>().UpdateRange();
        }

        public void OpenTurretInventory()
        {
            if (turretInventoryPage.activeSelf)
            {
                BuildManager.instance.Deselect();
                return;
            }

            if (turretInfoPage.activeSelf)
            {
                OpenTurretInfo();
                return;
            }
            
            Show();
            turretInventoryPage.SetActive(true);
            moduleInventoryPage.SetActive(false);
            turretInfoPage.SetActive(false);
            
            // TODO - Don't always do this
            //BuildManager.instance.Deselect();
        }

        public void OpenModuleInventory()
        {
            Show();
            // TODO: If turret is selected, work the button
            moduleInventoryPage.SetActive(true);
            turretInventoryPage.SetActive(false);
            turretInfoPage.SetActive(false);

            applyModuleButton.SetActive(_target != null);
        }

        private void OpenTurretInfo()
        {
            if (turretInfoPage.activeSelf)
            {
                BuildManager.instance.Deselect();
                return;
            }
            
            Show();
            turretInfoPage.SetActive(true);
            turretInventoryPage.SetActive(false);
            moduleInventoryPage.SetActive(false);
            
            UpdateStats();
        }

        public void Close()
        {
            turretInfoPage.SetActive(false);
            turretInventoryPage.SetActive(false);
            moduleInventoryPage.SetActive(false);
            
            _target = null;
            
            var rt = (RectTransform)transform;
            rt.anchorMin = new Vector2(-0.25f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(0f, rt.anchorMax.y);
        }

        private void Show()
        {
            var rt = (RectTransform)transform;
            rt.anchorMin = new Vector2(0f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(0.25f, rt.anchorMax.y);
        }
    }
}
