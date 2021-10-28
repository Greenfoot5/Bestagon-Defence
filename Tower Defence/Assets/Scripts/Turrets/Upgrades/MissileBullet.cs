using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/MissileBullet")]
    public class MissileBullet : Upgrade
    {
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {;
            bullet.explosionRadius += GETUpgradeValue();
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}