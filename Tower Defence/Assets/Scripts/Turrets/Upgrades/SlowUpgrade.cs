using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Upgrades/SlowsEnemyUpgrade")]
    public class SlowUpgrade : Upgrade
    {
        [SerializeField]
        private float percentageSlow;
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
                target.Slow(percentageSlow);
            }
        }
    }
}