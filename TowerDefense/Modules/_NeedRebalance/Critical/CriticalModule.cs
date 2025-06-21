using System;
using Enemies;
using Godot;
using Turrets;

namespace Modules.Critical
{
    /// <summary>
    /// Chance to deal double damage
    /// </summary>
    public partial class CriticalModule : Module
    {
        protected override Type[] ValidTypes => new[] {typeof(Turret)};
        
        /// <summary>
        /// Percentage chance to deal double damage
        /// </summary>
        [Export]
        private float criticalChance;

        public override void AddModule(Damager damager)
        {
            damager.OnHit += OnHit;
        }

        public override void RemoveModule(Damager damager)
        {
            damager.OnHit -= OnHit;
        }

        /// <summary>
        /// Attempts to deal double damage on all enemies hit
        /// </summary>
        /// <param name="target">The targets to attempt to critically strike</param>
        /// <param name="damager">The damager that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public void OnHit(Enemy target, Damager damager, Bullet bullet = null)
        {
            if (damager is not Turret turret) return;
            if (!(GD.Randf() < criticalChance / turret.fireRate.GetStat())) return;

            target.TakeDamageWithoutAbilities(bullet == null
                ? turret.damage.GetStat()
                : damager.damage.GetStat());
        }
    }
}