using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Turrets;
using UnityEngine;

namespace Modules
{
    /// <summary>
    /// A base abstract class to create a turret module from.
    /// </summary>
    public abstract class Module : ScriptableObject
    {
        [Tooltip("The tier number of the module")]
        public int moduleTier;
        [Tooltip("If the module can be obtained through upgrades")]
        public bool isUpgradableTo = true;

        protected abstract Type[] ValidTypes { get; }

        /// <summary>
        /// Checks if a turret is of a valid type for the module to work
        /// </summary>
        /// <param name="turret">The turret to check</param>
        /// <returns>If the turret is in the valid types</returns>
        public virtual bool ValidModule(Turret turret)
        {
            return ValidTypes == null || ValidTypes.Contains(turret.GetType());
        }
        
        /// <summary>
        /// Gets all the turrets that the module can be applied to
        /// </summary>
        /// <returns>A list of valid turret types</returns>
        public Type[] GetValidTypes()
        {
            return ValidTypes;
        }
        
        /// <summary>
        /// Called when a module is added to a turret
        /// </summary>
        /// <param name="turret">The turret to modify</param>
        public virtual void AddModule(Turret turret) { }
        
        /// <summary>
        /// Called when a module is removed from a turret
        /// </summary>
        /// <param name="turret">The turret to modify</param>
        public virtual void RemoveModule(Turret turret) { }
        
        /// <summary>
        /// Called when a turret makes an attack
        /// </summary>
        /// <param name="turret">The turret that attacked</param>
        public virtual void OnAttack(Turret turret) { }

        /// <summary>
        /// Called when a turret that fires bullets shoots
        /// </summary>
        /// <param name="bullet">The bullet to modify</param>
        public virtual void OnShoot(Bullet bullet) { }

        /// <summary>
        /// Called when a turret its an enemy
        /// </summary>
        /// <param name="targets">The enemy/ies to apply effect to</param>
        /// <param name="turret">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet that hit the enemies</param>
        public virtual void OnHit(IEnumerable<Enemy> targets, Turret turret, Bullet bullet = null) { }
    }
}