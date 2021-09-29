using Turrets;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;
    
    [Tooltip("Bullet Turret")]
    public TurretBlueprint standardTurret;
    [Tooltip("AoE Bullet Tower")]
    public TurretBlueprint missileLauncher;
    [Tooltip("Laser beam turret")]
    public TurretBlueprint laserBeamer;

    public GameObject selectionUI;
    public GameObject defaultButton;

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

    public void PurchaseEnhancement()
    {
        selectionUI.SetActive(true);
    }

    public void SelectUpgrade(Upgrade upgrade)
    {
        _buildManager.SelectTurretToBuild(null);
    }

    public void SelectTurret(GameObject turret)
    {
        _buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SpawnNewItem(Upgrade upgrade)
    {
        Instantiate(upgrade.GenerateButton(defaultButton, this), transform);
    }
}
