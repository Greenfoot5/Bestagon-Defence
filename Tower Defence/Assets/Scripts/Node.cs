using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    // The colour to set out node to
    public Color hoverColour;
    private Color _defaultColour;

    private GameObject _turret;
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
        if (_buildManager.GetTurretToBuild() == null)
        {
            return;
        }
        
        // TODO - Enable turret upgrades
        if (_turret != null)
        {
            Debug.Log("Turret already built!");
        }

        GameObject turretToBuild = _buildManager.GetTurretToBuild();
        _turret = Instantiate(turretToBuild, transform.position, transform.rotation);
    }
    
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (_buildManager.GetTurretToBuild() == null)
        {
            return;
        }
        
        _rend.material.color = hoverColour;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _defaultColour;
    }
}
