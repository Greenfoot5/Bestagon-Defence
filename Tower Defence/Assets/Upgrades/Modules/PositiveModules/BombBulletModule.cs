using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "BombBulletT0", menuName = "Modules/Bomb Bullet")]
    public class BombBulletModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter) };

        public float explosionRadiusChange;
        public float damagePercentageChange;
        public float fireRatePercentageChange;
        public float rangePercentageChange;
        public float speedPercentageChange;

        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
            turret.range.AddModifier(rangePercentageChange);
        }

        public override void RemoveModule(Turret turret)
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