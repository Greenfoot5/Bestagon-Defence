using Abstract.Attributes;
using Enemies;
using Godot;

namespace Turrets.Smasher;

/// <summary>
/// Extends Turret to add smashing functionality
/// </summary>
public partial class Smasher : Turret
{
    /// <summary>
    /// The effect to play when the smasher attacks
    /// </summary>
    [Export]
    private GpuParticles2D smashEffect;

    /// <summary>
    /// Check for new enemies in radius and attacks if there are.
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!Range.HasOverlappingAreas())
        {
            FireCountdown -= delta;
            return;
        }
            
        // If our attack is off cooldown
        if (FireCountdown <= 0 && Stats[AttributeType.FireRate].Value != 0)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            Attack((float) delta);
        }
            
        FireCountdown -= delta;
    }

    protected override void UpdateRange(Attribute attribute)
    {
        base.UpdateRange(attribute);
            
        // Update the smash effect size
        float scale = Stats[AttributeType.Range].Value * 0.05f;
        ((ParticleProcessMaterial)smashEffect.ProcessMaterial).Scale = new Vector2(scale, scale);
    }
        
    /// <summary>
    /// Deals damage to all enemies in range
    /// </summary>
    protected override void Attack(float delta)
    {
        smashEffect.Emitting = true;
            
        base.Attack(this);
            
        foreach (Area2D area in Range.GetOverlappingAreas())
        {
            if (area is not Enemy enemy) return;
                
            // Take damage depending on how close the enemy is to the turret's centre
            float distance = GlobalPosition.DistanceSquaredTo(area.GlobalPosition);
            // Magic scale
            distance /= 20 * 20;
            distance /= Stats[AttributeType.Range].Value * Stats[AttributeType.Range].Value;

            float damagePercentage = (1 - distance) + 0.5f;
                
            // We want to deal *some* damage to every enemy, and not too much
            damagePercentage = Mathf.Clamp(damagePercentage, 0.2f, 1f);
                     
            Hit(enemy, this);
            GD.Print(Stats[AttributeType.Damage].Value * damagePercentage);
            enemy.TakeDamage(Stats[AttributeType.Damage].Value * damagePercentage, this);
        }
    }
}