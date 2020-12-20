using UnityEngine;

public class Node : MonoBehaviour
{
    // The colour to set out node to
    public Color hoverColour;
    private Color _defaultColour;

    private GameObject _turret;

    private Renderer _rend;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _defaultColour = _rend.material.color;
    }

    private void OnMouseDown()
    {
        // TODO - Enable turret upgrades
        if (_turret != null)
        {
            Debug.Log("Turret already built!");
        }

        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        _turret = Instantiate(turretToBuild, transform.position, transform.rotation);
    }
    
    private void OnMouseEnter()
    {
        _rend.material.color = hoverColour;
    }

    private void OnMouseExit()
    {
        _rend.material.color = _defaultColour;
    }
}
