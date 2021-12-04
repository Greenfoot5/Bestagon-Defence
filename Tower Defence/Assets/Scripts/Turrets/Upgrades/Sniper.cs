using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "SniperT0", menuName = "Upgrades/Sniper")]
    public class Sniper : Upgrade
    {
        public override Type[] ValidTypes => null;  // any

        [FormerlySerializedAs("bulletRange")] [Header("Shooter Turret")]
        public float shooterRangePercentageChange;
        [FormerlySerializedAs("bulletDamage")] public float shooterDamagePercentageChange;
        [FormerlySerializedAs("bulletFireRate")] public float shooterFireRatePercentageChange;
        [FormerlySerializedAs("bulletTurnSpeed")] public float shooterRotationSpeedPercentageChange;
        [FormerlySerializedAs("bulletSpeed")] public float shooterBulletSpeedPercentageChange;

        [FormerlySerializedAs("laserRange")] [Header("Laser Turret")]
        public float laserRangePercentageChange;
        [FormerlySerializedAs("laserTurnSpeed")] public float laserRotationSpeedPercentageChange;
        [FormerlySerializedAs("laserDamage")] public float laserDamagePercentageChange;

        [FormerlySerializedAs("areaRange")] [Header("Smasher Turret")]
        public float smasherRangePercentageChange;
        [FormerlySerializedAs("areaDamage")] public float smasherDamagePercentageChange;
        [FormerlySerializedAs("smahsherFireRatePercentageChange")] [FormerlySerializedAs("areaFireRate")]
        public float smasherFireRatePercentageChange;
        
        public override void AddUpgrade(Turret turret)
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

        public override void RemoveUpgrade(Turret turret)
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