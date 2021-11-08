using System.Collections.Generic;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{

    public class Laser : Turret
    {
        private Transform _target;
        private Enemy _targetEnemy;
        
        public float range = 2.5f;

        // Bullets + Area
        [Tooltip("Time between each shot")]
        public float fireRate = 1f;
        private float _fireCountdown;
        
        // Area
        public float smashDamage = 20f;
        public ParticleSystem smashEffect;
        
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
                return;
            }
            
            if (_fireCountdown <= 0)
            {
                    Smash();
                    _fireCountdown = 1 / fireRate;
            }
            
            _fireCountdown -= Time.deltaTime;
        }

        private void Smash()
        {
            smashEffect.Play();
            
            // Get's all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, range);
            var enemies = new List<Enemy>();
            foreach (var collider2d in colliders)
            {
                if (!collider2d.CompareTag("Enemy")) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                enemies.Add(enemy);
                enemy.TakeDamage(smashDamage);
            }

            foreach (var upgrade in upgrades)
            {
                upgrade.OnHit(enemies.ToArray());
            }
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
