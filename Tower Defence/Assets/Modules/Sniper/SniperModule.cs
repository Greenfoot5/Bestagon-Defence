using System;
using Turrets;
using Turrets.Laser;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.Sniper
{
    /// <summary>
    /// Extends the Module class to create a Sniper upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "SniperT0", menuName = "Modules/Sniper")]
    public class SniperModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser), typeof(Smasher) };  // any

        [Header("Shooter Turret")]
        [Tooltip("The percentage to modify the range of the Shooter by")]
        [SerializeField]
        private float shooterRangePercentageChange;
        [Tooltip("The percentage to modify the damage of the Shooter by")]
        [SerializeField]
        private float shooterDamagePercentageChange;
        [Tooltip("The percentage to modify the fire rate of the Shooter by")]
        [SerializeField]
        private float shooterFireRatePercentageChange;
        [Tooltip("The percentage to modify the rotation speed of the shooter by")]
        [SerializeField]
        private float shooterRotationSpeedPercentageChange;
        [Tooltip("The percentage to modify the speed of the bullets by")]
        [SerializeField]
        private float shooterBulletSpeedPercentageChange;

        [Header("Laser Turret")]
        [Tooltip("The percentage to modify the range of the Laser by")]
        [SerializeField]
        private float laserRangePercentageChange;
        [Tooltip("The percentage to modify the rotation speed of the Laser by")]
        [SerializeField]
        private float laserRotationSpeedPercentageChange;
        [Tooltip("The percentage to modify the damage of the Laser by")]
        [SerializeField]
        private float laserDamagePercentageChange;

        [Header("Smasher Turret")]
        [Tooltip("The percentage to modify the range of the Smasher by")]
        [SerializeField]
        private float smasherRangePercentageChange;
        [Tooltip("The percentage to modify the damage of the Smasher by")]
        [SerializeField]
        private float smasherDamagePercentageChange;
        [Tooltip("The percentage to modify the fire rate of the Smasher by")]
        [SerializeField]
        private float smasherFireRatePercentageChange;
        
        /// <summary>
        /// Modifies a turret's stats
        /// </summary>
        /// <param name="turret">The turret's stats to modify</param>
        /// <exception cref="ArgumentOutOfRangeException">An invalid turret is being modified</exception>
        public override void AddModule(Turret turret)
        {
            switch (turret)
            {
                // Modify the Shooter stats
                case Shooter shooter:
                    turret.damage.AddModifier(shooterDamagePercentageChange);
                    turret.range.AddModifier(shooterRangePercentageChange);
                    turret.fireRate.AddModifier(shooterFireRatePercentageChange);
                    shooter.rotationSpeed.AddModifier(shooterRotationSpeedPercentageChange);
                    break;
                // Modify the Laser's stats
                case Laser laser:
                    turret.range.AddModifier(laserRangePercentageChange);
                    laser.rotationSpeed.AddModifier(laserRotationSpeedPercentageChange);
                    turret.damage.AddModifier(laserDamagePercentageChange);
                    break;
                // Modify the Smasher's stats
                case Smasher _:
                    turret.range.AddModifier(smasherRangePercentageChange);
                    turret.damage.AddModifier(smasherDamagePercentageChange);
                    turret.fireRate.AddModifier(smasherFireRatePercentageChange);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Removes any stats modifications from the module
        /// </summary>
        /// <param name="turret">The turret to remove the modifications from</param>
        /// <exception cref="ArgumentOutOfRangeException">An invalid turret</exception>
        public override void RemoveModule(Turret turret)
        {
            switch (turret)
            {
                // Modify the Shooter's stats
                case Shooter shooter:
                    turret.damage.TakeModifier(shooterDamagePercentageChange);
                    turret.range.TakeModifier(shooterRangePercentageChange);
                    turret.fireRate.TakeModifier(shooterFireRatePercentageChange);
                    shooter.rotationSpeed.TakeModifier(shooterRotationSpeedPercentageChange);
                    break;
                // Modify the Laser's stats
                case Laser laser:
                    turret.range.TakeModifier(laserRangePercentageChange);
                    laser.rotationSpeed.TakeModifier(laserRotationSpeedPercentageChange);
                    turret.damage.TakeModifier(laserDamagePercentageChange);
                    break;
                // Modify the Smasher's stats
                case Smasher _:
                    turret.range.TakeModifier(smasherRangePercentageChange);
                    turret.damage.TakeModifier(smasherDamagePercentageChange);
                    turret.fireRate.TakeModifier(smasherFireRatePercentageChange);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Applies stat modifications when the bullet when fired
        /// </summary>
        /// <param name="bullet">The bullet to modify</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.damage.AddModifier(shooterDamagePercentageChange);
            bullet.speed.AddModifier(shooterBulletSpeedPercentageChange);
        }
    }
}