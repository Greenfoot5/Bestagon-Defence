using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "FasterShotsT0", menuName = "Upgrades/FasterShots")]
    public class FasterShots : Upgrade
    {
        [SerializeField]
        private float percentageIncrease;
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {
            bullet.speed += (percentageIncrease * bullet.speed);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}