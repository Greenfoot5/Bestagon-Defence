using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "BurnEnemy", menuName = "Enemy Abilities/Burn Enemy")]
    public class BurnEnemy : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("A percentage of the enemy's max health to take per tick")]
        [Range(0, 1)]
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

            enemyComponent.health -= enemyComponent.maxHealth * damage;
        }

        public override void OnCounterEnd(GameObject target) { }
    }
}