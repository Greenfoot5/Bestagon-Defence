using Enemies;
using UnityEngine;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    [CreateAssetMenu(fileName = "TimedMinorSelfHeal", menuName = "Enemy Abilities/Heal Enemy")]
    public class HealEnemyAbility : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("If the heal is a % heal or a value heal")]
        [SerializeField]
        private bool isPercentage = true;
        [Tooltip("How much to heal for (value heal only)")]
        [SerializeField]
        private int healAmount = 20;
        [Tooltip("What percentage to heal by (% heal only)")]
        [SerializeField]
        private float healPercentage = 0.2f;
        
        /// <summary>
        /// Heals an enemy
        /// </summary>
        /// <param name="target">The enemy to heal</param>
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
            
            // Heal the target
            if (isPercentage)
            {
                enemyComponent.health += enemyComponent.maxHealth * healPercentage;
            }
            else
            {
                enemyComponent.health += healAmount;
            }
            
            // Make sure health isn't above cap
            if (enemyComponent.health > enemyComponent.maxHealth)
            {
                enemyComponent.health = enemyComponent.maxHealth;
            }

            enemyComponent.leftBar.fillAmount = enemyComponent.health / enemyComponent.maxHealth;
            enemyComponent.rightBar.fillAmount = enemyComponent.health / enemyComponent.maxHealth;
        }
        
        /// <summary>
        /// There's nothing to clear up after the counter finishes
        /// </summary>
        public override void OnCounterEnd(GameObject target) { }
    }
}
