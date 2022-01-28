using Enemies;
using UnityEngine;

namespace Upgrades.Abilities.NegativeAbilities
{
    /// <summary>
    /// Poisons an enemy, dealing damage.
    /// </summary>
    [CreateAssetMenu(fileName = "PoisonEnemy", menuName = "Enemy Abilities/Poison Enemy")]
    public class PoisonEnemyEffect : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("Static damage amount that's removed every tick, ticks being the timer")]
        public float damage;
        
        /// <summary>
        /// The object to activate a poison tick
        /// </summary>
        /// <param name="target">The object to poison</param>
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

            enemyComponent.TakeDamageWithoutAbilities(damage);
        }
        
        /// <summary>
        /// The ability doesn't do anything when the ability ends
        /// </summary>
        /// <param name="target"></param>
        public override void OnCounterEnd(GameObject target) { }
    }
}
