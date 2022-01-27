using System;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Increases the rotation speed of a dynamic turret
    /// </summary>
    [CreateAssetMenu(fileName = "TrackerModuleT0", menuName = "Modules/Tracker")]
    public class TrackerModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [SerializeField]    
        private float rotationSpeedPercentageChange;
        
        /// <summary>
        /// Increases the rotation speed of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void AddModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.AddModifier(rotationSpeedPercentageChange);
        }
        
        /// <summary>
        /// Removes the rotation speed increase of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void RemoveModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.TakeModifier(rotationSpeedPercentageChange);
        }
    }
}
