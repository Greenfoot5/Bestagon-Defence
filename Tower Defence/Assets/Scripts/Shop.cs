using Turrets;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;
    
    [Tooltip("Bullet Turret")]
    public TurretBlueprint standardTurret;
    [Tooltip("AoE Bullet Tower")]
    public TurretBlueprint missileLauncher;
    [Tooltip("Laser beam turret")]
    public TurretBlueprint laserBeamer;

    public GameObject turretInventory;
    public GameObject upgradeInventory;
    public GameObject defaultTurretButton;
    public GameObject defaultUpgradeButton;

    void Start()
    {
        _buildManager = BuildManager.instance;
    }
    
    /*
     * Turret Selection Functions
     */
    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected.");
        _buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected.");
        _buildManager.SelectTurretToBuild(missileLauncher);
    }
    
    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected.");
        _buildManager.SelectTurretToBuild(laserBeamer);
    }

    // public void PurchaseEnhancement()
    // {
    //     selectionUI.SetActive(true);
    // }
    
    public void SelectTurret(GameObject turret)
    {
        _buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        // Select Upgrade
    }
    
    public void SpawnNewTurret(GameObject turret)
    {
        // Add and display the new item
        var turretButton = Instantiate(defaultTurretButton, turretInventory.transform);
        turretButton.GetComponent<Button>().onClick.AddListener(delegate { SelectTurret(null); });
    }
    
    public void SpawnNewUpgrade(Upgrade upgrade)
    {
        Instantiate(upgrade.GenerateButton(defaultUpgradeButton, this), upgradeInventory.transform);
    }

    
}
