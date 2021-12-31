using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "PoisonEnemy", menuName = "Enemy Abilities/Poison Enemy")]
    public class PoisonEnemy : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("Static damage amount that's removed every tick, ticks being the timer")]
        public float damage;
    
        public override void Activate(GameObject target)
        {
            if (target == null)
            {
                return;
            }
            
            // Check we have an enemy to heal
            var enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent == null)
            {
                return;
            }

            enemyComponent.health -= damage;
        }

        public override void OnCounterEnd(GameObject target) { }
    }
}
