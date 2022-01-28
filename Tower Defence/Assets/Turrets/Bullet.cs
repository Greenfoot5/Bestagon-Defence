using System.Collections.Generic;
using Abstract.Data;
using Enemies;
using UnityEngine;
using Upgrades.Modules;

namespace Turrets
{
    /// <summary>
    /// The bullet shot from a turret
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private Transform _target;
    
        public UpgradableStat speed = new UpgradableStat(30f);
        [Tooltip("Only set if the bullet deals AoE damage")]
        public float explosionRadius;
        [HideInInspector]
        public UpgradableStat damage = new UpgradableStat(50f);
    
        [Tooltip("The effect spawned when the bullet hit's a target")]
        public GameObject impactEffect;

        private readonly List<Module> _modules = new List<Module>();
        
        /// <summary>
        /// Sets the new transform the bullet shoot go towards
        /// </summary>
        /// <param name="newTarget">The transform of the new target</param>
        public void Seek(Transform newTarget)
        {
            _target = newTarget;
        }

        /// <summary>
        /// Moves the bullet towards the target and check if it hits
        /// </summary>
        private void Update()
        {
            // Check the bullet still have a target to move towards
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }
        
            // Get the direction of the target, and the distance to move this frame
            var position = transform.position;
            var dir = ((Vector2)_target.position - (Vector2)position);
            var distanceThisFrame = speed.GetStat() * Time.deltaTime;
        
            // TODO - Make it based on target size
            const float targetSize = 0.25f;
            // Has the bullet "hit" the target?
            if (dir.magnitude <= targetSize)
            {
                HitTarget();
                return;
            }
        
            // Move the bullet towards the target
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            var toTarget = _target.position - position;
            Vector3.Normalize(toTarget);
            transform.up = toTarget;

        }
    
        /// <summary>
        /// Called when the bullet hits the target
        /// </summary>
        private void HitTarget()
        {
            // Spawn hit effect
            var position = transform;
            var effectIns = Instantiate(impactEffect, position.position, position.rotation);
            
            // Add module effects
            var enemy = _target.GetComponent<Enemy>();
            foreach (var module in _modules)
            {
                module.OnHit(new []{enemy});
            }
            
            Destroy(effectIns, 2f);
        
            // If the bullet has AoE damage or not
            if (explosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                Damage(_target);
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

            if (em != null)
            {
                em.TakeDamage(damage.GetStat(), gameObject);
            }
        }
    
        /// <summary>
        /// Used to deal damage to multiple enemies
        /// </summary>
        private void Explode()
        {
            // Gets all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var collider2d in colliders)
            {
                if (collider2d.CompareTag("Enemy"))
                {
                    Damage(collider2d.transform);
                }
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
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
