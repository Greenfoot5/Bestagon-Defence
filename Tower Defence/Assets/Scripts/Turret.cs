﻿using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform _target;
    
    [Header("Attributes")]
    
    public float range = 2.5f;
    public float fireRate = 1f;
    private float _fireCountdown;
    public float turnSpeed = 3f;
    
    [Header("References")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    
    public GameObject bulletPrefab;
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
            return;
        }
        
        // Rotate turret to look at target
        var aimDir = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        var up = partToRotate.up;
        var lookDir = Vector3.Lerp(up, aimDir, Time.deltaTime * turnSpeed);
        transform.rotation *= Quaternion.FromToRotation(up, lookDir);

        if (_fireCountdown <= 0)
        {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    // Create the bullet and set the target
    void Shoot()
    {
        var bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(_target);
        }
    }
    
    // Visualises a circle of range when turret is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}