using Turrets;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;

    public GameObject turretInventory;
    public GameObject upgradeInventory;
    public GameObject defaultTurretButton;
    public GameObject defaultUpgradeButton;

    public GameObject SelectionUI;

    void Start()
    {
        _buildManager = BuildManager.instance;
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
        turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(turret, turretButton); });
    }
    
    public void SpawnNewUpgrade(Upgrade upgrade)
    {
        Instantiate(upgrade.GenerateButton(defaultUpgradeButton, this), upgradeInventory.transform);
    }

    public void OpenSelectionUI()
    {
        SelectionUI.SetActive(true);
    }
}
