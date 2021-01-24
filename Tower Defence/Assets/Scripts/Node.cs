using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    // The colour to set out node to
    public Color hoverColour;
    public Color cantAffordColour;
    private Color _defaultColour;
    
    [Header("Optional")]
    public GameObject turret; 
    
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

        // TODO - Enable turret upgrades
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

        BuildTurret(_buildManager.GetTurretToBuild());
    }
    
    void BuildTurret(TurretBlueprint blueprint)
    {
        if (GameStats.money < blueprint.cost)
        {
            Debug.Log("Not enough gold!");
            return;
        }

        GameStats.money -= blueprint.cost;

        var nodePosition = transform.position;
        var newTurret = Instantiate(blueprint.prefab, nodePosition, Quaternion.identity);
        turret = newTurret;

        GameObject effect = Instantiate(_buildManager.buildEffect, nodePosition, Quaternion.identity);
        Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
    }
    
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
