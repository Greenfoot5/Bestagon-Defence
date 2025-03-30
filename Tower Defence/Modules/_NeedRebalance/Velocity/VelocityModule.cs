using System;
using Godot;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;


namespace Modules.Velocity
{
    /// <summary>
    /// Increases the speed of bullets
    /// </summary>
    public partial class VelocityModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Smasher), typeof(Lancer) };
        
        /// <summary>
        /// The percentage to modify the speed of the bullet by
        /// </summary>
        [ExportGroup("Gunner & Shooter & Lancer")]
        [Export]
        private float bulletSpeedChange;
        /// <summary>
        /// The percentage to modify the fire rate of the turret by
        /// </summary>
        [Export]
        private float fireRateChange;
        
        /// <summary>
        /// The percentage to modify the range of the bullet by
        /// </summary>
        [ExportGroup("Gunner & Shooter")]
        [Export]
        private float rangeChange;
        
        
        /// <summary>
        /// The percentage to modify the range of the Lancer's bullet
        /// </summary>
        [ExportGroup("Lancer")]
        [Export]
        private float lancerBulletRangeChange;
        
        /// <summary>
        /// The percentage to modify the damage of the smasher by
        /// </summary>
        [ExportGroup("Smasher")]
        [Export]
        private float smasherDamageChange;
        /// <summary>
        /// The percentage to modify the fire rate of the smasher by
        /// </summary>
        [Export]
        private float smasherFireRateChange;
        /// <summary>
        /// The percentage to modify the range of the smasher by
        /// </summary>
        [Export]
        private float smasherRangeChange;

        /// <summary>
        /// Increases the bullet speed of a turret
        /// </summary>
        /// <param name="damager">The turret to apply the modifications to</param>
        public override void AddModule(Damager damager)
        {
            switch (damager)
            {
                case Smasher smasher:
                    smasher.damage.AddModifier(smasherDamageChange);
                    smasher.range.AddModifier(smasherRangeChange);
                    smasher.fireRate.AddModifier(smasherFireRateChange);
                    break;
                case Lancer lancer:
                    lancer.fireRate.AddModifier(fireRateChange);
                    lancer.bulletRange.AddModifier(lancerBulletRangeChange);
                    break;
                case Turret turret:
                    turret.fireRate.AddModifier(fireRateChange);
                    turret.range.AddModifier(rangeChange);
                    break;
            }

            damager.OnShoot += OnShoot;
        }
        
        /// <summary>
        /// Decreases the bullet speed
        /// </summary>
        /// <param name="damager"></param>
        public override void RemoveModule(Damager damager)
        {
            switch (damager)
            {
                case Smasher smasher:
                    smasher.damage.TakeModifier(smasherDamageChange);
                    smasher.range.TakeModifier(smasherRangeChange);
                    smasher.fireRate.TakeModifier(smasherFireRateChange);
                    break;
                case Lancer lancer:
                    lancer.fireRate.TakeModifier(fireRateChange);
                    lancer.bulletRange.TakeModifier(lancerBulletRangeChange);
                    break;
                case Turret turret:
                    turret.fireRate.TakeModifier(fireRateChange);
                    turret.range.TakeModifier(rangeChange);
                    break;
            }

            damager.OnShoot -= OnShoot;
        }
        
        /// <summary>
        /// Increases the speed of the bullet once fired
        /// </summary>
        /// <param name="bullet">The bullet to accelerate</param>
        private void OnShoot(Bullet bullet)
        {
            bullet.speed.AddModifier(bulletSpeedChange);
        }
    }
}