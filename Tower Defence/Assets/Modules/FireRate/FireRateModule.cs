using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.FireRate
{
    /// <summary>
    /// Increases the fire rate of a turret
    /// </summary>
    [CreateAssetMenu(fileName = "FireRateT0", menuName = "ModuleTiers/Fire Rate")]
    public class FireRateModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };
        
        [Tooltip("What percentage to modify the fire rate of the turret by.\n\n" +
                 "If it's a gunner turret, the percentage to modify the fire rate cap, spin cooldown and spin multiplier")]
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
