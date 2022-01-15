using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "TrackerUpgradeT0", menuName = "Upgrades/Tracker")]
    public class TrackerUpgrade : Upgrade
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [SerializeField]    
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
