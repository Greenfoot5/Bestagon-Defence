using System;
using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "BombBulletT0", menuName = "Upgrades/BombBullet")]
    public class BombBullet : Upgrade
    {
        public override Type[] ValidTypes => new Type[] { typeof(Shooter) };

        public float explosionRadius;
        public float damageIncrease;
        public float fireRateDecrease;
        public float rangeDecrease;
        public float speedDecrease;

        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate *= fireRateDecrease;
            turret.range *= rangeDecrease;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= fireRateDecrease;
            turret.range /= rangeDecrease;
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius += explosionRadius;
            bullet.damage *= 1 + damageIncrease;
            bullet.speed *= speedDecrease;
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}