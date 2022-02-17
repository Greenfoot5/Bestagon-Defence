using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Laser;
using Turrets.Shooter;
using UnityEngine;

namespace Modules.Tracker
{
    /// <summary>
    /// Increases the rotation speed of a dynamic turret
    /// </summary>
    [CreateAssetMenu(fileName = "TrackerModuleT0", menuName = "Modules/Tracker")]
    public class TrackerModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser), typeof(Gunner) };

        [SerializeField]    
        private float rotationSpeedPercentageChange;
        [SerializeField]
        private float damagePercentageChange;
        
        /// <summary>
        /// Increases the rotation speed of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void AddModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.AddModifier(rotationSpeedPercentageChange);
            turret.damage.AddModifier(damagePercentageChange);
        }
        
        /// <summary>
        /// Removes the rotation speed increase of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void RemoveModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.TakeModifier(rotationSpeedPercentageChange);
            turret.damage.TakeModifier(damagePercentageChange);
        }
    }
}
