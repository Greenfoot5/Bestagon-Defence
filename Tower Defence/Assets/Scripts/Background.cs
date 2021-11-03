using UnityEngine;
using UnityEngine.EventSystems;

public class Background : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Make sure we're hovering over the node and nothing else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Debug.Log("Deselected");
        BuildManager.instance.Deselect();
    }
}
