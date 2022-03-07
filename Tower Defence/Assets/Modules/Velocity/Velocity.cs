using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.Velocity
{
    /// <summary>
    /// Increases the speed of bullets
    /// </summary>
    [CreateAssetMenu(fileName = "VelocityT0", menuName = "Modules/Velocity")]
    public class Velocity : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Smasher) };
        
        [Tooltip("The percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float bulletSpeedChange;
        [Tooltip("The percentage to modify the range of the bullet by")]
        [SerializeField]
        private float rangeChange;
        [Tooltip("The percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRateChange;

        [Header("Smasher")]
        [SerializeField]
        [Tooltip("The percentage to modify the damage of the smasher by")]
        private float smasherDamageChange;
        [SerializeField]
        [Tooltip("The percentage to modify the range of the smasher by")]
        private float smasherRangeChange;
        [SerializeField]
        [Tooltip("The percentage to modify the fire rate of the smasher by")]
        private float smasherFireRateChange;
        
        /// <summary>
        /// Increases the range of a turret
        /// </summary>
        /// <param name="turret">The turret to apply the modifications to</param>
        public override void AddModule(Turret turret)
        {
            if (turret.GetType() == typeof(Shooter) || turret.GetType() == typeof(Gunner))
            {
                turret.range.AddModifier(rangeChange);
                turret.fireRate.AddModifier(fireRateChange);
            }
            else
            {
                turret.damage.AddModifier(smasherDamageChange);
                turret.range.AddModifier(smasherRangeChange);
                turret.fireRate.AddModifier(smasherFireRateChange);
            }
        }

        public override void RemoveModule(Turret turret)
        {
            if (turret.GetType() == typeof(Shooter) || turret.GetType() == typeof(Gunner))
            {
                turret.range.TakeModifier(rangeChange);
                turret.fireRate.TakeModifier(fireRateChange);
            }
            else
            {
                turret.damage.TakeModifier(smasherDamageChange);
                turret.range.TakeModifier(smasherRangeChange);
                turret.fireRate.TakeModifier(smasherFireRateChange);
            }
        }
        
        /// <summary>
        /// Increases the speed of the bullet once fired
        /// </summary>
        /// <param name="bullet">The bullet to accelerate</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(bulletSpeedChange);
        }
    }
}