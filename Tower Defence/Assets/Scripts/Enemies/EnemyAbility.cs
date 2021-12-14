using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAbility : ScriptableObject
    {
        public AbilityTarget targetingType = AbilityTarget.Single;
        public AbilityTrigger[] triggers;
        
        [Header("Radius Stats")]
        public float range = 3f;

        [Header("Timer Stats")]
        public float timer = 5f;
        
        public abstract void Activate(GameObject target);
    }
}
