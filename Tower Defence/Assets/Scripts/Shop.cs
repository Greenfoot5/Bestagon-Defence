using TMPro;
using Turrets.Blueprints;
using Turrets.Upgrades.TurretUpgrades;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;
    private LevelData.LevelData _levelData;
    private GameObject _selectedUpgrade;

    public GameObject turretInventory;
    public GameObject upgradeInventory;
    public GameObject defaultTurretButton;
    public GameObject defaultUpgradeButton;

    public GameObject selectionUI;
    private int _selectionCost;

    private void Start()
    {
        _buildManager = BuildManager.instance;
        _levelData = _buildManager.GetComponent<GameManager>().levelData;
        _selectionCost = _levelData.initialSelectionCost;
    }
    
    private void SelectTurret(TurretBlueprint turret, GameObject button)
    {
        _buildManager.SelectTurretToBuild(turret, button);
    }

    private void SelectUpgrade(Upgrade upgrade, GameObject button)
    {
        _selectedUpgrade.transform.GetChild(0).gameObject.SetActive(false);
        _selectedUpgrade = button;
        button.transform.GetChild(0).gameObject.SetActive(false);
    }

    public Upgrade UseUpgrade()
    {
        var upgrade = _selectedUpgrade.GetComponent<EvolutionSelectionUI>().upgrade;
        Destroy(_selectedUpgrade);
        return upgrade;
    }
    
    public void SpawnNewTurret(TurretBlueprint turret)
    {
        // Add and display the new item
        var turretButton = Instantiate(defaultTurretButton, turretInventory.transform);
        turretButton.GetComponent<Image>().sprite = turret.shopIcon;
        turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(turret, turretButton); });
    }
    
    public void SpawnNewUpgrade(Upgrade upgrade)
    {
        var upgradeButton = Instantiate(defaultUpgradeButton, upgradeInventory.transform);
        upgradeButton.GetComponent<Image>().sprite = upgrade.icon;
        upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { SelectUpgrade(upgrade, upgradeButton); });
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
        turretInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Icons\" name=\"Coin\"> " + _selectionCost;
        upgradeInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "<sprite=\"UI-Icons\" name=\"Coin\"> " + _selectionCost;
    }

    public void EnableUpgradeInventory()
    {
        // TODO - Only show those that can be used
        turretInventory.SetActive(false);
        upgradeInventory.SetActive(true);
    }

    public void EnableTurretInventory()
    {
        turretInventory.SetActive(true);
        upgradeInventory.SetActive(false);
    }
}
