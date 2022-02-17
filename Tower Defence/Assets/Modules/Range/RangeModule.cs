using System;
using Turrets;
using UnityEngine;

namespace Modules.Range
{
    /// <summary>
    /// Increases the range of a turret
    /// </summary>
    [CreateAssetMenu(fileName = "RangeModule", menuName = "Modules/Range")]
    public class RangeModule : Module
    {
        protected override Type[] ValidTypes => null;  // any
        
        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the range of a turret
        /// </summary>
        /// <param name="turret">The turret to increase range for</param>
        public override void AddModule(Turret turret)
        {
            turret.range.AddModifier(percentageChange);
        }
        
        /// <summary>
        /// Removes the range increase from a turret
        /// </summary>
        /// <param name="turret">The turret to decrease the range for</param>
        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(percentageChange);
        }
    }
}
