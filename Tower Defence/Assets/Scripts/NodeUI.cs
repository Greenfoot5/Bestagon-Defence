using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    
    private Node _target;

    public void SetTarget(Node node)
    {
        _target = node;

        transform.position = _target.transform.position;
        
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        _target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }
}
