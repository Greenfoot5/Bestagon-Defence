using System.Collections.Generic;
using System.Linq;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{

    public class Smasher : Turret
    {
        private Transform _target;
        private Enemy _targetEnemy;
        
        public float range = 2.5f;

        // Bullet + Laser
        public float turnSpeed = 3f;

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
                if (!lineRenderer.enabled) return;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget();

            if (!IsLookingAtTarget())
            {
                if (!lineRenderer.enabled) return;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }
            
        

            FireLaser();
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

        // TODO - Animate the laser slightly (make it pulse)
        private void FireLaser()
        {
            // Deal damage
            _targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

            foreach (var upgrade in upgrades)
            {
                upgrade.OnHit(new [] {_targetEnemy});
            }

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
