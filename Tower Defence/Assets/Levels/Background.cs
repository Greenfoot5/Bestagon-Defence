using Abstract.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Background : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Make sure the player is hovering over the node and nothing else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        BuildManager.instance.Deselect();
    }
}
