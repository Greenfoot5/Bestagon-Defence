using UnityEngine;

namespace Enemies
{
    public abstract class EnemyAbility : ScriptableObject
    {
        public AbilityTarget targetingType = AbilityTarget.Single;
        public AbilityTrigger[] triggers;

        public Sprite abilityIcon;
        public GameObject abilityEffect;
        
        [Header("Radius Stats")]
        public float range = 3f;

        [Header("Timer Stats")]
        public float timer = 5f;

        [Header("Counter")]
        [Tooltip("Set as -1 to not use timer")]
        public int startCount = -1;
        
        public abstract void Activate(GameObject target);
        public abstract void OnCounterEnd(GameObject target);
    }
}
