using System;
using System.Collections;
using System.Linq;
using Abstract;
using Abstract.Attributes;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UI.Inventory;


namespace Modules.Surge
{
    /// <summary>
    /// Grants a temporary fire rate increase to a turret
    /// </summary>
    public partial class SurgeModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Lancer), typeof(Choker) };
        
        /// <summary>
        /// How many ticks to burn the enemy for
        /// </summary>
        [ExportGroup("Effect Details")]
        [Export]
        private int _duration;
        /// <summary>
        /// How long each tick is in seconds
        /// </summary>
        [Export]
        private float _cooldown;
        
        /// <summary>
        /// The VFX to spawn when the turret surges
        /// </summary>
        [Export]
        private GodotObject _surgeEffect;
        /// <summary>
        /// The VFX to spawn when the ends it's turret surge
        /// </summary>
        [Export]
        private GodotObject _surgeEndEffect;
        
        /// <summary>
        /// Multiplicative percentage modifier to shooter's fire rate when surging
        /// </summary>
        [Export]
        [ExportGroup("Shooter Surging")]
        private AttributeModifier _surgeShooterFireRateChange;
        /// <summary>
        /// Multiplicative percentage modifier to shooter's damage when surging
        /// </summary>
        [Export]
        private AttributeModifier _surgeShooterDamageChange;
        
        /// <summary>
        /// Multiplicative percentage modifier to smasher's fire rate when surging
        /// </summary>
        [ExportGroup("Smasher Surging")]
        [Export]
        private AttributeModifier _surgeSmasherFireRateChange;
        /// <summary>
        /// Multiplicative percentage modifier to smasher's range when surging
        /// </summary>
        [Export]
        private AttributeModifier _surgeSmasherRangeChange;
        
        
        /// <summary>
        /// Multiplicative percentage modifier to lancer's fire rate when surging
        /// </summary>
        [ExportGroup("Lancer Surging")]
        [Export]
        private AttributeModifier _surgeLancerFireRateChange;
        // TODO - Get this to work
        /// <summary>
        /// Multiplicative percentage modifier to lancer's arrow knockback when surging
        /// </summary>
        [Export]
        private AttributeModifier _surgeLancerKnockbackChange;
        
        /// <summary>
        /// Multiplicative percentage modifier to choker's fire rate when surging
        /// </summary>
        [ExportGroup("Choker Surging")]
        [Export]
        private AttributeModifier _surgeChokerFireRateChange;
        /// <summary>
        /// Multiplicative percentage modifier to choker's part count when surging
        /// </summary>
        [Export]
        private AttributeModifier _surgeChokerPartCountChange;
        
        /// <summary>
        /// Multiplicative percentage modifier to part fire rate
        /// </summary>
        [ExportGroup("Cooldown effect")]
        [Export]
        private AttributeModifier _fireRateChange;
        /// <summary>
        /// Multiplicative percentage modifier to part damage
        /// </summary>
        [Export]
        private AttributeModifier _damageChange;

        /// <summary>
        /// Begins the surge effect on the turret
        /// </summary>
        /// <param name="damager">The turret to start the surge loop on</param>
        public override void AddModule(Damager damager)
        {
            if (damager is not Turret turret) return;
            // LINQ to get the turret tier
            int tier = damager.moduleHandlers.Where(handler => handler.GetModule().GetType() == typeof(SurgeModule)).Select(handler => handler.GetTier()).FirstOrDefault();
            
            turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SurgeCooldown", _fireRateChange);
            turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SurgeCooldown", _damageChange);
            
            Runner.Run(Surge(turret, tier));
        }

        // TODO - What if removed while surging?
        public override void RemoveModule(Damager damager)
        {
            if (damager is not Turret turret) return;
            turret.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId() + "SurgeCooldown");
            turret.Stats[AttributeType.Damage].Remove(GetSceneUniqueId() + "SurgeCooldown");
            
            turret.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId() + "SURGE");
            turret.Stats[AttributeType.PartCount].Remove(GetSceneUniqueId() + "SURGE");
            turret.Stats[AttributeType.Damage].Remove(GetSceneUniqueId() + "SURGE");
            turret.Stats[AttributeType.Range].Remove(GetSceneUniqueId() + "SURGE");
        }

        /// <summary>
        /// Handles the surge effect
        /// </summary>
        /// <param name="turret">The turret to increase the fire rate for</param>
        /// <param name="tier">The tier of the module</param>
        private IEnumerator Surge(Turret turret, int tier)
        {
            // Wait the cooldown
            // yield return new WaitForSeconds(cooldown);
            
            while (turret != null && turret.moduleHandlers.Any(module => module.GetModule().GetType() == typeof(SurgeModule) && module.GetTier() == tier))
            {
                // SURGE!
                turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SurgeCooldown", new AttributeModifier(0f));
                turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SurgeCooldown", new AttributeModifier(0f));
                switch (turret)
                {
                    case Choker:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", _surgeChokerFireRateChange);
                        turret.Stats[AttributeType.PartCount].Add(GetSceneUniqueId() + "SURGE", _surgeChokerPartCountChange);
                        break;
                    case Lancer:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", _surgeLancerFireRateChange);
                        break;
                    case Shooter:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", _surgeShooterFireRateChange);
                        turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SURGE", _surgeShooterDamageChange);
                        break;
                    case Smasher:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", _surgeSmasherFireRateChange);
                        turret.Stats[AttributeType.Range].Add(GetSceneUniqueId() + "SURGE", _surgeSmasherRangeChange);
                        turret.UpdateRange();
                        break;
                }
                TurretInfo.instance.UpdateStats();
                Vector2 position = turret.Position;
                // TODO - Create & remove effect after duration
                // GodotObject effect = Instantiate(surgeEffect, position, Quaternion.identity);
                // effect.Name = "_" + effect.Name;
                // Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
                

                // yield return new WaitForSeconds(duration);
                
                turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SurgeCooldown", _fireRateChange);
                turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SurgeCooldown", _damageChange);
                switch (turret)
                {
                    case Choker:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        turret.Stats[AttributeType.PartCount].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        break;
                    case Lancer:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        break;
                    case Shooter:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        break;
                    case Smasher:
                        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        turret.Stats[AttributeType.Range].Add(GetSceneUniqueId() + "SURGE", new AttributeModifier(0f));
                        turret.UpdateRange();
                        break;
                }
                TurretInfo.instance.UpdateStats();
                // TODO - Create & remove effect after duration
                // GodotObject endEffect = Instantiate(surgeEndEffect, position, Quaternion.identity);
                // endEffect.Name = "_" + endEffect.Name;
                // Destroy(endEffect, endEffect.GetComponent<ParticleSystem>().main.duration);
                
                // Wait the cooldown
                // yield return new WaitForSeconds(cooldown);
            }
            yield break;
        }
    }
}