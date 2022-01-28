using Enemies;
using UnityEngine;

namespace Upgrades.Abilities.NegativeAbilities
{
    /// <summary>
    /// Burns the enemy the ability is applied to
    /// </summary>
    [CreateAssetMenu(fileName = "BurnEnemy", menuName = "Enemy Abilities/Burn Enemy")]
    public class BurnEnemyEffect : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("% of max health damage per tick")]
        public float damage;
        
        /// <summary>
        /// Burn the enemy and deal damage to it
        /// </summary>
        /// <param name="target">The object to burn</param>
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

            enemyComponent.TakeDamageWithoutAbilities(enemyComponent.maxHealth * damage);
        }
        
        /// <summary>
        /// The ability doesn't do anything when the ability ends
        /// </summary>
        public override void OnCounterEnd(GameObject target) { }
    }
}