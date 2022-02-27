using System;
using System.Collections.Generic;
using Modules;
using Turrets;
using UnityEngine;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Applies a module to turret(s)
    /// </summary>
    [CreateAssetMenu(fileName = "DebuffTurret", menuName = "Enemy Abilities/Debuff Turret")]
    public class DebuffTurret : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("The list of debuffs to apply to the turret")]
        [SerializeField]
        private List<Module> debuffs;
        
        
        /// <summary>
        /// Applies a module to a turret
        /// </summary>
        /// <param name="target">The turret to debuff</param>
        public override void Activate(GameObject target)
        {
            // Check there is a turret to downgrade
            var turretComponent = target.GetComponent<Turret>();
            if (turretComponent == null)
            {
                return;
            }
            
            // Add the debuffs
            foreach (var module in debuffs)
            {
                turretComponent.AddModule(module);
            }
        }
        
        /// <summary>
        /// Removes the debuff from a turret
        /// </summary>
        /// <param name="target">The turret to remove the debuff for</param>
        /// <exception cref="NotImplementedException">The function isn't implemented yet</exception>
        public override void OnCounterEnd(GameObject target)
        {
            throw new NotImplementedException();
        }
    }
}