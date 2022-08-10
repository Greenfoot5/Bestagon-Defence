using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Modules;
using UnityEngine;

namespace Turrets.Lancer
{
    /// <summary>
    /// Extends Turret to add lancing functionality
    /// </summary>
    public class Lancer : Turret
    {
        /// <summary>How long between each target update</summary>
        private const float UpdateTargetTimer = 0.5f;
        
        [Tooltip("The bullet prefab to spawn each attack")]
        [SerializeField]
        private GameObject bulletPrefab;

        [Tooltip("The current target")]
        private Transform _target;
        [Tooltip("The Enemy script of the current target")]
        private Enemy _targetEnemy;
        
        // Reference
        [Tooltip("The transform at which attack from (e.g. instantiate bullets)")]
        [SerializeField]
        private Transform firePoint;

        [Tooltip("The Transform to perform any rotations on")]
        [SerializeField]
        private Transform partToRotate;

        /// <summary>
        /// Begins the target searching
        /// </summary>
        private void Start()
        {
            // Start finding targets
            StartCoroutine(TargetCoroutine());
        }
        
        /// <summary>
        /// Calls our targeting method every UpdateTargetTimer.
        /// </summary>
        private IEnumerator TargetCoroutine()
        {
            while (gameObject.activeSelf)
            {
                HasATarget();
                yield return new WaitForSeconds(UpdateTargetTimer);
            }
        }
        
        /// <summary>
        /// Check the turret is currently looking at our target.
        /// Used to see if the turret can shoot or needs to rotate more
        /// </summary>
        /// <returns>If the turret is currently looking at the target</returns>
        private bool HasATarget()
        {
            //if (_targetEnemy == null) return false;

            // Setup the raycast
            var results = new List<RaycastHit2D>();
            var contactFilter = new ContactFilter2D()
            {
                layerMask = LayerMask.GetMask("Enemies")
            };
            Physics2D.Raycast(firePoint.position, firePoint.up, contactFilter, results, range.GetStat());

            // Loop through the hits to see if the turret can hit the target
            var foundEnemy = false;
            foreach (RaycastHit2D unused in results.Where(hit => hit.transform.CompareTag("Enemy")))
            {
                foundEnemy = true;
            }
            return foundEnemy;
        }

        /// <summary>
        /// Check for new enemies in attack range
        /// </summary>
        private void Update()
        {
            // Don't do anything if no enemy is in range
            if (!HasATarget())
            {
                fireCountdown -= Time.deltaTime;
                return;
            }
            
            // If our attack is off cooldown
            if (fireCountdown <= 0)
            {
                fireCountdown = 1 / fireRate.GetStat();
                Attack();
            }
            
            fireCountdown -= Time.deltaTime;
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack()
        {
            // Creates the bullet
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletGo.name = "_" + bulletGo.name;
            var bullet = bulletGo.GetComponent<Bullet>();
            bullet.damage = damage;
            
            // If for some reason the bullet no longer has a Bullet component
            if (bullet == null) return;
            
            // Adds the modules to the bullet
            foreach (Module module in modules)
            {
                bullet.AddModule(module);
            }
            
            // Get the end point of the line renderer
            Vector3 direction = (firePoint.up * range.GetStat());
            Vector3 endPosition = (direction + transform.position);
            
            bullet.Seek(endPosition, this);
        }
    }
}
