using System.Collections.Generic;
using Abstract.Data;
using Enemies;
using Modules;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// The bullet shot from a turret
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private Turret _source;
        private Transform _target;
        private Vector3 _targetLocation;
        private bool _useLocation;

        [Header("Types")]
        [Tooltip("Hits all enemies on path, but does not destroy itself")]
        public bool isEthereal;
        
        [Header("Stats")]
        [Tooltip("The speed of the bullet")]
        public UpgradableStat speed = new(30f);
        [Tooltip("The radius to deal damage in. If <= 0, will just damage the target it hits")]
        public UpgradableStat explosionRadius;
        [Tooltip("The knockback the bullet deals to a target (set by turret)")]
        public UpgradableStat knockbackAmount;
        [Tooltip("The amount of damage the bullet deals (set by turret)")]
        [HideInInspector]
        public UpgradableStat damage = new(50f);
    
        [Tooltip("The effect spawned when the bullet hit's a target")]
        public GameObject impactEffect;

        private readonly List<Module> _modules = new();
        private readonly List<int> _hitEnemies = new();
        
        /// <summary>
        /// Sets the new transform the bullet shoot go towards
        /// </summary>
        /// <param name="newTarget">The transform of the new target</param>
        /// <param name="turret">The turret telling the bullet to seek a target</param>
        public void Seek(Transform newTarget, Turret turret)
        {
            _target = newTarget;
            _source = turret;
            _useLocation = false;
        }
        
        /// <summary>
        /// Sets the location the bullet goes towards
        /// </summary>
        /// <param name="location">The location the bullet goes towards</param>
        /// <param name="turret">The turret telling the bullet to seek the location</param>
        public void Seek(Vector3 location, Turret turret)
        {
            _targetLocation = location;
            _source = turret;
            _useLocation = true;
        }

        /// <summary>
        /// Moves the bullet towards the target and check if it hits
        /// </summary>
        private void Update()
        {
            // Check the bullet still have a target to move towards
            if (_target == null && !_useLocation)
            {
                Destroy(gameObject);
            }
            else if (_useLocation)
            {
                SeekTarget(_targetLocation, false);
            }
            else
            {
                SeekTarget(_target.position, true);
            }
        }
        
        /// <summary>
        /// Moves the bullet towards a target location
        /// </summary>
        /// <param name="location">The location to move towards</param>
        /// <param name="isEnemy">If the location is an enemy</param>
        private void SeekTarget(Vector3 location, bool isEnemy)
        {
            // Get the direction of the target, and the distance to move this frame
            Vector3 position = transform.position;
            float distanceThisFrame = speed.GetStat() * Time.deltaTime;
            
            // Move bullet towards target
            transform.position = Vector2.MoveTowards(position, location, distanceThisFrame);

            Vector2 difference = location - position;
            // TODO - Make it based on target size
            const float targetSize = 0.25f;
            
            // Has the bullet "hit" the target?
            if (difference.sqrMagnitude <= targetSize * targetSize)
            {
                HitTarget(isEnemy); 
                return;
            }
            
            // Rotate to target
            transform.up = (location - position).normalized;
        }

        /// <summary>
        /// Called when the bullet hits the target
        /// </summary>
        private void HitTarget(bool isEnemy)
        {
            // Spawn hit effect
            Transform position = transform;
            GameObject effectIns = Instantiate(impactEffect, position.position, position.rotation);
            effectIns.name = "_" + effectIns.name;

            Destroy(effectIns, 2f);

            if (isEnemy)
            {
                // If the bullet has AoE damage or not
                if (explosionRadius.GetStat() > 0f)
                {
                    Explode();
                }
                else
                {
                    Damage(_target);
                }
            }

            // Destroy so the bullet only hits the target once
            Destroy(gameObject);
        }
    
        /// <summary>
        /// Used to deal damage to a single enemy
        /// </summary>
        /// <param name="enemy">The enemy to deal damage to</param>
        private void Damage(Component enemy)
        {
            var em = enemy.GetComponent<Enemy>();
            
            // Add module effects
            foreach (Module module in _modules)
            {
                module.OnHit(new []{em}, _source, this);
            }

            if (em == null) return;

            if (knockbackAmount.GetTrueStat() != 0)
            {
                em.GetComponent<EnemyMovement>().TakeKnockback(knockbackAmount.GetTrueStat(), _source.transform.position);
            }

            em.TakeDamage(damage.GetStat(), gameObject);
        }
    
        /// <summary>
        /// Used to deal damage to multiple enemies
        /// </summary>
        private void Explode()
        {
            // Gets all the enemies in the AoE and calls Damage on them
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius.GetStat());
            foreach (Collider2D collider2d in colliders)
            {
                if (!collider2d.CompareTag("Enemy")) continue;
                
                foreach (Module module in _modules)
                {
                    module.OnHit(new []{collider2d.GetComponent<Enemy>()}, _source, this);
                }
                Damage(collider2d.transform);
            }
        }
        
        /// <summary>
        /// Adds a module to the bullet
        /// </summary>
        /// <param name="module">The module to add</param>
        public void AddModule(Module module)
        {
            module.OnShoot(this);
            _modules.Add(module);
        }
    
        /// <summary>
        /// Displays the bullet's AoE in the editor
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, explosionRadius.GetStat());
        }
        
        /// <summary>
        /// Called when the bullet touches an enemy
        /// It may not result in a hit
        /// </summary>
        /// <param name="col">The collider that was touched</param>
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isEthereal) return;

            if (_hitEnemies.Contains(col.gameObject.GetInstanceID())) return;
            _hitEnemies.Add(col.gameObject.GetInstanceID());

                // We don't want to hit the target twice
            if (_target != null && _target == col.transform) return;

            if (col.transform.CompareTag("Enemy"))
                Damage(col.gameObject.GetComponent<Enemy>());
        }
    }
}
