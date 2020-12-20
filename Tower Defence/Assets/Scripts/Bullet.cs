using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    
    public float speed = 30f;

    public GameObject impactEffect;

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
        
        // Get the direction of the target, and the distance to move this frame
        var dir = ((Vector2)_target.position - (Vector2)transform.position);
        var distanceThisFrame = speed * Time.deltaTime;
        
        // TODO - Make it based on target size
        const float targetSize = 0.25f;
        // Have we "hit" the target
        if (dir.magnitude <= targetSize)
        {
            HitTarget();
            return;
        }
        
        // Move the bullet towards the target
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
    
    // Called when we hit the target
    private void HitTarget()
    {
        // Spawn hit effect
        var effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
        
        // TODO - should damage target
        Destroy(_target.gameObject);
        
        // Destroy so we only hit once
        Destroy(gameObject);
    }
}
