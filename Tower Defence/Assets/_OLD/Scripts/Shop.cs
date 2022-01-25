using TMPro;
using Turrets.Blueprints;
using Turrets.Modules;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;
    private LevelData.LevelData _levelData;
    private Module _selectedModule;
    private GameObject _selectedModuleButton;

    public GameObject turretInventory;
    public GameObject ModuleInventory;
    public GameObject defaultTurretButton;
    public GameObject defaultModuleButton;

    public GameObject selectionUI;
    private int _selectionCost;

    private void Start()
    {
        _buildManager = BuildManager.instance;
        _levelData = _buildManager.GetComponent<GameManager>().levelData;
        _selectionCost = _levelData.initialSelectionCost;
        // Update button text
        turretInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
        ModuleInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
    }
    
    private void SelectTurret(TurretBlueprint turret, GameObject button)
    {
        _buildManager.SelectTurretToBuild(turret, button);
    }

    private void SelectModule(Module Module, GameObject button)
    {
        if (_selectedModuleButton != null) _selectedModuleButton.transform.GetChild(0).gameObject.SetActive(false);
        _selectedModuleButton = button;
        button.transform.GetChild(0).gameObject.SetActive(true);
        _selectedModule = Module;
    }

    public Module GetModule()
    {
        return _selectedModule;
    }

    public void RemoveModule()
    {
        Destroy(_selectedModuleButton);
        _selectedModule = null;
    }
    
    public void SpawnNewTurret(TurretBlueprint turret)
    {
        // Add and display the new item
        var turretButton = Instantiate(defaultTurretButton, turretInventory.transform);
        turretButton.GetComponent<Image>().sprite = turret.shopIcon;
        turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(turret, turretButton); });
    }
    
    public void SpawnNewModule(Module Module)
    {
        var ModuleButton = Instantiate(defaultModuleButton, ModuleInventory.transform);
        ModuleButton.GetComponentInChildren<ModuleIcon>().SetData(Module);
        ModuleButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectModule(Module, ModuleButton); });
    }

    public void OpenSelectionUI()
    {
        selectionUI.SetActive(true);
    }

    public int GetSelectionCost()
    {
        return _selectionCost;
    }

    public void IncrementSelectionCost()
    {
        GameStats.money -= _selectionCost;
        _selectionCost += _levelData.selectionCostIncrement;
        // Update button text
        turretInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
        ModuleInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Gold\" name=\"gold\"> " + _selectionCost;
    }

    public void EnableModuleInventory()
    {
        // TODO - Only show those that can be used
        turretInventory.SetActive(false);
        ModuleInventory.SetActive(true);
    }

    public void EnableTurretInventory()
    {
        turretInventory.SetActive(true);
        ModuleInventory.SetActive(false);
    }
}
