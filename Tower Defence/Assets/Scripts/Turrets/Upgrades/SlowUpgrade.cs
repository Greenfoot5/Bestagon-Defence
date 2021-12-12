using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Upgrades/SlowsEnemyUpgrade")]
    public class SlowUpgrade : Upgrade
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private float slowPercentage;
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {
            bullet.AddUpgrade(this);
        }

        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (var target in targets)
            {
                target.Slow(slowPercentage);
            }
        }
    }
}
