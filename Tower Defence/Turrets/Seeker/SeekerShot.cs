using Abstract.Attributes;
using Enemies;
using Godot;
using Turrets;

namespace Turrets.Seeker;

public partial class SeekerShot : Bullet
{
    /// <summary>
    /// Called when the bullet hits the target
    /// </summary>
    protected override void HitTarget(bool isEnemy, Enemy enemy = null)
    {
        enemy ??= Target;
            
        // TODO - Impact Effects
            
        // var effect = impactEffect.Instantiate<Node2D>();
        // effect.Name = "_" + effect.Name;
        // effect.Position = Position;
        // effect.Rotation = Rotation;
        //
        // GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };

        if (isEnemy)
        {
            // If the bullet has AoE damage or not
            if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                Explode();
            else
                Damage(enemy);
        }
        else
        {
            if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                Explode();
        }

        // Destroy so the bullet only hits the target once
        // QueueFree();
    }
    
    /// <summary>
    /// Deals damage to hit enemies the first time when the bullet should
    /// </summary>
    /// <param name="col">The collider that was touched</param>
    protected new void OnAreaEntered(Area2D col)
    {
        if (col is not Enemy enemy) return;

        if (IsInstanceValid(enemy) && Target.GetInstanceId() == col.GetInstanceId())
        {
            HitTarget(true, enemy);
            return;
        }

        if (IsEthereal)
        {
            if (!WillHitFirst)
                Damage(Target);
            else
                HitTarget(true, enemy);
        }
    }
}