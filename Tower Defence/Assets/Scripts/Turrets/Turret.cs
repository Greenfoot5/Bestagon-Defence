using System.Collections.Generic;
using Turrets.Upgrades.BulletUpgrades;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;

namespace Turrets
{
    public enum TurretType
    {
        Bullet = 0,
        Laser = 1,
        Area = 2
    }
    
    public class Turret : MonoBehaviour
    {
        private Transform _target;
        private Enemy _targetEnemy;
        
        public float range = 2.5f;
        public float turnSpeed = 3f;
        
        public TurretType attackType;
        
        // Bullets
        public float fireRate = 1f;
        private float _fireCountdown;
        public GameObject bulletPrefab;
        
        // Lasers
        public float damageOverTime = 5;
        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;
        
        // Effects
        public float slowPercentage;
        
        // References
        public string enemyTag = "Enemy";
        public Transform partToRotate;
        public Transform firePoint;
        
        // Upgrades
        public List<TurretUpgrade> turretUpgrades = new List<TurretUpgrade>();
        public List<BulletUpgrade> bulletUpgrades = new List<BulletUpgrade>();

        // Start is called before the first frame update
        private void Start()
        {
            // Call the function every 2s
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
            foreach (TurretUpgrade upgrade in turretUpgrades)
            {
                AddUpgrade(upgrade);
            }
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
                if (attackType == TurretType.Laser && lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                }
            
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();
        
            // Check which attack type we're using
            if (attackType == TurretType.Laser)
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
            // Get's the rotation we need to end up at, and lerp each frame towards that
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
    
        // TODO - Animate the laser slightly (make it pulse)
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

        // TODO - Actually check the upgrade is valid
        public bool AddUpgrade(TurretUpgrade upgrade)
        {
            // if (!upgrade.ValidUpgrade(this))
            //     return false;
            
            Debug.Log("upgrade.AddUpgrade");
            upgrade.AddUpgrade(this);
            
            turretUpgrades.Add(upgrade);

            return true;
        }

        public bool AddUpgrade(BulletUpgrade upgrade)
        {
            Debug.Log("Bullet Upgrade");
            return false;
        }

        public bool AddUpgrade(Upgrade upgrade)
        {
            Debug.Log("Generic Upgrade");
            switch (upgrade)
            {
                case TurretUpgrade turretUpgrade:
                    Debug.Log("Turret Upgrade");
                    return AddUpgrade(turretUpgrade);
                case BulletUpgrade bulletUpgrade:
                    Debug.Log("Bullet Upgrade");
                    return AddUpgrade(bulletUpgrade);
                default:
                    Debug.Log("Invalid Upgrade Type");
                    return false;
            }
        }
    }
}
