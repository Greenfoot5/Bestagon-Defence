using System;
using Abstract.Attributes;
using Enemies;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;


namespace Modules.Frostbite;

/// <summary>
/// Extends the Module class to create a Damage upgrade
/// </summary>
[GlobalClass]
public partial class FrostbiteModule : Module
{
    protected override Type[] ValidTypes => [typeof(Choker), typeof(Gunner), typeof(Lancer), typeof(Shooter)];
        
    /// <summary>
    /// Multiplicative modifier to damage based on seconds left of slow
    /// </summary>
    [Export]
    private AttributeModifier _damageMultiplier = new(0f, Operation.Multiplicative);
    /// <summary>
    /// Multiplicative percentage modifier to bullet explosion radius
    /// </summary>
    [Export]
    private AttributeModifier _bulletExplosionRadius = new(0f, Operation.Multiplicative);
    // <summary>
    // Slow effect of any level (used to get effect key)
    // </summary>
    // [Export]
    // TODO - What should enemyEffect type be?
    // private EnemyEffect enemyEffect;
        
    /// <summary>
    /// Changes the turret's stats when added
    /// </summary>
    /// <param name="damager">The turret to change stats for</param>
    public override void AddModule(Damager damager)
    {
        damager.OnShoot += OnShoot;
        damager.OnHit += OnHit;
    }
        
    /// <summary>
    /// Removes stat modifications for a turret
    /// </summary>
    /// <param name="damager">The turret to revert stat changes for</param>
    public override void RemoveModule(Damager damager)
    {
        damager.OnShoot -= OnShoot;
        damager.OnHit -= OnHit;
    }
        
    /// <summary>
    /// Modifies the stats of a bullet when fired
    /// </summary>
    /// <param name="bullet">The bullet to add stats for</param>
    private void OnShoot(Bullet bullet)
    {
        bullet.Stats[AttributeType.ExplosionRadius].Add(GetSceneUniqueId(), _bulletExplosionRadius);
    }

    /// <summary>
    /// Remove the slow effect and deal damage
    /// </summary>
    private void OnHit(Enemy enemy, Damager damager, Bullet bullet)
    {
        // TODO - Check for slow & clear
        // if (!enemy.ActiveEffects.TryGetValue(enemyEffect.effectType, out EnemyEffect effect)) return;
            
        // bullet.damage.MultiplyModifier(damageMultiplier * effect.ticksLeft * effect.tickDuration);
        // effect.isCancelled = true;
    }
}