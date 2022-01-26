using Abstract;
using Abstract.Managers;
using TMPro;
using Turrets.Blueprints;
using Turrets.Modules;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Levels
{
    /// <summary>
    /// Handles the shop and inventory of the player
    /// </summary>
    public class Shop : MonoBehaviour
    {
        private BuildManager _buildManager;
        private LevelData.LevelData _levelData;
        private Module _selectedModule;
        private GameObject _selectedModuleButton;

        public GameObject turretInventory;
        [FormerlySerializedAs("ModuleInventory")] public GameObject moduleInventory;
        public GameObject defaultTurretButton;
        public GameObject defaultModuleButton;

        public GameObject selectionUI;
        private int _selectionCost;

        private TextMeshProUGUI _turretInventoryButton;
        private TextMeshProUGUI _moduleInventoryButton;
        
        /// <summary>
        /// Initialises values and set's starting prices
        /// </summary>
        private void Start()
        {
            _buildManager = BuildManager.instance;
            _levelData = _buildManager.GetComponent<GameManager>().levelData;
            _selectionCost = _levelData.initialSelectionCost;
            // Update button text
            _turretInventoryButton = turretInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            _moduleInventoryButton = moduleInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            _turretInventoryButton.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
            _moduleInventoryButton.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
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
        private void SelectModule(Module module, GameObject button)
        {
            if (_selectedModuleButton != null) _selectedModuleButton.transform.GetChild(0).gameObject.SetActive(false);
            _selectedModuleButton = button;
            button.transform.GetChild(0).gameObject.SetActive(true);
            _selectedModule = module;
        }
        
        /// <summary>
        /// Returns the currently selected module
        /// </summary>
        /// <returns>The currently selected Module</returns>
        public Module GetModule()
        {
            return _selectedModule;
        }
        
        /// <summary>
        /// Removes a module from the inventory
        /// </summary>
        public void RemoveModule()
        {
            Destroy(_selectedModuleButton);
            _selectedModule = null;
        }
        
        /// <summary>
        /// Adds a new turret button to the turret inventory
        /// </summary>
        /// <param name="turret">The blueprint of the turret to add</param>
        public void SpawnNewTurret(TurretBlueprint turret)
        {
            // Add and display the new item
            var turretButton = Instantiate(defaultTurretButton, turretInventory.transform);
            turretButton.GetComponent<Image>().sprite = turret.shopIcon;
            turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(turret, turretButton); });
        }
        
        /// <summary>
        /// Adds a new module button to the module inventory
        /// </summary>
        /// <param name="module">The module to add</param>
        public void SpawnNewModule(Module module)
        {
            var moduleButton = Instantiate(defaultModuleButton, moduleInventory.transform);
            moduleButton.GetComponentInChildren<ModuleIcon>().SetData(module);
            moduleButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectModule(module, moduleButton); });
        }
        
        /// <summary>
        /// Opens the shop and displays the selection
        /// </summary>
        public void OpenSelectionUI()
        {
            selectionUI.SetActive(true);
        }
        
        /// <summary>
        /// Gets the cost of opening the shop and displaying the selection
        /// </summary>
        /// <returns>The cost to open the shop</returns>
        public int GetSelectionCost()
        {
            return _selectionCost;
        }
        
        /// <summary>
        /// Increases the shop cost by the amount in the level data
        /// </summary>
        public void IncrementSelectionCost()
        {
            GameStats.money -= _selectionCost;
            _selectionCost += _levelData.selectionCostIncrement;
            // Update button text
            _turretInventoryButton.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
            _moduleInventoryButton.text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
        }
        
        /// <summary>
        /// Displays the module inventory and hides the turret inventory
        /// </summary>
        public void EnableModuleInventory()
        {
            turretInventory.SetActive(false);
            moduleInventory.SetActive(true);
        }
        
        /// <summary>
        /// Displays the turret inventory and hides the module inventory
        /// </summary>
        public void EnableTurretInventory()
        {
            turretInventory.SetActive(true);
            moduleInventory.SetActive(false);
        }
    }
}
