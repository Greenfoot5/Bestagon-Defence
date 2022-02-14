using System;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Increases the speed of bullets
    /// </summary>
    [CreateAssetMenu(fileName = "FasterShotsT0", menuName = "Modules/Faster Shots")]
    public class FasterShotsModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner) };

        [SerializeField]
        private float bulletSpeedChange;
        [SerializeField]
        private float rangeChange;
        
        /// <summary>
        /// Increases the range of a turret
        /// </summary>
        /// <param name="turret">The turret to apply the modifications to</param>
        public override void AddModule(Turret turret)
        {
            turret.range.AddModifier(rangeChange);
        }

        public override void RemoveModule(Turret turret)
        {
            turret.range.TakeModifier(rangeChange);
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