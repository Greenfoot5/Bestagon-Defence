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
        
        [SerializeField]
        public TurretType attackType;
        
        // Bullets + Area
        [Tooltip("Time between each shot")]
        public float fireRate = 1f;
        private float _fireCountdown;
        
        // Bullet + Laser
        public float turnSpeed = 3f;
        
        // Bullets
        public GameObject bulletPrefab;
        
        // Lasers
        public float damageOverTime = 5;
        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;
        
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
                if (attackType != TurretType.Laser || !lineRenderer.enabled) return;
                
                lineRenderer.enabled = false;
                impactEffect.Stop();
                return;
            }
        
            // Rotates the turret each frame
            if (attackType != TurretType.Area)
                LookAtTarget();
        
            // Check which attack type we're using
            if (attackType == TurretType.Laser)
            {
                FireLaser();
            }
            else switch (_fireCountdown <= 0)
            {
                case true when attackType == TurretType.Bullet:
                    Shoot();
                    _fireCountdown = fireRate;
                    break;
                case true when attackType == TurretType.Area:
                    Smash();
                    _fireCountdown = fireRate;
                    break;
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

            if (bullet == null) return;
            
            foreach (var upgrade in upgrades)
            {
                bullet.AddUpgrade(upgrade);
            }
            bullet.Seek(_target);
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
                return false;
            
            upgrade.AddUpgrade(this);
            upgrades.Add(upgrade);
            return true;
        }
    }
}
