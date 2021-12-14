using System.Collections.Generic;
using Enemies;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets
{
    public class Bullet : MonoBehaviour
    {
        private Transform _target;
    
        public UpgradableStat speed = new UpgradableStat(30f);
        [Tooltip("Only set if we're dealing AoE")]
        public float explosionRadius;
        [HideInInspector]
        public UpgradableStat damage = new UpgradableStat(50f);
    
        [Tooltip("The effect spawned when the bullet hit's a target")]
        public GameObject impactEffect;

        private readonly List<Upgrade> _upgrades = new List<Upgrade>();

        public void Seek(Transform newTarget)
        {
            _target = newTarget;
        }

        // Update is called once per frame
        private void Update()
        {
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
            // Have we "hit" the target
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
    
        // Called when we hit the target
        private void HitTarget()
        {
            // Spawn hit effect
            var position = transform;
            var effectIns = Instantiate(impactEffect, position.position, position.rotation);
            
            // Add upgrade effects
            var enemy = _target.GetComponent<Enemy>();
            foreach (var upgrade in _upgrades)
            {
                upgrade.OnHit(new []{enemy});
            }
            
            Destroy(effectIns, 2f);
        
            // If we have AoE effect or not
            if (explosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                Damage(_target);
            }

            // Destroy so we only hit once
            Destroy(gameObject);
        }
    
        // Called when dealing damage to a single enemy
        private void Damage(Component enemy)
        {
            var em = enemy.GetComponent<Enemy>();

            if (em != null)
            {
                em.TakeDamage(damage.GetStat(), gameObject);
            }
        }
    
        // Called if we have an AoE effect
        private void Explode()
        {
            // Get's all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var collider2d in colliders)
            {
                if (collider2d.CompareTag("Enemy"))
                {
                    Damage(collider2d.transform);
                }
            }
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            upgrade.OnShoot(this);
            _upgrades.Add(upgrade);
        }
    
        // Allows us to visualise the bullet's AoE in the editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
