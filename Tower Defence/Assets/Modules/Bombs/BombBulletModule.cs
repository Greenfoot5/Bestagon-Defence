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
        
        [Tooltip("What percentage to modify the explosion radius of bullets by")]
        [SerializeField]
        private float explosionRadiusChange;
        [Tooltip("What percentage to modify the damage of the turret by")]
        [SerializeField]
        private float damagePercentageChange;
        [Tooltip("What percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRatePercentageChange;
        [Tooltip("What percentage to modify the range of the turret by")]
        [SerializeField]
        private float rangePercentageChange;
        [Tooltip("What percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float speedPercentageChange;
        
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