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
}
