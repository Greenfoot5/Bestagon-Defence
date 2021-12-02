using System;
using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/MissileBullet")]
    public class MissileBullet : Upgrade
    {
        public override Type[] ValidTypes => new[] { typeof(Shooter) };

        public float explosionRadius;
        public float damageIncrease;
        public float fireRateDecrease;
        public float speedIncrease;

        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate *= fireRateDecrease;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= fireRateDecrease;
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius += explosionRadius;
            bullet.damage *= 1 + damageIncrease;
            bullet.speed *= 1 + speedIncrease;
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}