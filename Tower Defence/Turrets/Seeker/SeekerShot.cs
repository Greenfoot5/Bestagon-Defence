using Abstract.Attributes;
using Enemies;
using Godot;
using Turrets;

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
}
