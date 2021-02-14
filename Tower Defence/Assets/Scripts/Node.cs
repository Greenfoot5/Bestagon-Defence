using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    // The colour to set out node to
    public Color hoverColour;
    public Color cantAffordColour;
    private Color _defaultColour;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded;
    
    private Renderer _rend;
    private BuildManager _buildManager;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _defaultColour = _rend.material.color;
        _buildManager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        // Make sure we're hovering over the node and nothing else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    
        // Select the node/turret
        if (turret != null)
        {
            _buildManager.SelectNode(this);
            return;
        }
        
        // Check we are trying to build
        if (!_buildManager.CanBuild)
        {
            return;
        }
        
        // Construct a turret
        BuildTurret(_buildManager.GetTurretToBuild());
    }
    
    // Called when we're building a turret
    private void BuildTurret(TurretBlueprint blueprint)
    {
        // Check we can afford it
        if (GameStats.money < blueprint.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }
        
        // Subtract the cost
        GameStats.money -= blueprint.cost;
        
        // Spawn the turret and set the turret and blueprint
        var nodePosition = transform.position;
        var newTurret = Instantiate(blueprint.prefab, nodePosition, Quaternion.identity);
        turret = newTurret;
        turretBlueprint = blueprint;
        
        // Spawn the build effect and destroy after
        GameObject effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
    }
    
    // Called when we upgrade the turret
    public void UpgradeTurret()
    {
        // Check we can afford it
        if (GameStats.money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough gold!");
            return;
        }
        
        // Subtract the cost
        GameStats.money -= turretBlueprint.upgradeCost;
        
        // Remove old turret, and spawn new one
        Destroy(turret);
        var nodePosition = transform.position;
        var newTurret = Instantiate(turretBlueprint.upgradedPrefab, nodePosition, Quaternion.identity);
        turret = newTurret;
        
        // Spawn the build effect
        GameObject effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);

        isUpgraded = true;
        
        BuildManager.instance.DeselectNode();
    }
    
    // Called when we sell turrets
    public void SellTurret()
    {
        // Grant the money
        GameStats.money += turretBlueprint.GetSellAmount();
        
        // Spawn the sell effect
        GameObject effect = Instantiate(_buildManager.sellEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        // Destroy the turret and reset any of the node's selection variables
        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;

        BuildManager.instance.DeselectNode();
    }
    
    // Called when the mouse hovers over the object
    private void OnMouseEnter()
    {
        // Make sure we're hovering over the node and nothing else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        // Make sure we're trying to build
        if (!_buildManager.CanBuild)
        {
            return;
        }
        
        // Check if we can afford the select turret
        if (_buildManager.HasMoney)
        {
            _rend.material.color = hoverColour;
        }
        else
        {
            _rend.material.color = cantAffordColour;
        }
    }
    
    // Reset colour when we no longer hover.
    private void OnMouseExit()
    {
        _rend.material.color = _defaultColour;
    }
}
