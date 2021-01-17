using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform _target;
    private Enemy _targetEnemy;
    
    [Header("Attributes")]
    
    public float range = 2.5f;
    public float turnSpeed = 3f;
    
    [Header("Bullets (default)")]
    public float fireRate = 1f;
    private float _fireCountdown;
    public GameObject bulletPrefab;

    [Header("User Laser")] 
    public bool useLaser;

    public float damageOverTime = 5;
    [Range(0, 1)]
    public float slowPercentage;
    
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    
    [Header("References")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    
    public Transform firePoint;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Call the function every 2s
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        // Create a list of enemies and remember shortest distance and enemy
        var enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        var shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        // Loop through the enemies and find the closest
        foreach (var enemy in enemies)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            // Save if new closest enemy
            if (!(distanceToEnemy < shortestDistance)) continue;
            shortestDistance = distanceToEnemy;
            nearestEnemy = enemy;
        }
        
        // Check if we have a target and should shoot
        if (nearestEnemy != null && shortestDistance <= range)
        {
            _target = nearestEnemy.transform;
            _targetEnemy = _target.GetComponent<Enemy>();
        }
        // Set our target to null if out of range
        else
        {
            _target = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Don't do anything if we don't have a target
        if (_target == null)
        {
            if (useLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
            }
            
            return;
        }

        LookAtTarget();

        if (useLaser)
        {
            FireLaser();
        }
        else if (_fireCountdown <= 0)
        {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    private void LookAtTarget()
    {
        var aimDir = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        var up = partToRotate.up;
        var lookDir = Vector3.Lerp(up, aimDir, Time.deltaTime * turnSpeed);
        transform.rotation *= Quaternion.FromToRotation(up, lookDir);
    }

    // Create the bullet and set the target
    private void Shoot()
    {
        var bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(_target);
        }
    }

    private void FireLaser()
    {
        // Deal damage
        _targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        
        // Slow the enemy
        _targetEnemy.Slow(slowPercentage);

        // Enable visuals
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
        }
        
        // Set Laser positions
        var targetPosition = _target.position;
        var firePointPosition = firePoint.position;
        lineRenderer.SetPosition(0, firePointPosition);
        lineRenderer.SetPosition(1, targetPosition);

        // Set impact effect rotation
        var impactEffectTransform = impactEffect.transform;
        var aimDir = (Vector3)((Vector2)firePointPosition - (Vector2)impactEffectTransform.position).normalized;
        impactEffectTransform.rotation = Quaternion.LookRotation(aimDir);

        // Set impact effect position
        impactEffectTransform.position = targetPosition + aimDir * 0.2f;
    }
    
    // Visualises a circle of range when turret is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
