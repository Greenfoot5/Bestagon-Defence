using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "SniperT0", menuName = "Modules/Sniper")]
    public class SniperModule : Module
    {
        protected override Type[] ValidTypes => null;  // any

        [Header("Shooter Turret")]
        public float shooterRangePercentageChange;
        public float shooterDamagePercentageChange;
        public float shooterFireRatePercentageChange;
        public float shooterRotationSpeedPercentageChange;
        public float shooterBulletSpeedPercentageChange;

        [Header("Laser Turret")]
        public float laserRangePercentageChange;
        public float laserRotationSpeedPercentageChange;
        public float laserDamagePercentageChange;

        [Header("Smasher Turret")]
        public float smasherRangePercentageChange;
        public float smasherDamagePercentageChange;
        public float smasherFireRatePercentageChange;
        
        public override void AddModule(Turret turret)
        {
            switch (turret)
            {
                case Shooter shooter:
                    turret.damage.AddModifier(shooterDamagePercentageChange);
                    turret.range.AddModifier(shooterRangePercentageChange);
                    turret.fireRate.AddModifier(shooterFireRatePercentageChange);
                    shooter.rotationSpeed.AddModifier(shooterRotationSpeedPercentageChange);
                    break;
                case Laser laser:
                    turret.range.AddModifier(laserRangePercentageChange);
                    laser.rotationSpeed.AddModifier(laserRotationSpeedPercentageChange);
                    turret.damage.AddModifier(laserDamagePercentageChange);
                    break;
                case Smasher _:
                    turret.range.AddModifier(smasherRangePercentageChange);
                    turret.damage.AddModifier(smasherDamagePercentageChange);
                    turret.fireRate.AddModifier(smasherFireRatePercentageChange);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void RemoveModule(Turret turret)
        {
            switch (turret)
            {
                case Shooter shooter:
                    turret.damage.TakeModifier(shooterDamagePercentageChange);
                    turret.range.TakeModifier(shooterRangePercentageChange);
                    turret.fireRate.TakeModifier(shooterFireRatePercentageChange);
                    shooter.rotationSpeed.TakeModifier(shooterRotationSpeedPercentageChange);
                    break;
                case Laser laser:
                    turret.range.TakeModifier(laserRangePercentageChange);
                    laser.rotationSpeed.TakeModifier(laserRotationSpeedPercentageChange);
                    turret.damage.TakeModifier(laserDamagePercentageChange);
                    break;
                case Smasher _:
                    turret.range.TakeModifier(smasherRangePercentageChange);
                    turret.damage.TakeModifier(smasherDamagePercentageChange);
                    turret.fireRate.TakeModifier(smasherFireRatePercentageChange);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.damage.AddModifier(shooterDamagePercentageChange);
            bullet.speed.AddModifier(shooterBulletSpeedPercentageChange);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}