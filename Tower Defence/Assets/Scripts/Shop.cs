using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;

    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;

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
}
