using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Enemies;
using Godot;

namespace BestagonDefense.Turrets.Seeker;

public partial class SeekerShot : Bullet
{
    private ulong startTime;

    public override void _Ready()
    {
        base._Ready();

        startTime = Time.GetTicksMsec();
        
        ((DynamicTurret)Source).OnNewTarget += UpdateTarget;
        TargetLocation = Source.GlobalPosition;
    }

    /// <summary>
    /// Moves the bullet towards the target and check if it hits
    /// </summary>
    public override void _Process(double delta)
    {
        if (startTime + Stats[AttributeType.Lifetime].Value * 1000 < Time.GetTicksMsec())
        {
            Die();
            return;
        }

        if (!IsInstanceValid(Target) && !UseLocation)
            UpdateTarget(null);
        
        base._Process(delta);
    }

    public override void _ExitTree()
    {
        ((DynamicTurret)Source).OnNewTarget -= UpdateTarget;
    }

    private void UpdateTarget(Enemy target)
    {
        if (target == null)
        {
            UseLocation = true;
            return;
        }

        UseLocation = false;
        Target = target;
    }
    
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
    private void OnAreaEntered(Area2D col)
    {
        if (col is not Enemy enemy) return;

        if (IsInstanceValid(enemy) && IsInstanceValid(Target) && Target.GetInstanceId() == col.GetInstanceId())
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