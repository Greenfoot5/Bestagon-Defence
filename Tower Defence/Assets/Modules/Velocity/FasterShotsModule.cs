using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using UnityEngine;

namespace Modules.Velocity
{
    /// <summary>
    /// Increases the speed of bullets
    /// </summary>
    [CreateAssetMenu(fileName = "VelocityT0", menuName = "Modules/Velocity")]
    public class FasterShotsModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner) };
        
        [Tooltip("The percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float bulletSpeedChange;
        [Tooltip("The percentage to modify the range of the bullet by")]
        [SerializeField]
        private float rangeChange;
        [Tooltip("The percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRateChange;
        
        /// <summary>
        /// Increases the range of a turret
        /// </summary>
        /// <param name="turret">The turret to apply the modifications to</param>
        public override void AddModule(Turret turret)
        {
            turret.range.AddModifier(rangeChange);
            turret.fireRate.AddModifier(fireRateChange);
        }

        public override void RemoveModule(Turret turret)
        {
            turret.range.TakeModifier(rangeChange);
            turret.fireRate.TakeModifier(fireRateChange);
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