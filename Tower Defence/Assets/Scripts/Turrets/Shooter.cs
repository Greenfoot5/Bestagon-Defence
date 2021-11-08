using System.Collections.Generic;
using System.Linq;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{

    public class Shooter : Turret
    {
        private Transform _target;
        private Enemy _targetEnemy;
        
        public float range = 2.5f;

        // Bullets + Area
        [Tooltip("Time between each shot")]
        public float fireRate = 1f;
        private float _fireCountdown;
        
        // Bullet + Laser
        public float turnSpeed = 3f;
        
        // Bullets
        public GameObject bulletPrefab;

        // Effects
        public float slowPercentage;
        
        // References
        public string enemyTag = "Enemy";
        public Transform partToRotate;
        public Transform firePoint;
        
        // Upgrades
        public List<Upgrade> upgrades = new List<Upgrade>();

        // Start is called before the first frame update
        private void Start()
        {
            // Call the function every 2s
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
            // Added when a turret is built.
            // TODO - Apply effects for pre-placed turrets in maps
            // foreach (var upgrade in turretUpgrades)
            // {
            //     AddUpgrade(upgrade);
            // }
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
                _fireCountdown -= Time.deltaTime;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
            {
                _fireCountdown -= Time.deltaTime;
                impactEffect.Stop();
                return;
            }
            
            
            if (_fireCountdown <= 0)
            {
                Shoot();
                _fireCountdown = 1 / fireRate;
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
        
        // Check we're actually looking at the target before shooting
        private bool IsLookingAtTarget()
        {
            if (_targetEnemy == null) return false;
            
            // Setup the raycast
            var results = new List<RaycastHit2D>();
            var contactFilter = new ContactFilter2D()
            {
                layerMask = LayerMask.GetMask(new[] { "Enemies" })
            };
            Physics2D.Raycast(firePoint.position, firePoint.up, contactFilter, results, range);
            
            // Loop through the hits to see if we can hit the target
            var foundEnemy = false;
            foreach (var hit in results.Where(hit => hit.transform == _targetEnemy.transform))
            {
                foundEnemy = true;
            }
            return foundEnemy;
        }

        // Create the bullet and set the target
        private void Shoot()
        {
            var bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bullet = bulletGO.GetComponent<Bullet>();

            if (bullet == null) return;
            
            foreach (var upgrade in upgrades)
            {
                bullet.AddUpgrade(upgrade);
            }
            bullet.Seek(_target);
        }

        // Visualises a circle of range when turret is selected
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        public bool AddUpgrade(Upgrade upgrade)
        {
            if (!upgrade.ValidUpgrade(this))
            {
                Debug.Log("Invalid Upgrade");
                return false;
            }

            upgrade.AddUpgrade(this);
            upgrades.Add(upgrade);
            return true;
        }
    }
}
