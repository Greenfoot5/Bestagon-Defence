using System.Collections.Generic;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Enemies;
using Godot;

namespace BestagonDefense.Turrets.Laser;

/// <summary>
/// Extends DynamicTurret to add Laser functionality
/// </summary>
public partial class Laser : DynamicTurret
{
    [Export]
    private Line2D line;

    /// <summary> How long left until the next attack </summary>
    private double _durationCountdown;
    /// <summary> How long left until the next attack </summary>
    private double _cooldownCountdown;

    public Laser()
    {
        Stats[AttributeType.LaserDuration] = new Attribute(AttributeType.LaserDuration, 1f);
        Stats[AttributeType.LaserCooldown] = new Attribute(AttributeType.LaserCooldown, 1f);
    }

    /// <summary>
    /// Fires the laser if the turret have a target and are looking at them.
    /// Otherwise, rotate to target if there is one.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        _durationCountdown -= delta;
            
        if (_cooldownCountdown >= 0 && _durationCountdown <= 0)
        {
            _cooldownCountdown -= delta;

            if (TargetEnemy is not null)
                LookAtTarget(delta);
                
            line.Visible = false;
            return;
        }
            
        // Don't do anything if the turret doesn't have a target
        // or fire rate is <= 0
        if (TargetEnemy is null || Stats[AttributeType.LaserDuration].Value < 0)
        {
            return;
        }
        
        // Rotates the turret each frame
        LookAtTarget(delta);
            
        // One of the two laser timers expired
        if (_durationCountdown <= 0)
        {
            if (IsLookingAtTarget())
            {
                _durationCountdown = Stats[AttributeType.LaserDuration].Value;
                _cooldownCountdown = Stats[AttributeType.LaserCooldown].Value;
            }

            return;
        }
            
        Attack((float)delta);
    }
        
    /// <summary>
    /// Fires the laser towards the enemy and deals damage
    /// </summary>
    /// <param name="delta">The time since last frame (in seconds)</param>
    protected override void Attack(float delta)
    {
        var enemies = new List<Enemy>();

        while (Ray.GetCollider() != null)
        {
            Ray.AddException((CollisionObject2D)Ray.GetCollider());
            if (Ray.GetCollider() is Enemy enemy)
            {
                Ray.ForceRaycastUpdate();
                enemies.Add(enemy);
            }
        }
            
        Ray.ClearExceptions();
            
        base.Attack(this);
        HitMany(enemies, this);
            
        // Deal damage to every enemy hit
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(Stats[AttributeType.Damage].Value * delta, this);
        }

        // Enable visuals
        if (!line.Visible)
        {
            line.Visible = true;
        }
    }
}