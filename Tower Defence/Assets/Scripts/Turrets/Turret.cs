using System;
using System.Collections.Generic;
using System.Linq;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// The various targeting methods a turret can use to find a target
    /// </summary>
    public enum TargetingMethod
    {
        Closest = 0,
        Weakest = 1,
        Strongest = 2
    }
    
    public abstract class Turret : MonoBehaviour
    {
        public float damage;
        
        // Targeting
        protected Transform _target;
        protected Enemy _targetEnemy;
        public string enemyTag = "Enemy";
        
        public float range = 2.5f;
        public TargetingMethod targetingMethod = TargetingMethod.Closest;
        public bool aggressiveRetargeting = true;

        // Attack speed
        [Tooltip("Time between each shot")]
        public float fireRate = 1f;
        protected float _fireCountdown;
        
        // Rotation
        public float turnSpeed = 3f;
        public Transform partToRotate;
        
        public Transform firePoint;
        
        // Effects
        public float slowPercentage;
        
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
        
        /// <summary>
        /// Update our current target to check if it's still the most valuable, or pick a new one.
        /// </summary>
        private void UpdateTarget()
        {
            // If we're not aggressively retargeting, check if the target is still in range
            if (!aggressiveRetargeting)
            {
                var distanceToEnemy = Vector3.Distance(transform.position, _target.position);
                if (distanceToEnemy <= range) return;
            }
            
            // Create a list of enemies within range
            var enemiesInRange = (from enemy in GameObject.FindGameObjectsWithTag(enemyTag)
                let distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position)
                where distanceToEnemy <= range select enemy).ToArray();
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
                _target = null;
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
                _target = mostValuableEnemy.transform;
                _targetEnemy = _target.GetComponent<Enemy>();
            }
            // Set our target to null if there is none
            else
            {
                _target = null;
            }
        }
        
        /// <summary>
        /// Rotates the turret towards our target
        /// </summary>
        protected void LookAtTarget()
        {
            // Get's the rotation we need to end up at, and lerp each frame towards that
            var aimDir = ((Vector2)_target.position - (Vector2)transform.position).normalized;
            var up = partToRotate.up;
            var lookDir = Vector3.Lerp(up, aimDir, Time.deltaTime * turnSpeed);
            transform.rotation *= Quaternion.FromToRotation(up, lookDir);
        }
        
        /// <summary>
        /// Check we're currently looking at our target.
        /// Used to see if we can shoot on single-target turrets.
        /// </summary>
        /// <returns>If we're currently looking at the target</returns>
        protected bool IsLookingAtTarget()
        {
            if (_targetEnemy == null) return false;
            
            // Setup the raycast
            var results = new List<RaycastHit2D>();
            var contactFilter = new ContactFilter2D()
            {
                layerMask = LayerMask.GetMask("Enemies")
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

        protected abstract void Attack();

        /// <summary>
        /// Allows the editor to display the range of the turret
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        
        /// <summary>
        /// Adds upgrades to our turret after checking they're valid.
        /// </summary>
        /// <param name="upgrade">The upgrade to apply to the turret</param>
        /// <returns>true If the upgrade was applied successfully</returns>
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
