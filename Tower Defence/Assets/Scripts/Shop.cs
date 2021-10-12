using TMPro;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;
    private LevelData.LevelData _levelData;

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
    
    /*
     * Turret Selection Functions
     */
    private void SelectTurret(TurretBlueprint turret, GameObject button)
    {
        _buildManager.SelectTurretToBuild(turret, button);
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        // Select Upgrade
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
        Instantiate(upgrade.GenerateButton(defaultUpgradeButton, this), upgradeInventory.transform);
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
}
