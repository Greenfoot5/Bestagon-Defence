using System;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Extends the Module class to create a Damage upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "DamageT0", menuName = "Modules/Damage")]
    public class DamageModule : Module
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the damage for a turret
        /// </summary>
        /// <param name="turret">The turret to increase damage for</param>
        public override void AddModule(Turret turret)
        {
            turret.damage.AddModifier(percentageChange);
        }
        
        /// <summary>
        /// Removes a damage upgrade for a turret
        /// </summary>
        /// <param name="turret">The turret to remove a damage upgrade for</param>
        public override void RemoveModule(Turret turret)
        {
            turret.damage.TakeModifier(percentageChange);
        }
        
        /// <summary>
        /// Increases a bullet's damage
        /// </summary>
        /// <param name="bullet">The bullet to increase damage for</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.damage.AddModifier(percentageChange);
        }
    }
}