using Enemies;
using UnityEngine;

namespace _WIP.Abilities.NegativeAbilities
{
    /// <summary>
    /// Slows the enemy the ability is attached to
    /// </summary>
    [CreateAssetMenu(fileName = "SlowEnemy", menuName = "Enemy Abilities/Slow Enemy")]
    public class SlowEnemyEffect : EnemyAbility
    {
        [Header("Ability Stats")]
        public float slowPercentage = 0.2f;
        
        /// <summary>
        /// Slows an enemy's movement speed
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        public override void Activate(GameObject target)
        {
            if (target == null)
            {
                return;
            }
            
            // Check the target is an enemy
            var enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent == null)
            {
                return;
            }
            
            // Don't slow if there's already a greater slow, or one of the same value
            if (!(enemyComponent.startSpeed * (1 - slowPercentage) < enemyComponent.speed))
            {
                return;
            }
            
            enemyComponent.speed *= 1f - slowPercentage;
        }
        
        /// <summary>
        /// Increase the enemy's movement speed back to normal
        /// </summary>
        /// <param name="target">The enemy to accelerate</param>
        public override void OnCounterEnd(GameObject target)
        {
            if (target == null)
            {
                return;
            }
            
            var enemyComponent = target.GetComponent<Enemy>();
            
            if (enemyComponent == null)
            {
                return;
            }
            
            enemyComponent.speed /= 1f - slowPercentage;
        }
    }
}