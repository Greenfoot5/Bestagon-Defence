using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Modules/Missile Bullet")]
    public class MissileBulletModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter) };

        public float explosionRadiusChange;
        public float damagePercentageChange;
        public float fireRatePercentageChange;
        public float speedPercentageChange;

        public override void AddModule(Turret turret)
        {
            turret.fireRate.AddModifier(fireRatePercentageChange);
        }

        public override void RemoveModule(Turret turret)
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