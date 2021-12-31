using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "StunEnemy", menuName = "Enemy Abilities/Stun Enemy")]
    public class StunEnemyEffect : EnemyAbility
    {
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

            enemyComponent.speed -= enemyComponent.startSpeed * 5000f;
        }

        public override void OnCounterEnd(GameObject target)
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

            enemyComponent.speed += enemyComponent.startSpeed * 5000f;
        }
    }
}