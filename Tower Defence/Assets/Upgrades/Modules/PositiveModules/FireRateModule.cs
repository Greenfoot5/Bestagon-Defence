using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "FireRateModule", menuName = "Modules/Fire Rate")]
    public class FireRateModule : Module
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(percentageChange);
        }

        public override void RemoveModule(Turret turret)
        {
            turret.fireRate.TakeModifier(percentageChange);
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
