using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "DamageT0", menuName = "Upgrades/Damage")]
    public class DamageUpgrade : Upgrade
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageChange;
        public override void AddUpgrade(Turret turret)
        {
            turret.damage.AddModifier(percentageChange);
        }

        public override void RemoveUpgrade(Turret turret)
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