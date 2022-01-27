using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Extends the Module class to create a MissileBulletModule upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Modules/Missile Bullet")]
    public class MissileBulletModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter) };

        public float explosionRadiusChange;
        public float damagePercentageChange;
        public float fireRatePercentageChange;
        public float speedPercentageChange;
        
        /// <summary>
        /// Applies the stat changes to the turret
        /// </summary>
        /// <param name="turret">The turret to modifies the stats for</param>
        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
        }
        
        /// <summary>
        /// Removes the stats changes from a turret
        /// </summary>
        /// <param name="turret">The turret to remove the stats changes for</param>
        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(fireRatePercentageChange);
        }
        
        /// <summary>
        /// Applies stat changes to a bullet
        /// </summary>
        /// <param name="bullet">The bullet to modify</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius += explosionRadiusChange;
            bullet.damage.AddModifier(damagePercentageChange);
            bullet.speed.AddModifier(speedPercentageChange);
        }
    }
}