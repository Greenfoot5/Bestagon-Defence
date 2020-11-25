using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;

    public float speed = 30f;

    public void Seek(Transform newTarget)
    {
        _target = newTarget;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        var dir = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        var distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        
        transform.Translate(dir * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Debug.Log("We hit something!");
    }
}
