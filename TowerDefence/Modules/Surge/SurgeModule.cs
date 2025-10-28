using System;
using System.Collections;
using System.Linq;
using BestagonDefence.Abstract;
using BestagonDefence.Abstract.Attributes;
using BestagonDefence.Levels._Tiles;
using BestagonDefence.Turrets;
using BestagonDefence.Turrets.Choker;
using BestagonDefence.Turrets.Lancer;
using BestagonDefence.Turrets.Smasher;
using Godot;

namespace BestagonDefence.Modules.Surge;

/// <summary>
/// Grants a temporary fire rate increase to a turret
/// </summary>
[GlobalClass]
[Icon("./TA_Surge.tres")]
[Tool]
public partial class SurgeModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Smasher), typeof(Lancer), typeof(Choker)];
        
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
    private Modifier _surgeShooterFireRateChange;
    /// <summary>
    /// Multiplicative percentage modifier to shooter's damage when surging
    /// </summary>
    [Export]
    private Modifier _surgeShooterDamageChange;
        
    /// <summary>
    /// Multiplicative percentage modifier to smasher's fire rate when surging
    /// </summary>
    [ExportGroup("Smasher Surging")]
    [Export]
    private Modifier _surgeSmasherFireRateChange;
    /// <summary>
    /// Multiplicative percentage modifier to smasher's range when surging
    /// </summary>
    [Export]
    private Modifier _surgeSmasherRangeChange;
        
        
    /// <summary>
    /// Multiplicative percentage modifier to lancer's fire rate when surging
    /// </summary>
    [ExportGroup("Lancer Surging")]
    [Export]
    private Modifier _surgeLancerFireRateChange;
    // TODO - Get this to work
    /// <summary>
    /// Multiplicative percentage modifier to lancer's arrow knockback when surging
    /// </summary>
    [Export]
    private Modifier _surgeLancerKnockbackChange;
        
    /// <summary>
    /// Multiplicative percentage modifier to choker's fire rate when surging
    /// </summary>
    [ExportGroup("Choker Surging")]
    [Export]
    private Modifier _surgeChokerFireRateChange;
    /// <summary>
    /// Multiplicative percentage modifier to choker's part count when surging
    /// </summary>
    [Export]
    private Modifier _surgeChokerPartCountChange;
        
    /// <summary>
    /// Multiplicative percentage modifier to part fire rate
    /// </summary>
    [ExportGroup("Cooldown effect")]
    [Export]
    private Modifier _fireRateChange;
    /// <summary>
    /// Multiplicative percentage modifier to part damage
    /// </summary>
    [Export]
    private Modifier _damageChange;

    /// <summary>
    /// Begins the surge effect on the turret
    /// </summary>
    /// <param name="damager">The turret to start the surge loop on</param>
    public override void AddModule(Damager damager)
    {
        if (damager is not Turret turret) return;
        // LINQ to get the turret tier
        int tier = damager.ModuleHandlers.Where(handler => handler.GetModule().GetType() == typeof(SurgeModule)).Select(handler => handler.GetTier()).FirstOrDefault();
            
        turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SurgeCooldown", _fireRateChange);
        turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SurgeCooldown", _damageChange);
            
        Runner.Run(Surge(turret, tier));
    }

    /// <summary>
    /// Handles removing the effects of surge on the damager
    /// </summary>
    /// <param name="damager">The damager to remove the module from</param>
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
            
        while (turret != null && turret.ModuleHandlers.Any(module => module.GetModule().GetType() == typeof(SurgeModule) && module.GetTier() == tier))
        {
            // SURGE!
            turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SurgeCooldown", new Modifier(0f));
            turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SurgeCooldown", new Modifier(0f));
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
                    break;
            }
            if (BuildableTile.SelectedTile == turret.GetParent()) BuildableTile.SelectedTile = BuildableTile.SelectedTile;
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
                    turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    turret.Stats[AttributeType.PartCount].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    break;
                case Lancer:
                    turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    break;
                case Shooter:
                    turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    turret.Stats[AttributeType.Damage].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    break;
                case Smasher:
                    turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    turret.Stats[AttributeType.Range].Add(GetSceneUniqueId() + "SURGE", new Modifier(0f));
                    break;
            }
            if (BuildableTile.SelectedTile == turret.GetParent()) BuildableTile.SelectedTile = BuildableTile.SelectedTile;
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