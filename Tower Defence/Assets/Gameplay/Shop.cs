using System;
using Abstract.Data;
using Levels.Maps;
using MaterialLibrary.Trapezium;
using TMPro;
using Turrets;
using UI.Inventory;
using UI.Modules;
using UI.Shop;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay
{
    /// <summary>
    /// Handles the shop and inventory of the player
    /// </summary>
    public class Shop : MonoBehaviour
    {
        private BuildManager _buildManager;
        private LevelData _levelData;
        private ModuleChainHandler _selectedHandler;
        private GameObject _selectedHandlerButton;

        
        [SerializeField]
        [Tooltip("The inventory to place turret buttons")]
        private GameObject turretInventory;
        [SerializeField]
        [Tooltip("The inventory to place module buttons under")]
        private GameObject moduleInventory;
        [SerializeField]
        [Tooltip("The generic turret button prefab")]
        private GameObject defaultTurretButton;
        [Tooltip("The generic module button prefab")]
        [SerializeField]
        private GameObject defaultModuleButton;
        
        [Tooltip("The UI to display when the player opens the shop")]
        [SerializeField]
        private GameObject selectionUI;
        [FormerlySerializedAs("selectionCost")] [Range(0, Mathf.Infinity)]
        [HideInInspector]
        public int nextCost;
        private bool _hasPlayerMadePurchase;
        
        [FormerlySerializedAs("shopButton")] [FormerlySerializedAs("turretInventoryButton")] [SerializeField]
        private TextMeshProUGUI powercellsText;
        [SerializeField]
        private Progress energyProgress;
        
        [Header("Shop Button Colours")]
        [Tooltip("Shop Button Colours Top when can afford")]
        [SerializeField]
        private Color defaultButtonTop;
        [Tooltip("Shop Button Colours Bottom when can afford")]
        [SerializeField]
        private Color defaultButtonBottom;
        [Tooltip("Shop Button Colours Top when can't afford")]
        [SerializeField]
        private Color expensiveButtonTop;
        [Tooltip("Shop Button Colours Bottom when can afford")]
        [SerializeField]
        private Color expensiveButtonBottom;

        /// <summary>
        /// Initialises values and set's starting prices
        /// </summary>
        private void Start()
        {
            _buildManager = BuildManager.instance;
            _levelData = _buildManager.GetComponent<GameManager>().levelData;
            // It should only be greater than 0 if we've loaded a save
            if (nextCost == 0)
            {
                nextCost = _levelData.initialSelectionCost;
            }
            else
            {
                _hasPlayerMadePurchase = true;
            }

            // Update button text
            powercellsText.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + nextCost;
            GameStats.OnGainMoney += CalculateEnergy;
            GameStats.OnGainPowercell += UpdateEnergyButton;
            CalculateEnergy();
        }
        
        /// <summary>
        /// Selects a module from the inventory
        /// </summary>
        /// <param name="module">The selected module</param>
        /// <param name="button">The button that selected the module</param>
        private void SelectModule(ModuleChainHandler module, GameObject button)
        {
            if (_selectedHandlerButton != null) _selectedHandlerButton.transform.GetChild(0).gameObject.SetActive(false);
            _selectedHandlerButton = button;
            button.transform.GetChild(0).gameObject.SetActive(true);
            _selectedHandler = module;
        }
        
        /// <summary>
        /// Returns the currently selected module
        /// </summary>
        /// <returns>The currently selected ModuleChainHandler</returns>
        public ModuleChainHandler GetModuleChainHandler()
        {
            return _selectedHandler;
        }
        
        /// <summary>
        /// Removes a module from the inventory
        /// </summary>
        public void RemoveModule()
        {
            Destroy(_selectedHandlerButton);
            GameManager.ModuleInventory.Remove(_selectedHandler);
            _selectedHandler = new ModuleChainHandler();
        }

        /// <summary>
        /// Adds a new turret button to the turret inventory
        /// </summary>
        /// <param name="turret">The blueprint of the turret to add</param>
        public void SpawnNewTurret(TurretBlueprint turret)
        {
            _hasPlayerMadePurchase = true;
            // Add and display the new item
            GameObject turretButton = Instantiate(defaultTurretButton, turretInventory.transform);
            turretButton.name = "_" + turretButton.name;
            turretButton.GetComponent<TurretInventoryItem>().Init(turret);
            
            selectionUI.GetComponentInChildren<AddSelection>().AddTurretType(turret.prefab.GetComponent<Turret>().GetType());
            GameManager.TurretInventory.Add(turret);
        }
        
        /// <summary>
        /// Adds a new module button to the module inventory
        /// </summary>
        /// <param name="module">The module to add</param>
        public void SpawnNewModule(ModuleChainHandler module)
        {
            GameObject moduleButton = Instantiate(defaultModuleButton, moduleInventory.transform);
            moduleButton.name = "_" + moduleButton.name;
            moduleButton.GetComponentInChildren<ModuleInventoryItem>().Init(module);
            moduleButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectModule(module, moduleButton); });
            
            GameManager.ModuleInventory.Add(module);
        }
        
        /// <summary>
        /// Opens the shop and displays the selection
        /// </summary>
        public void OpenSelectionUI()
        {
            selectionUI.SetActive(true);
        }
        
        /// <summary>
        /// Gets if the player has made a purchase yet
        /// </summary>
        /// <returns>If the player has made a purchase</returns>
        public bool HasPlayerMadePurchase()
        {
            return _hasPlayerMadePurchase || (!_levelData.hasInitialSelection);
        }
        
        /// <summary>
        /// Displays the turret inventory and hides the module inventory
        /// </summary>
        public void EnableTurretInventory()
        {
            turretInventory.SetActive(true);
            moduleInventory.SetActive(false);

            if (turretInventory.transform.childCount == 0) return;

            var button = turretInventory.transform.GetChild(0).GetComponent<Button>();
            button.onClick.Invoke();
            button.Select();
        }

        /// <summary>
        /// Displays the module inventory and hides the turret inventory
        /// </summary>
        /// <param name="turret">The selected turret</param>
        public void EnableModuleInventory(Turret turret)
        {
            turretInventory.SetActive(false);
            moduleInventory.SetActive(true);

            Transform moduleTransform = moduleInventory.transform;
            
            // Loop through all modules and check if they are applicable
            for(var i = 0; i < moduleTransform.childCount; i++)
            {
                Transform child = moduleTransform.GetChild(i);
                try
                {
                    child.GetComponentInChildren<Button>().interactable =
                        child.GetComponentInChildren<ModuleIcon>().GetModule().ValidModule(turret);
                }
                // One will be the shop button
                catch (NullReferenceException)
                { }
            }

            if (moduleTransform.childCount == 0 ||
                !moduleTransform.GetChild(0).GetComponentInChildren<Button>().interactable) return;
            
            var button = moduleTransform.GetChild(0).GetComponentInChildren<Button>();
            button.onClick.Invoke();
            button.Select();
        }

        public int GetSellAmount()
        {
            return (int) (_levelData.sellPercentage * nextCost);
        }

        private void CalculateEnergy()
        {
            while (GameStats.Money > nextCost)
            {
                GameStats.Money -= nextCost;
                nextCost += _levelData.selectionCostIncrement;
                GameStats.Powercells++;
            }
            UpdateEnergyButton();
        }

        private void UpdateEnergyButton()
        {
            energyProgress.outColorA = expensiveButtonTop;
            energyProgress.outColorB = expensiveButtonBottom;
            powercellsText.color = expensiveButtonTop;
            powercellsText.text = "<sprite=\"UI-Powercell\" name=\"empty\"> " + 0;
            if (GameStats.Powercells > 0)
            {
                energyProgress.outColorA = defaultButtonTop;
                energyProgress.outColorB = defaultButtonBottom;
                powercellsText.color = new Color(1,1,1);
                powercellsText.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + GameStats.Powercells;
            }

            energyProgress.percentage = GameStats.Money / (float)nextCost;
        }
    }
}
