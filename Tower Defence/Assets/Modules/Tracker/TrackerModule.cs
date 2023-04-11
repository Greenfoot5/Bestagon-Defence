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
        [Tooltip("The percentage to modify the rotation speed of the turret by")]
        private float rotationSpeedPercentageChange;
        [SerializeField]
        [Tooltip("The percentage to modify the damage of the turret by")]
        private float damagePercentageChange;

        [Header("Gunner")]
        [SerializeField]
        [Tooltip("The percentage to modify the fire rate cap by")]
        private float gunnerFireRateCapChange;
        
        /// <summary>
        /// Increases the rotation speed of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void AddModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationFrequency.AddModifier(rotationSpeedPercentageChange);
            turret.damage.AddModifier(damagePercentageChange);
            if (turret.GetType() == typeof(Gunner))
                ((Gunner)turret).maxFireRate.AddModifier(gunnerFireRateCapChange);
        }
        
        /// <summary>
        /// Removes the rotation speed increase of a turret
        /// </summary>
        /// <param name="turret">The turret to affect</param>
        public override void RemoveModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationFrequency.TakeModifier(rotationSpeedPercentageChange);
            turret.damage.TakeModifier(damagePercentageChange);
            if (turret.GetType() == typeof(Gunner))
                ((Gunner)turret).maxFireRate.TakeModifier(gunnerFireRateCapChange);
        }
    }
}
