using Enemies;
using UnityEngine;

namespace _WIP.Abilities.NegativeAbilities
{
    /// <summary>
    /// Stuns an enemy for a duration, holding them in palace
    /// </summary>
    [CreateAssetMenu(fileName = "StunEnemy", menuName = "Enemy Abilities/Stun Enemy")]
    public class StunEnemyEffect : EnemyAbility
    {
        /// <summary>
        /// Stuns the enemy, holding them in place
        /// </summary>
        /// <param name="target">The enemy to stun</param>
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

            enemyComponent.speed -= enemyComponent.startSpeed * 5000f;
        }
        
        /// <summary>
        /// Makes the enemy no longer stunned
        /// </summary>
        /// <param name="target">The enemy to de-stun</param>
        public override void OnCounterEnd(GameObject target)
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

            enemyComponent.speed += enemyComponent.startSpeed * 5000f;
        }
    }
}