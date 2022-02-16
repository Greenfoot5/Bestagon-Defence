using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// Extends Turret to add smashing functionality
    /// </summary>
    public class Smasher : Turret
    {
        public ParticleSystem smashEffect;

        /// <summary>
        /// Check for new enemies in radius and attacks if there are.
        /// </summary>
        private void Update()
        {
            // Don't do anything if no enemy is in range
            if (!Physics2D.OverlapCircleAll(transform.position, range.GetStat()).Any(x => 
                x.CompareTag(enemyTag)))
            {
                fireCountdown -= Time.deltaTime;
                return;
            }
            
            // If our attack is off cooldown
            if (fireCountdown <= 0)
            {
                Attack();
                fireCountdown = 1 / fireRate.GetStat();
            }
            
            fireCountdown -= Time.deltaTime;
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack()
        {
            smashEffect.Play();
            
            // Gets all the enemies in the AoE and calls Damage on them
            var colliders = Physics2D.OverlapCircleAll(transform.position, range.GetStat());
            var enemies = new List<Enemy>();
            foreach (var collider2d in colliders)
            {
                if (!collider2d.CompareTag(enemyTag)) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                enemies.Add(enemy);
                enemy.TakeDamage(damage.GetStat(), gameObject);
            }
            
            // Activates the turret's OnHit modules
            foreach (var module in modules)
            {
                module.OnHit(enemies.ToArray());
            }
        }
    }
}
