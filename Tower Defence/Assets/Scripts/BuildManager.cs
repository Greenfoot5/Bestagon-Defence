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
    public GameObject sellEffect;

    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;

    public NodeUI nodeUI;

    public bool CanBuild => _turretToBuild != null;
    public bool HasMoney => GameStats.money >= _turretToBuild.cost;

    // Used to set the turret we want to build.
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        _turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return _turretToBuild;
    }

    public void SelectNode(Node node)
    {
        if (_selectedNode == node)
        {
            DeselectNode();
            return;
        }
        _selectedNode = node;
        _turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        _selectedNode = null;
        nodeUI.Hide();
    }
}
