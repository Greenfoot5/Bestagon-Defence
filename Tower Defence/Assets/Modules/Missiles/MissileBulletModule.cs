using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using UnityEngine;

namespace Modules.Missiles
{
    /// <summary>
    /// Extends the Module class to create a MissileBulletModule upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Modules/Missile Bullet")]
    public class MissileBulletModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner) };
        
        [Tooltip("What percentage to modify the explosion radius of the bullet by")]
        [SerializeField]
        private float explosionRadiusChange;
        [Tooltip("The percentage to modify the damage of the turret by")]
        [SerializeField]
        private float damagePercentageChange;
        [Tooltip("The percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRatePercentageChange;
        [Tooltip("The percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float speedPercentageChange;
        
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
            bullet.explosionRadius.AddModifier(explosionRadiusChange);
            bullet.damage.AddModifier(damagePercentageChange);
            bullet.speed.AddModifier(speedPercentageChange);
        }
    }
}