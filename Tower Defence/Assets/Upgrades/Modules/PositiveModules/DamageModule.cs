using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "DamageT0", menuName = "Modules/Damage")]
    public class DamageModule : Module
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        public override void AddModule(Turret turret)
        {
            turret.damage.AddModifier(percentageChange);
        }

        public override void RemoveModule(Turret turret)
        {
            turret.damage.TakeModifier(percentageChange);
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.damage.AddModifier(percentageChange);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}