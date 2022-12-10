using System;
using System.Linq;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.Velocity
{
    /// <summary>
    /// Increases the speed of bullets
    /// </summary>
    [CreateAssetMenu(fileName = "VelocityT0", menuName = "Modules/Velocity")]
    public class VelocityModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Smasher), typeof(Lancer) };

        [Header("Gunner & Shooter & Lancer")]
        [Tooltip("The percentage to modify the speed of the bullet by")]
        [SerializeField]
        private float bulletSpeedChange;
        [Tooltip("The percentage to modify the fire rate of the turret by")]
        [SerializeField]
        private float fireRateChange;
        
        [Header("Gunner & Shooter")]
        [Tooltip("The percentage to modify the range of the bullet by")]
        [SerializeField]
        private float rangeChange;
        
        
        [Header("Lancer")]
        [SerializeField]
        [Tooltip("The percentage to modify the range of the Lancer's bullet")]
        private float lancerBulletRangeChange;

        [Header("Smasher")]
        [SerializeField]
        [Tooltip("The percentage to modify the damage of the smasher by")]
        private float smasherDamageChange;
        [SerializeField]
        [Tooltip("The percentage to modify the range of the smasher by")]
        private float smasherRangeChange;
        [SerializeField]
        [Tooltip("The percentage to modify the fire rate of the smasher by")]
        private float smasherFireRateChange;

        /// <summary>
        /// Increases the bullet speed of a turret
        /// </summary>
        /// <param name="turret">The turret to apply the modifications to</param>
        public override void AddModule(Turret turret)
        {
            if (turret.GetType() != typeof(Smasher))
            {
                turret.fireRate.AddModifier(fireRateChange);
                if (turret.GetType() == typeof(Lancer))
                {
                    ((Lancer) turret).bulletRange.AddModifier(lancerBulletRangeChange);
                }
                else
                {
                    turret.range.AddModifier(rangeChange);
                }
            }
            else
            {
                turret.damage.AddModifier(smasherDamageChange);
                turret.range.AddModifier(smasherRangeChange);
                turret.fireRate.AddModifier(smasherFireRateChange);
            }
        }
        
        /// <summary>
        /// Decreases the bullet speed
        /// </summary>
        /// <param name="turret"></param>
        public override void RemoveModule(Turret turret)
        {
            if (new[] {typeof(Shooter), typeof(Gunner), typeof(Lancer)}.Contains(turret.GetType()))
            {
                turret.fireRate.TakeModifier(fireRateChange);
                if (turret.GetType() == typeof(Lancer))
                {
                    ((Lancer) turret).bulletRange.TakeModifier(lancerBulletRangeChange);
                }
                else
                {
                    turret.range.AddModifier(rangeChange);
                }
            }
            else
            {
                turret.damage.TakeModifier(smasherDamageChange);
                turret.range.TakeModifier(smasherRangeChange);
                turret.fireRate.TakeModifier(smasherFireRateChange);
            }
        }
        
        /// <summary>
        /// Increases the speed of the bullet once fired
        /// </summary>
        /// <param name="bullet">The bullet to accelerate</param>
        public override void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(bulletSpeedChange);
        }
    }
}