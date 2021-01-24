using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private Node _target;

    public void SetTarget(Node node)
    {
        _target = node;

        transform.position = _target.transform.position;
    }
}
