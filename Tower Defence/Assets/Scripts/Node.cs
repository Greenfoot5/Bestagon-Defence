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
        
        // Check we are trying to build
        if (!_buildManager.CanBuild)
        {
            return;
        }
        
        // TODO - Enable turret upgrades
        if (turret != null)
        {
            Debug.Log("Turret already built!");
            return;
        }

        _buildManager.BuildTurretOn(this);
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
