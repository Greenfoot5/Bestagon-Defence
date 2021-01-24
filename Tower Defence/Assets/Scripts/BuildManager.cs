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
    
    public GameObject buildEffect;

    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;

    public NodeUI nodeUI;

    public bool CanBuild => _turretToBuild != null;
    public bool HasMoney => GameStats.money >= _turretToBuild.cost;
    
    // Called when we build
    public void BuildTurretOn(Node node)
    {
        if (GameStats.money < _turretToBuild.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        GameStats.money -= _turretToBuild.cost;

        var nodePosition = node.transform.position;
        var turret = Instantiate(_turretToBuild.prefab, nodePosition, Quaternion.identity);
        node.turret = turret;

        GameObject effect = Instantiate(buildEffect, nodePosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        Debug.Log("Turret build. Gold left: " + GameStats.money);
    }
    
    // Used to set the turret we want to build.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        _turretToBuild = turret;
        _selectedNode = null;
        
        nodeUI.Hide();
    }

    public void SelectNode(Node node)
    {
        _selectedNode = node;
        _turretToBuild = null;

        nodeUI.SetTarget(node);
    }
}
