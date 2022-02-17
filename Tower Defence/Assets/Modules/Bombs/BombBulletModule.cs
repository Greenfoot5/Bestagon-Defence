using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using UnityEngine;

namespace Modules.Bombs
{
    /// <summary>
    /// Extends the Module class to create a BombBullet upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "BombBulletT0", menuName = "Modules/Bomb Bullet")]
    public class BombBulletModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner)};

        public float explosionRadiusChange;
        public float damagePercentageChange;
        public float fireRatePercentageChange;
        public float rangePercentageChange;
        public float speedPercentageChange;
        
        /// <summary>
        /// Changes the turret's stats when added
        /// </summary>
        /// <param name="turret">The turret to change stats for</param>
        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
            turret.range.AddModifier(rangePercentageChange);
        }
        
        /// <summary>
        /// Removes stat modifications for a turret
        /// </summary>
        /// <param name="turret">The turret to revert stat changes for</param>
        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(fireRatePercentageChange);
            turret.range.TakeModifier(rangePercentageChange);
        }
        
        /// <summary>
        /// Modifies the stats of a bullet when fired
        /// </summary>
        /// <param name="bullet">The bullet to add stats for</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius.AddModifier(explosionRadiusChange);
            bullet.damage.AddModifier(damagePercentageChange);
            bullet.speed.AddModifier(speedPercentageChange);
        }
    }
}