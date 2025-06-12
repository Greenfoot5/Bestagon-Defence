using System;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Enemies;
using BestagonDefense.Turrets;
using BestagonDefense.Turrets.Choker;
using BestagonDefense.Turrets.Gunner;
using BestagonDefense.Turrets.Lancer;
using BestagonDefense.Turrets.Smasher;
using Godot;

namespace BestagonDefense.Modules.Slow;

/// <summary>
/// Extends the Module class to create a DebuffEnemy upgrade,
/// Used to add effects to enemies
/// </summary>
[GlobalClass]
[Tool]
public partial class SlowModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Gunner), typeof(Lancer), typeof(Choker), typeof(Smasher)
    ];
        
    /// <summary>
    /// The percentage the slow the enemy's movement speed
    /// </summary>
    [Export]
    private PackedScene _effect;
        
    /// <summary>
    /// Multiplicative percentage modifier to damage
    /// </summary>
    [Export]
    private Modifier _damageChange;

    /// <summary>
    /// Modifies the stats of the turret when applied
    /// </summary>
    /// <param name="damager">The turret to modify the stats for</param>
    public override void AddModule(Damager damager)
    {
        damager.OnHit += OnHit;
        damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _damageChange);
    }
        
    /// <summary>
    /// Removes stats modifications of the turret
    /// </summary>
    /// <param name="damager">The turrets to remove the modifications of</param>
    public override void RemoveModule(Damager damager)
    {
        damager.OnHit -= OnHit;
        damager.Stats[AttributeType.Damage].Remove(GetSceneUniqueId());
    }

    /// <summary>
    /// Adds the EnemyAbility to some target(s)
    /// </summary>
    /// <param name="target">The target(s) to apply the ability to</param>
    /// <param name="damager">The turret that attacked the enemies</param>
    /// <param name="bullet">The bullet (if any) that hit the enemies</param>
    private void OnHit(Enemy target, Damager damager, Bullet bullet = null)
    {
        if (damager is not Turret) return;
        // TODO - Somehow instantiate the effect
        var newEffect = _effect.Instantiate<SlowEnemyEffect>();
        newEffect.Apply(target);
    }
}