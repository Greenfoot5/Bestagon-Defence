using System;
using Abstract.Data;
using Levels.Maps;
using TMPro;
using Turrets;
using UI.Modules;
using UI.Shop;
using UnityEngine;
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
        [Range(0, Mathf.Infinity)]
        public int selectionCost;
        private bool _hasPlayerMadePurchase;
        
        [SerializeField]
        private TextMeshProUGUI turretInventoryButton;
        [SerializeField]
        private TextMeshProUGUI moduleInventoryButton;

        [Tooltip("The percentage of the selection cost to sell turrets for")]
        [SerializeField]
        private double sellPercentage = 0.85;
        
        /// <summary>
        /// Initialises values and set's starting prices
        /// </summary>
        private void Start()
        {
            _buildManager = BuildManager.instance;
            _levelData = _buildManager.GetComponent<GameManager>().levelData;
            // It should only be greater than 0 if we've loaded a save
            if (selectionCost == 0)
            {
                selectionCost = _levelData.initialSelectionCost;
            }
            else
            {
                _hasPlayerMadePurchase = true;
            }

            // Update button text
            turretInventoryButton.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + selectionCost;
            moduleInventoryButton.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + selectionCost;
        }
        
        /// <summary>
        /// Selects a turret from the inventory
        /// </summary>
        /// <param name="turret">The TurretBlueprint of the selected turret</param>
        /// <param name="button">The button that selected the turret</param>
        private void SelectTurret(TurretBlueprint turret, GameObject button)
        {
            _buildManager.SelectTurretToBuild(turret, button);
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
            turretButton.GetComponent<Image>().sprite = turret.shopIcon;
            turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(turret, turretButton); });
            
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
            moduleButton.GetComponentInChildren<ModuleIcon>().SetData(module);
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
        /// Increases the shop cost by the amount in the level data
        /// </summary>
        public void IncrementSelectionCost()
        {
            GameStats.money -= selectionCost;
            selectionCost += _levelData.selectionCostIncrement;
            UpdateCostText();
        }

        /// <summary>
        /// Updates the text displaying the cost of the next shop opening
        /// </summary>
        public void UpdateCostText()
        {
            turretInventoryButton.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + selectionCost;
            moduleInventoryButton.text = "<sprite=\"UI-Powercell\" name=\"full\"> " + selectionCost;
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

            if (moduleTransform.childCount != 2 ||
                !moduleTransform.GetChild(1).GetComponentInChildren<Button>().interactable) return;
            
            var button = moduleTransform.GetChild(1).GetComponentInChildren<Button>();
            button.onClick.Invoke();
            button.Select();
        }
        
        /// <summary>
        /// Displays the turret inventory and hides the module inventory
        /// </summary>
        public void EnableTurretInventory()
        {
            turretInventory.SetActive(true);
            moduleInventory.SetActive(false);

            if (turretInventory.transform.childCount != 2) return;

            var button = turretInventory.transform.GetChild(1).GetComponent<Button>();
            button.onClick.Invoke();
            button.Select();
        }

        public int GetSellAmount()
        {
            return (int) (sellPercentage * selectionCost);
        }
    }
}
