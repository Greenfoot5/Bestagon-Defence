using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/MissileBullet")]
    public class MissileBullet : Upgrade
    {
        public override Type[] ValidTypes => new[] { typeof(Shooter) };

        [FormerlySerializedAs("explosionRadius")] public float explosionRadiusChange;
        [FormerlySerializedAs("damageIncrease")] public float damagePercentageChange;
        [FormerlySerializedAs("fireRateDecrease")] public float fireRatePercentageChange;
        [FormerlySerializedAs("speedIncrease")] public float speedPercentageChange;

        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate.TakeModifier(fireRatePercentageChange);
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