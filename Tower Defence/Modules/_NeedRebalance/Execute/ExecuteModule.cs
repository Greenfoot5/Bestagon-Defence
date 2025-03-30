using System;
using System.Collections.Generic;
using Enemies;
using Godot;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;


namespace Modules.Execute
{
    /// <summary>
    /// Extends the module class to create the execute module
    /// </summary>
    public partial class ExecuteModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Lancer) };
        
        /// <summary>
        /// The maximum percentage of health the enemy can have before they get executed
        /// </summary>
        [Export]
        private float percentageHealthRemaining;

        public override void AddModule(Damager damager)
        {
            damager.OnHit += OnHit;
        }

        public override void RemoveModule(Damager damager)
        {
            damager.OnHit -= OnHit;
        }

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="target">The target(s) to apply the ability to</param>
        /// <param name="damager">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        private void OnHit(Enemy target, Damager damager, Bullet bullet = null)
        {
            if ((target.Health / target.MaxHealth) <= percentageHealthRemaining)
            {
                target.TakeDamage(target.MaxHealth, null);
            }
        }
    }
}