using System.Collections;
using Abstract;
using Enemies;
using UnityEngine;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    [CreateAssetMenu(fileName = "SugarRush", menuName = "Enemy Abilities/Sugar Rush")]
    public class SugarRushEnemyAbility : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("The multiplier to the speed")]
        [SerializeField]
        private float speedMultiplier = 1.5f;
        [Tooltip("How long the effect lasts")]
        [SerializeField]
        private float duration = 1f;

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

            Runner.Run(EatSugar(enemyComponent));

        }

        public override void OnCounterEnd(GameObject target) { }

        /// <summary>
        /// There's nothing to clear up after the counter finishes
        /// </summary>
        private IEnumerator EatSugar(Enemy enemyComponent)
        {
            enemyComponent.speed.MultiplyModifier(speedMultiplier);
            yield return new WaitForSeconds(duration);
            enemyComponent.speed.DivideModifier(speedMultiplier);
        }
    }
}
