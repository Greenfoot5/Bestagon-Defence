using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "SlowEnemy", menuName = "EnemyAbilities/SlowSelf")]
    public class SlowEnemy : EnemyAbility
    {
        [Header("Ability Stats")]
        //public bool isPermanent = true;
        public float slowPercentage = 0.2f;
        //public float slow duration;

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
            
            // Don't slow if there's already a greater slow, or one of the same value
            if (!(enemyComponent.startSpeed * (1 - slowPercentage) < enemyComponent.speed))
            {
                return;
            }
            
            enemyComponent.speed *= 1f - slowPercentage;
        }

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