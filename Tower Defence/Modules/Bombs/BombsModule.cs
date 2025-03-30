using System;
using Abstract.Attributes;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;


namespace Modules.Bombs
{
    /// <summary>
    /// Extends the Module class to create a BombBullet upgrade
    /// </summary>
    public partial class BombsModule : Module
    {
        // Choker - Fewer shots, are explosive
        // Gunner - Small explosive bullets
        // Lancer - No change
        // Laser - No effect
        // Shooter - Targets location, significant explosion, slight slow of bullet speed
        // Smasher - Larger range & damage
        protected override Type[] ValidTypes => new[] { typeof(Choker), typeof(Gunner), typeof(Shooter), typeof(Smasher)};
        
        /// <summary>
        /// Additive percentage modifier to part explosion radius
        /// </summary>
        [ExportGroup("Choker")]
        [Export]
        private AttributeModifier _chokerExplosionRadiusChange;
        /// <summary>
        /// Multiplicative percentage modifier to part count
        /// </summary>
        [Export]
        private AttributeModifier _chokerBulletCountChange;
        /// <summary>
        /// Additive percentage modifier to choker damage
        /// </summary>
        [Export]
        private AttributeModifier _chokerDamageChange;
        
        /// <summary>
        /// Additive percentage modifier to pellet explosion radius
        /// </summary>
        [ExportGroup("Gunner")]
        [Export]
        private AttributeModifier _gunnerExplosionRadiusChange;
        /// <summary>
        /// Additive percentage modifier to gunner damage
        /// </summary>
        [Export]
        private AttributeModifier _gunnerDamageChange;
        
        /// <summary>
        /// Additive percentage modifier to bullet explosion radius
        /// </summary>
        [ExportGroup("Shooter")]
        [Export]
        private AttributeModifier _shooterExplosionRadiusChange;
        /// <summary>
        /// Additive percentage modifier to bullet speed
        /// </summary>
        [Export]
        private AttributeModifier _shooterBulletSpeedChange;
        /// <summary>
        /// Additive percentage modifier to shooter damage
        /// </summary>
        [Export]
        private AttributeModifier _shooterDamageChange;
        
        /// <summary>
        /// The percentage to modify the damage of smasher by
        /// </summary>
        [ExportGroup("Smasher")]
        [Export]
        private AttributeModifier _smasherDamageChange;
        /// <summary>
        /// The percentage to modify the range of smasher by
        /// </summary>
        [Export]
        private AttributeModifier _smasherRangeChange;
        
        /// <summary>
        /// Changes the turret's stats when added
        /// </summary>
        /// <param name="damager">The turret to change stats for</param>
        public override void AddModule(Damager damager)
        {
            damager.OnShoot += OnShoot;
            switch (damager)
            {
                case Choker:
                    damager.Stats[AttributeType.PartCount].Add(GetSceneUniqueId(), _chokerBulletCountChange);
                    damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _chokerDamageChange);
                    break;
                case Gunner:
                    damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _gunnerDamageChange);
                    break;
                case Shooter:
                    damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _shooterDamageChange);
                    break;
                case Smasher:
                    damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _smasherDamageChange);
                    damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _smasherRangeChange);
                    break;
            }
        }
        
        /// <summary>
        /// Removes stat modifications for a turret
        /// </summary>
        /// <param name="damager">The turret to revert stat changes for</param>
        public override void RemoveModule(Damager damager)
        {
            damager.OnShoot -= OnShoot;
            damager.Stats[AttributeType.Damage].Remove(GetSceneUniqueId());
            switch (damager)
            {
                case Choker:
                    damager.Stats[AttributeType.PartCount].Remove(GetSceneUniqueId());
                    break;
                case Smasher:
                    damager.Stats[AttributeType.Range].Remove(GetSceneUniqueId());
                    break;
            }
        }
        
        /// <summary>
        /// Modifies the stats of a bullet when fired
        /// </summary>
        /// <param name="bullet">The bullet to add stats for</param>
        private void OnShoot(Bullet bullet)
        {
            switch (bullet.source)
            {
                case Choker:
                    bullet.Stats[AttributeType.ExplosionRadius].Add(GetSceneUniqueId(), _chokerExplosionRadiusChange);
                    break;
                case Gunner:
                    bullet.Stats[AttributeType.ExplosionRadius].Add(GetSceneUniqueId(), _gunnerExplosionRadiusChange);
                    break;
                case Shooter:
                    bullet.Stats[AttributeType.ExplosionRadius].Add(GetSceneUniqueId(), _shooterExplosionRadiusChange);
                    bullet.Stats[AttributeType.Speed].Add(GetSceneUniqueId(), _shooterBulletSpeedChange);
                    bullet.useLocation = true;
                    bullet.targetLocation = bullet.target.Position;
                    break;
            }
        }
    }
}