using System;
using Abstract.Attributes;
using Enemies;
using Godot;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;

namespace Modules.Instakill;

/// <summary>
/// Chance to instakill an enemy
/// </summary>
[GlobalClass]
public partial class InstakillModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer)];
        
    /// <summary>
    /// The percentage chance to kill the enemy
    /// </summary>
    [Export]
    private float _instakillChance;
        
    /// <summary>
    /// The effect to play on kill
    /// </summary>
    [Export]
    private PackedScene _instakillEffect;

    public override void AddModule(Damager damager)
    {
        damager.OnHit += OnHit;
    }

    public override void RemoveModule(Damager damager)
    {
        damager.OnHit -= OnHit;
    }

    /// <summary>
    /// Attempt to instakill all hit enemies
    /// </summary>
    /// <param name="target">The targets to attempt to instakill</param>
    /// <param name="damager">The turret that attacked the enemies</param>
    /// <param name="bullet">The bullet (if any) that hit the enemies</param>
    private void OnHit(Enemy target, Damager damager, Bullet bullet = null)
    {
        if (damager is not Turret turret) return;
        if (!(GD.Randf() < (_instakillChance / turret.Stats[AttributeType.FireRate].Value)) || !(target.Health > 0) ||
            target.Stats.IsBoss) return;
            
        var effect = _instakillEffect.Instantiate<Node2D>();
        effect.Position = target.Position;
            
        // TODO - Use the correct time
        effect.GetTree().CreateTimer(1).Timeout += () => { effect.QueueFree(); };
        target.TakeDamage(target.Stats.Attributes[AttributeType.MaxHealth].Value, this);
    }
}