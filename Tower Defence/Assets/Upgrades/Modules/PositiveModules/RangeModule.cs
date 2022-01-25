using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "RangeModule", menuName = "Modules/Range")]
    public class RangeModule : Module
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        public override void AddModule(Turret turret)
        {
            turret.range.AddModifier(percentageChange);
        }

        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(percentageChange);
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
