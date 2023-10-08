using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.VFX;

namespace Turrets.Smasher
{
    /// <summary>
    /// Extends Turret to add smashing functionality
    /// </summary>
    public class Smasher : Turret
    {
        [Tooltip("The effect to play when the smasher attacks")]
        [SerializeField]
        private VisualEffect smashEffect;

        /// <summary>
        /// Check for new enemies in radius and attacks if there are.
        /// </summary>
        private void Update()
        {
            // If there's no fire rate, the turret shouldn't do anything
            if (fireRate.GetStat() == 0)
            {
                return;
            }
            
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
            if (fireCountdown <= 0 && fireRate.GetStat() != 0)
            {
                fireCountdown = 1 / fireRate.GetStat();
                Attack();
            }
            
            fireCountdown -= Time.deltaTime;
        }

        public override void UpdateRange()
        {
            // Update the effect radius
            smashEffect.SetFloat("size", range.GetStat() * (7f/3f));
            // Update the range shader's size
            Vector3 localScale = transform.localScale;
            rangeDisplay.transform.localScale = new Vector3(
                range.GetStat() / localScale.x * 2,
                range.GetStat() / localScale.y * 2,
                1);
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack()
        {
            smashEffect.Play();
            
            base.Attack(this);
            
            // Gets all the enemies in the AoE and calls Damage on them
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, range.GetStat());
            
            foreach (Collider2D collider2d in results)
            {
                if (!collider2d.CompareTag(enemyTag)) continue;
                
                var enemy = collider2d.GetComponent<Enemy>();
                
                // Take damage depending on how close the enemy is to the turret's centre
                float distance = 1 - (transform.position - collider2d.transform.position).sqrMagnitude /
                    (range.GetTrueStat() * range.GetTrueStat());
                float damagePercentage = Mathf.Clamp(distance + 0.33f, 0f, 1f);
                
                // Only deal damage if it will actually damage the enemy
                if (!(damagePercentage > 0)) continue;
                
                Hit(enemy, this);
                enemy.TakeDamage(damage.GetTrueStat() * damagePercentage, gameObject);
            }
        }
    }
}
