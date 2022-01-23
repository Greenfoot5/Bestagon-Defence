using Turrets;
using Turrets.Blueprints;
using Turrets.Upgrades;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    // The colour to set out node to
    public Color hoverColour;
    public Color cantAffordColour;
    private Color _defaultColour;

    //[HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded;
    
    private Renderer _rend;
    private BuildManager _buildManager;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _defaultColour = _rend.material.color;
        _buildManager = BuildManager.instance;
    }

    /// <summary>
    /// Places the turret on the node
    /// </summary>
    /// <param name="blueprint">The blueprint of the turret to build</param>
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
    
    /// <summary>
    /// Called when upgrading a turret
    /// </summary>
    /// <param name="upgrade">The upgrade to add to the turret</param>
    /// <returns>If the upgrade was applied</returns>
    public bool UpgradeTurret(Upgrade upgrade)
    {
        // Apply the upgrade
        var appliedUpgrade = turret.GetComponent<Turret>().AddUpgrade(upgrade);
        if (!appliedUpgrade) return false;

        // Spawn the build effect
        var effect = Instantiate(_buildManager.buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        // Deselect and reselect to avoid issues from upgrading
        BuildManager.instance.DeselectNode();
        BuildManager.instance.SelectNode(this);
        return true;
    }
    
    /// <summary>
    /// Called when the turret is sold
    /// </summary>
    public void SellTurret()
    {
        // // Grant the money
        // GameStats.money += turretBlueprint.GetSellAmount();
        
        // Spawn the sell effect
        var effect = Instantiate(_buildManager.sellEffect, transform.position, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        
        // Destroy the turret and reset any of the node's selection variables
        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;

        BuildManager.instance.DeselectNode();
    }
    
    /// <summary>
    /// Called when the mouse is down.
    /// Either Selects the turret or builds
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Select the node/turret
        if (turret != null)
        {
            _buildManager.SelectNode(this);
            return;
        }
        
        // Check we are trying to build
        if (!_buildManager.CanBuild)
        {
            _buildManager.Deselect();
            return;
        }
        
        // Construct a turret
        BuildTurret(_buildManager.GetTurretToBuild());
        _buildManager.BuiltTurret();
    }
    
    // Called when the mouse hovers over the node
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Make sure we're trying to build
        if (!_buildManager.CanBuild)
        {
            return;
        }
        // Check if we can afford the select turret
        _rend.material.color = hoverColour;
    }
    
    /// <summary>
    /// Called when the mouse is no longer over the node
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _rend.material.color = _defaultColour;
    }
}
