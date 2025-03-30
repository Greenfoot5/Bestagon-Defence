using System;
using System.Collections;
using Abstract;
using Enemies;
using Godot;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UI.Inventory;

namespace Modules.Stun
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    public partial class StunModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };
        
        /// <summary>
        /// The percentage chance to stun an enemy
        /// </summary>
        [Export]
        private float enemyStunChance;
        /// <summary>
        /// How long to stun the enemy for
        /// </summary>
        [Export]
        private float enemyDuration;
        /// <summary>
        /// percentage chance to stun the tower each attack
        /// </summary>
        [Export]
        private float turretStunChance;
        /// <summary>
        /// How long to stun the tower for
        /// </summary>
        [Export]
        private float turretDuration;
        /// <summary>
        /// The VFX to spawn when the turret is stunned
        /// </summary>
        [Export]
        private GodotObject stunEffect;
        /// <summary>
        /// The VFX to spawn when the ends it's turret stun
        /// </summary>
        [Export]
        private GodotObject stunEndEffect;
        
        public override void AddModule(Damager damager)
        {
            damager.OnHit += OnHit;
            damager.OnAttack += OnAttack;
        }

        public override void RemoveModule(Damager damager)
        {
            damager.OnHit -= OnHit;
            damager.OnAttack -= OnAttack;
        }

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="target">The target(s) to apply the ability to</param>
        /// <param name="damager">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        private void OnHit(Enemy target, Damager damager, Bullet bullet = null)
        {
            if (damager is not Turret turret) return;
            Runner.Run(StunEnemy(target, turret));
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to stun</param>
        /// <param name="turret">The turret attempting to stun</param>
        private IEnumerator StunEnemy(Enemy target, Turret turret)
        {
            // Check the enemy isn't already stunned/has immunity
            if (target.UniqueEffects.Contains("Stun"))
            {
                yield break;
            }
            target.UniqueEffects.Add("Stun");
            
            float originalSpeed = target.Speed.GetBase();
            // Check the target isn't already stunned (again) and the turret hit the chance
            if (originalSpeed <= 0 || GD.Randf() > (enemyStunChance / turret.fireRate.GetStat())) yield break;
            
            target.Speed.SetBase(0);

            // yield return new WaitForSeconds(enemyDuration);

            target.Speed.SetBase(originalSpeed);
        }

        private void OnAttack(Damager damager)
        {
            if (damager is not Turret turret) return;
            if (GD.Randf() < turretStunChance) 
                Runner.Run(StunTurret(turret));
        }

        private IEnumerator StunTurret(Turret turret)
        {
            float originalFireRate = turret.fireRate.GetBase();
            float originalRotSpeed = 1;
            if (turret is DynamicTurret dynamicTurret)
                originalRotSpeed = dynamicTurret.rotationSpeed.GetBase();

            // Check the turret isn't already stunned
            if (originalFireRate <= 0)
                yield break;
            
            // Summons the stun particle effect
            Vector2 position = turret.Position;
            // TODO - Spawn and remove effect
            // GodotObject effect = Instantiate(stunEffect, position, Quaternion.identity);
            // effect.Name = "_" + effect.Name;
            // Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
            
            // Updates the fire rate and rotation speed
            turret.fireRate.SetBase(0);
            if (turret is DynamicTurret dynamicTurret1)
                dynamicTurret1.rotationSpeed.SetBase(0);
            TurretInfo.instance.UpdateStats();
            
            // yield return new WaitForSeconds(turretDuration);
            
            // Summons the end stun particle effect
            // TODO - Spawn and remove effect
            // GodotObject endEffect = Instantiate(stunEndEffect, position, Quaternion.identity);
            // endEffect.Name = "_" + endEffect.Name;
            // Destroy(endEffect, endEffect.GetComponent<ParticleSystem>().main.duration);
            
            // Resets fire rate and rotation speed
            turret.fireRate.SetBase(originalFireRate);
            if (turret is DynamicTurret dynamicTurret2)
                dynamicTurret2.rotationSpeed.SetBase(originalRotSpeed);
            TurretInfo.instance.UpdateStats();
        }
    }
}
