using UnityEngine;

namespace _WIP.Abilities
{
    /// <summary>
    /// The base for any enemy ability
    /// </summary>
    public abstract class EnemyAbility : ScriptableObject
    {
        [Tooltip("What should the ability target")]
        public AbilityTarget targetingType = AbilityTarget.Single;
        [Tooltip("What triggers the ability to activate")]
        public AbilityTrigger[] triggers;
        
        [Tooltip("The icon to place above the enemy's health bar")]
        public Sprite abilityIcon;
        [Tooltip("The particle effect to play when activating the ability")]
        public GameObject abilityEffect;
        
        [Header("Radius Stats")]
        [Tooltip("The range of the ability (if using radius targeting")]
        public float range = 3f;

        [Header("Timer Stats")]
        [Tooltip("The duration of the ability (if applicable)")]
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
