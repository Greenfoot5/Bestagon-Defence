using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Upgrades/RangeUpgrade")]
    public class RangeUpgrade : Upgrade
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        public override void AddUpgrade(Turret turret)
        {
            turret.range.AddModifier(percentageChange);
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate.TakeModifier(percentageChange);
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
