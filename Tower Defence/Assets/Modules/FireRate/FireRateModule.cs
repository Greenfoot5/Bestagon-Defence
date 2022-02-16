using System;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Increases the fire rate of a turret
    /// </summary>
    [CreateAssetMenu(fileName = "FireRateModule", menuName = "Modules/Fire Rate")]
    public class FireRateModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };

        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the fire rate of a turret
        /// </summary>
        /// <param name="turret">The turret to increase the fire rate for</param>
        public override void AddModule(Turret turret)
        {
            if (turret.GetType() != typeof(Gunner))
            {
                turret.fireRate.AddModifier(percentageChange);
            }
            else
            {
                var gunner = (Gunner)turret;
                gunner.maxFireRate.AddModifier(percentageChange);
                gunner.spinCooldown.AddModifier(percentageChange);
                gunner.spinMultiplier.AddModifier(percentageChange);
            }
        }
        
        /// <summary>
        /// Removes the fire rate increase
        /// </summary>
        /// <param name="turret">The turret to remove the fire rate for</param>
        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(percentageChange);
        }
    }
}
