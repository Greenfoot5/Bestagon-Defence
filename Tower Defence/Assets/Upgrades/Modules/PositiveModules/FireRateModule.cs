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
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the fire rate of a turret
        /// </summary>
        /// <param name="turret">The turret to increase the fire rate for</param>
        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(percentageChange);
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
