using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Bombs
{
    /// <summary>
    /// Extends the Module class to create a BombBullet upgrade
    /// </summary>
    [CreateAssetMenu(fileName = "BombsT0", menuName = "Modules/Bombs")]
    public class BombsModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Smasher), typeof(Lancer)};
        
        [Header("Shooter, Gunner & Lancer")]
        [Tooltip("What percentage to modify the explosion radius of bullets by")]
        [SerializeField]
        private float explosionRadiusChange;
        [Tooltip("What percentage to modify the damage of the turret by")]
        [SerializeField]
        private float damagePercentageChange;
        [Tooltip("What percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRatePercentageChange;
        [Tooltip("What percentage to modify the range of the turret by")]
        [SerializeField]
        private float rangePercentageChange;
        [Tooltip("What percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float speedPercentageChange;
        
        [Header("Lancer Only")]
        [Tooltip("The percentage to modify the knockback of the bullet")]
        [SerializeField]
        private float knockbackPercentageChange;

        [Header("Smasher")]
        [SerializeField]
        [Tooltip("The percentage to modify the damage of smasher by")]
        private float smasherDamageChange;
        [SerializeField]
        [Tooltip("The percentage to modify the range of smasher by")]
        private float smasherRangeChange;
        
        /// <summary>
        /// Changes the turret's stats when added
        /// </summary>
        /// <param name="turret">The turret to change stats for</param>
        public override void AddModule(Turret turret)
        {
            if (turret.GetType() == typeof(Smasher))
            {
                turret.damage.AddModifier(smasherDamageChange);
                turret.range.AddModifier(smasherRangeChange);
            }
            else
            {
                turret.fireRate.AddModifier(fireRatePercentageChange);
                turret.range.AddModifier(rangePercentageChange);
            }
            
        }
        
        /// <summary>
        /// Removes stat modifications for a turret
        /// </summary>
        /// <param name="turret">The turret to revert stat changes for</param>
        public override void RemoveModule(Turret turret)
        {
            if (turret.GetType() == typeof(Smasher))
            {
                turret.damage.TakeModifier(smasherDamageChange);
                turret.range.TakeModifier(smasherRangeChange);
            }
            else
            {
                turret.fireRate.TakeModifier(fireRatePercentageChange);
                turret.range.TakeModifier(rangePercentageChange);
            }
        }
        
        /// <summary>
        /// Modifies the stats of a bullet when fired
        /// </summary>
        /// <param name="bullet">The bullet to add stats for</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.explosionRadius.AddModifier(explosionRadiusChange);
            bullet.damage.AddModifier(damagePercentageChange);
            bullet.speed.AddModifier(speedPercentageChange);
            bullet.knockbackAmount.AddModifier(knockbackPercentageChange);
            if (bullet.useLocation) return;
            
            bullet.useLocation = true;
            bullet.targetLocation = bullet.target.position;
        }
    }
}