using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets
{
    public abstract class DynamicTurret : Turret
    {
        // Targeting
        public TargetingMethod targetingMethod = TargetingMethod.Closest;
        public bool aggressiveRetargeting = true;

        protected Transform target;
        protected Enemy targetEnemy;

        // Reference
        public Transform firePoint;

        // Rotation
        [FormerlySerializedAs("turnSpeed")]
        public UpgradableStat rotationSpeed = new UpgradableStat(3f);
        public Transform partToRotate;


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


        /// <summary>
        /// Update our current target to check if it's still the most valuable, or pick a new one.
        /// </summary>
        private void UpdateTarget()
        {
            // If we're not aggressively retargeting, check if the target is still in range
            if (!aggressiveRetargeting)
            {
                var distanceToEnemy = Vector3.Distance(transform.position, target.position);
                if (distanceToEnemy <= range.GetStat()) return;
            }

            // Create a list of enemies within range
            var enemiesInRange = (from enemy in GameObject.FindGameObjectsWithTag(enemyTag)
                let distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position)
                where distanceToEnemy <= range.GetStat()
                select enemy).ToArray();
            // Set the current value to be too high or too low.
            // Value is based on targeting method
            var currentValue = Mathf.Infinity;
            if (targetingMethod == TargetingMethod.Strongest)
            {
                currentValue = Mathf.NegativeInfinity;
            }
            GameObject mostValuableEnemy = null;

            // Check there are enemies in range, and if not, we have no target
            if (enemiesInRange.Length == 0)
            {
                target = null;
                return;
            }

            // Loop through the enemies and find the most valuable
            foreach (var enemy in enemiesInRange)
            {
                switch (targetingMethod)
                {
                    case TargetingMethod.Closest:
                        // Find if the enemy is closer than our current most valuable
                        var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < currentValue)
                        {
                            currentValue = distanceToEnemy;
                            mostValuableEnemy = enemy;
                        }
                        break;
                    case TargetingMethod.Weakest:
                        // Find if the enemy has less health than our current most valuable
                        var health = enemy.GetComponent<Enemy>().health;
                        if (health < currentValue)
                        {
                            currentValue = health;
                            mostValuableEnemy = enemy;
                        }
                        break;
                    case TargetingMethod.Strongest:
                        // Find if the enemy has more health than our current most valuable
                        var enemyHealth = enemy.GetComponent<Enemy>().health;
                        if (enemyHealth > currentValue)
                        {
                            currentValue = enemyHealth;
                            mostValuableEnemy = enemy;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Check if we have a valid target
            if (mostValuableEnemy != null)
            {
                target = mostValuableEnemy.transform;
                targetEnemy = target.GetComponent<Enemy>();
            }
            // Set our target to null if there is none
            else
            {
                target = null;
            }
        }

        /// <summary>
        /// Rotates the turret towards our target
        /// </summary>
        protected void LookAtTarget()
        {
            // Get's the rotation we need to end up at, and lerp each frame towards that
            var aimDir = ((Vector2)target.position - (Vector2)transform.position).normalized;
            var up = partToRotate.up;
            var lookDir = Vector3.Lerp(up, aimDir, Time.deltaTime * rotationSpeed.GetStat());
            transform.rotation *= Quaternion.FromToRotation(up, lookDir);
        }

        /// <summary>
        /// Check we're currently looking at our target.
        /// Used to see if we can shoot on single-target turrets.
        /// </summary>
        /// <returns>If we're currently looking at the target</returns>
        protected bool IsLookingAtTarget()
        {
            if (targetEnemy == null) return false;

            // Setup the raycast
            var results = new List<RaycastHit2D>();
            var contactFilter = new ContactFilter2D()
            {
                layerMask = LayerMask.GetMask("Enemies")
            };
            Physics2D.Raycast(firePoint.position, firePoint.up, contactFilter, results, range.GetStat());

            // Loop through the hits to see if we can hit the target
            var foundEnemy = false;
            foreach (var hit in results.Where(hit => hit.transform == targetEnemy.transform))
            {
                foundEnemy = true;
            }
            return foundEnemy;
        }
    }
}
