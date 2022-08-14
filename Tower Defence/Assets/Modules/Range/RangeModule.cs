using System;
using Turrets;
using Turrets.Lancer;
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
        
        [Tooltip("The percentage to modify the range of the turret by")]
        [SerializeField]
        private float percentageChange;
        
        /// <summary>
        /// Increases the range of a turret
        /// </summary>
        /// <param name="turret">The turret to increase range for</param>
        public override void AddModule(Turret turret)
        {
            turret.range.AddModifier(percentageChange);
            if (turret.GetType() == typeof(Lancer))
            {
                ((Lancer) turret).bulletRange.AddModifier(percentageChange);
            }
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
