using UnityEngine;

namespace Upgrades.Abilities
{
    /// <summary>
    /// The base for any enemy abilitiy
    /// </summary>
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
        public float startCount = -1f;
        
        /// <summary>
        /// Activates the ability
        /// </summary>
        /// <param name="target">What the ability is activated on</param>
        public abstract void Activate(GameObject target);
        
        /// <summary>
        /// Ends the ability when the timer ends
        /// </summary>
        /// <param name="target">What to clean up the ability on</param>
        public abstract void OnCounterEnd(GameObject target);
    }
}
