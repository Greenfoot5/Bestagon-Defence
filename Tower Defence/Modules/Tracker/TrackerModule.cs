using System;
using Abstract.Attributes;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Laser;
using Turrets.Shooter;


namespace Modules.Tracker
{
    /// <summary>
    /// Increases the rotation speed of a dynamic turret
    /// </summary>
    public partial class TrackerModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser), typeof(Gunner), typeof(Choker) };
        
        /// <summary>
        /// The percentage to modify the rotation speed of the turret by
        /// </summary>
        [Export]
        private AttributeModifier _rotationSpeedPercentageChange = new();
        /// <summary>
        /// The percentage to modify the damage of the turret by
        /// </summary>
        [Export]
        private AttributeModifier _damagePercentageChange = new();
        
        /// <summary>
        /// Increases the rotation speed of a turret
        /// </summary>
        /// <param name="damager">The turret to affect</param>
        public override void AddModule(Damager damager)
        {
            damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _damagePercentageChange);
            if (damager is DynamicTurret turret)
                turret.Stats[AttributeType.RotationSpeed].Add(GetSceneUniqueId(), _rotationSpeedPercentageChange);
        }
        
        /// <summary>
        /// Removes the rotation speed increase of a turret
        /// </summary>
        /// <param name="damager">The turret to affect</param>
        public override void RemoveModule(Damager damager)
        {
            damager.Stats[AttributeType.Damage].Remove(GetSceneUniqueId());
            if (damager is DynamicTurret turret)
                turret.Stats[AttributeType.RotationSpeed].Remove(GetSceneUniqueId());
        }
    }
}
