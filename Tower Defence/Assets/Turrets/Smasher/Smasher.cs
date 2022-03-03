using System.Collections.Generic;
using System.Linq;
using Enemies;
using Modules;
using UnityEngine;

namespace Turrets.Smasher
{
    /// <summary>
    /// Extends Turret to add smashing functionality
    /// </summary>
    public class Smasher : Turret
    {
        [Tooltip("The effect to play when the smasher attacks")]
        [SerializeField]
        private ParticleSystem smashEffect;

        /// <summary>
        /// Check for new enemies in radius and attacks if there are.
        /// </summary>
        private void Update()
        {
            // Don't do anything if no enemy is in range
            var results = new Collider2D[128];
            Physics2D.OverlapCircleNonAlloc(transform.position, range.GetStat(), results);
            if (!results.Any(x => 
                    x != null && x.CompareTag(enemyTag)))
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
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range.GetStat());
            
            var enemies = new List<Enemy>();
            foreach (Collider2D collider2d in results)
            {
                if (!collider2d.CompareTag(enemyTag)) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                enemies.Add(enemy);
                
                // Take damage depending on how close the enemy is to the turret's centre
                float damagePercentage = 1 - (transform.position - collider2d.transform.position).sqrMagnitude /
                                         (range.GetTrueStat() * range.GetTrueStat());
                // Only deal damage if it will actually damage the enemy
                if (damagePercentage > 0)
                {
                    enemy.TakeDamage(damage.GetTrueStat() * damagePercentage, gameObject);
                }
            }
            
            // Activates the turret's OnHit modules
            foreach (Module module in modules)
            {
                module.OnHit(enemies.ToArray());
            }
        }
    }
}
