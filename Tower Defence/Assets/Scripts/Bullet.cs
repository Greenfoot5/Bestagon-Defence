using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    
    public float speed = 30f;
    public float explosionRadius;
    public int damage = 50;

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
        var position = transform.position;
        var dir = ((Vector2)_target.position - (Vector2)position);
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
        var toTarget = _target.position - position;
        Vector3.Normalize(toTarget);
        transform.up = toTarget;

    }
    
    // Called when we hit the target
    private void HitTarget()
    {
        // Spawn hit effect
        var position = transform;
        var effectIns = Instantiate(impactEffect, position.position, position.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }

        // Destroy so we only hit once
        Destroy(gameObject);
    }

    void Damage(Transform enemy)
    {
        Enemy em = enemy.GetComponent<Enemy>();

        if (em != null)
        {
            em.TakeDamage(damage);
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        Debug.Log(colliders);
        foreach (Collider2D collider2d in colliders)
        {
            if (collider2d.CompareTag("Enemy"))
            {
                Damage(collider2d.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
