using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRateUpgrade")]
    public class FireRateUpgrade : Upgrade
    {
        [SerializeField]
        private float percentageDecrease;
        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate *= 1 - percentageDecrease;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 - percentageDecrease;
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
