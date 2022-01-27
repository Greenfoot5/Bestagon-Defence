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
        protected override Type[] ValidTypes => new[] { typeof(Shooter) };

        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the speed of the bullet once fired
        /// </summary>
        /// <param name="bullet">The bullet to accelerate</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(percentageChange);
        }
    }
}