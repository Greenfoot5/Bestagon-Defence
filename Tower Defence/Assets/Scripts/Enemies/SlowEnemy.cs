using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "SlowSelfHeal", menuName = "EnemyAbilities/SlowSelf")]
    public class SlowEnemy : EnemyAbility
    {
        [Header("Ability Stats")]
        //public bool isPermanent = true;
        public float slowPercentage = 0.2f;
        //public float slow duration;

        public override void Activate(GameObject target)
        {
            // Check we have an enemy to heal
            var enemyComponent = target.GetComponent<Enemy>();
            if (enemyComponent == null)
            {
                Debug.Log("Returned False");
                return;
            }
            
            // Don't slow if there's already a greater slow, or one of the same value
            Debug.Log($"{enemyComponent.startSpeed} * (1 - {slowPercentage}) = {enemyComponent.startSpeed * (1 - slowPercentage)} < {enemyComponent.speed}");
            if (!(enemyComponent.startSpeed * (1 - slowPercentage) < enemyComponent.speed))
            {
                Debug.Log("Already Slowed");
                return;
            }
            
            Debug.Log("Actually Slowed");
            enemyComponent.speed *= 1 - slowPercentage;
            return;

        }

        public override void OnCounterEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}