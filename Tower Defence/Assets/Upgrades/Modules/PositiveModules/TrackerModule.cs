using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "TrackerModuleT0", menuName = "Modules/Tracker")]
    public class TrackerModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [SerializeField]    
        private float rotationSpeedPercentageChange;
        public override void AddModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.AddModifier(rotationSpeedPercentageChange);
        }

        public override void RemoveModule(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.TakeModifier(rotationSpeedPercentageChange);
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
