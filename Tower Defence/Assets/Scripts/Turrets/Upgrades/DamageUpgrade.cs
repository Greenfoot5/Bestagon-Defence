using System;
using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "DamageT0", menuName = "Upgrades/Damage")]
    public class DamageUpgrade : Upgrade
    {
        public override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float percentageIncrease;
        public override void AddUpgrade(Turret turret)
        {
            turret.damage += percentageIncrease * turret.damage;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.damage -= percentageIncrease * turret.damage;
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.damage += (percentageIncrease * bullet.damage);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}