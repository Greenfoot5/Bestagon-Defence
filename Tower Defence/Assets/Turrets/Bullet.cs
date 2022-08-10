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
        
        [Tooltip("The speed of the bullet")]
        public UpgradableStat speed = new UpgradableStat(30f);
        [Tooltip("The radius to deal damage in. If <= 0, will just damage the target it hits")]
        public UpgradableStat explosionRadius;
        [Tooltip("The amount of damage the bullet deals (set bu turret)")]
        [HideInInspector]
        public UpgradableStat damage = new UpgradableStat(50f);
    
        [Tooltip("The effect spawned when the bullet hit's a target")]
        public GameObject impactEffect;

        private readonly List<Module> _modules = new List<Module>();
        
        /// <summary>
        /// Sets the new transform the bullet shoot go towards
        /// </summary>
        /// <param name="newTarget">The transform of the new target</param>
        /// <param name="turret">The turret telling the bullet to seek a target</param>
        public void Seek(Transform newTarget, Turret turret)
        {
            _target = newTarget;
            _source = turret;
            _targetLocation = Vector3.negativeInfinity;
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
        }

        /// <summary>
        /// Moves the bullet towards the target and check if it hits
        /// </summary>
        private void Update()
        {
            // Check the bullet still have a target to move towards
            if (_target == null && _targetLocation == Vector3.negativeInfinity)
            {
                Destroy(gameObject);
            }
            else if (_targetLocation == Vector3.negativeInfinity)
            {
                SeekTarget(_target.position, true);
            }
            else
            {
                SeekTarget(_targetLocation, false);
            }
        }

        private void SeekTarget(Vector3 location, bool isEnemy)
        {
            // Get the direction of the target, and the distance to move this frame
            Vector3 position = transform.position;
            Vector2 difference = location - position;
            float distanceThisFrame = speed.GetStat() * Time.deltaTime;
        
            // TODO - Make it based on target size
            const float targetSize = 0.25f;
            // Has the bullet "hit" the target?
            if (difference.sqrMagnitude <= targetSize * targetSize)
            {
                HitTarget(isEnemy);
return;
            }

            // Calculate movement of the bullet
            Vector2 movement = difference.normalized * distanceThisFrame;
            Vector2 prediction = difference + movement;

            // If within this frame the bullet will pass the enemy, it's a guaranteed hit
            if (difference.x >= 0 != prediction.x >= 0 || difference.y >= 0 != prediction.y >= 0)
            {
                HitTarget(isEnemy);
                return;
            }

            // Move the bullet towards the target
            transform.Translate(movement, Space.World);
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
    }
}
