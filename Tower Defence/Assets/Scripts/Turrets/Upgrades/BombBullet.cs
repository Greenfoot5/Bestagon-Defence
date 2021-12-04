using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "BombBulletT0", menuName = "Upgrades/BombBullet")]
    public class BombBullet : Upgrade
    {
        public override Type[] ValidTypes => new[] { typeof(Shooter) };

        [FormerlySerializedAs("explosionRadius")] public float explosionRadiusChange;
        [FormerlySerializedAs("damageIncrease")] public float damagePercentageChange;
        [FormerlySerializedAs("fireRateDecrease")] public float fireRatePercentageChange;
        [FormerlySerializedAs("rangeDecrease")] public float rangePercentageChange;
        [FormerlySerializedAs("speedDecrease")] public float speedPercentageChange;

        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
            turret.range.AddModifier(rangePercentageChange);
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate.TakeModifier(fireRatePercentageChange);
            turret.range.TakeModifier(rangePercentageChange);
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius += explosionRadiusChange;
            bullet.damage.AddModifier(damagePercentageChange);
            bullet.speed.AddModifier(speedPercentageChange);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}