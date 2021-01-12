using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
            return;
        }
        instance = this;
    }

    private TurretBlueprint _turretToBuild;

    public bool CanBuild => _turretToBuild != null;
    
    // Called when we build
    public void BuildTurretOn(Node node)
    {
        if (GameStats.gold < _turretToBuild.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        GameStats.gold -= _turretToBuild.cost;
        
        var turret = Instantiate(_turretToBuild.prefab, node.transform.position, Quaternion.identity);
        node.turret = turret;
        
        Debug.Log("Turret build. Gold left: " + GameStats.gold);
    }
    
    // Used to set the turret we want to build.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        _turretToBuild = turret;
    }
    
}
