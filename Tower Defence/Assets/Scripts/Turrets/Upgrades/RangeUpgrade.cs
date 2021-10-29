using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Upgrades/RangeUpgrade")]
    public class RangeUpgrade : Upgrade
    {
        [SerializeField]
        private float percentageIncrease;
        public override void AddUpgrade(Turret turret)
        {
            turret.range *= 1 + percentageIncrease;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 + percentageIncrease;
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
