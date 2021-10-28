using Turrets;
using Turrets.Blueprints;
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
        _buildManager.BuiltTurret();
    }
    
    // Called when we're building a turret
    private void BuildTurret(TurretBlueprint blueprint)
    {
        // Spawn the turret and set the turret and blueprint
        var nodePosition = transform.position;
        var newTurret = Instantiate(blueprint.prefab, nodePosition, Quaternion.identity);
        turret = newTurret;
        var turretClass = turret.GetComponent<Turret>();
        turretBlueprint = blueprint;
        
        foreach (var turretUpgrade in blueprint.upgrades)
        {
            turretClass.AddUpgrade(turretUpgrade);
        }
        
        // Spawn the build effect and destroy after
        var effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
    }
    
    // Called when we upgrade the turret
    public void UpgradeTurret(Upgrade upgrade)
    {
        // Apply the upgrade
        var appliedUpgrade = turret.GetComponent<Turret>().AddUpgrade(upgrade);
        if (appliedUpgrade)
            return;

            // Spawn the build effect
        var effect = Instantiate(_buildManager.buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        BuildManager.instance.DeselectNode();
    }
    
    // Called when we sell turrets
    public void SellTurret()
    {
        // // Grant the money
        // GameStats.money += turretBlueprint.GetSellAmount();
        
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
        _rend.material.color = hoverColour;
    }
    
    // Reset colour when we no longer hover.
    private void OnMouseExit()
    {
        _rend.material.color = _defaultColour;
    }
}
