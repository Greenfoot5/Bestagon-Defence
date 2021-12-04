using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "FasterShotsT0", menuName = "Upgrades/FasterShots")]
    public class FasterShots : Upgrade
    {
        public override Type[] ValidTypes => new[] { typeof(Shooter) };

        [SerializeField]
        private float percentageChange;
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(percentageChange);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}