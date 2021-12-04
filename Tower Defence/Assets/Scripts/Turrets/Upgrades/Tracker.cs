using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "TrackerUpgradeT0", menuName = "Upgrades/Tracker")]
    public class Tracker : Upgrade
    {
        public override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [FormerlySerializedAs("rotationSpeedPercenageChange")] [FormerlySerializedAs("turnSpeed")] [SerializeField]    
        private float rotationSpeedPercentageChange;
        public override void AddUpgrade(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.AddModifier(rotationSpeedPercentageChange);
        }

        public override void RemoveUpgrade(Turret turret)
        {
            ((DynamicTurret)turret).rotationSpeed.TakeModifier(rotationSpeedPercentageChange);
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
