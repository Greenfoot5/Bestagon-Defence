using UnityEngine;

public class Shop : MonoBehaviour
{
    private BuildManager _buildManager;

    void Start()
    {
        _buildManager = BuildManager.instance;
    }
    
    public void PurchaseStandardTurret()
    {
        Debug.Log("Standard Turret Selected.");
        _buildManager.SetTurretToBuild(_buildManager.standardTurretPrefab);
    }

    public void PurchaseMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected.");
        _buildManager.SetTurretToBuild(_buildManager.missileLauncherPrefab);
    }
}
