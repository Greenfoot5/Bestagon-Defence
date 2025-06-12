using BestagonDefense.Abstract.Attributes;
using Godot;

namespace BestagonDefense.Turrets.Choker;

/// <summary>
/// Extends DynamicTurret to add Choking functionality.
/// </summary>
public partial class Choker : DynamicTurret
{
    // Bullets
    /// <summary>
    /// The bullet prefab to spawn each attack
    /// </summary>
    [Export]
    private PackedScene _bulletPrefab;
    // <summary>
    // The effect to fire when the bullet is shot
    // </summary>
    // [Export]
    // private VisualEffect attackEffect;

    public Choker()
    {
        Stats[AttributeType.PartSpread] = new Attribute(AttributeType.PartSpread, 0.5f, min: 0f);
        Stats[AttributeType.PartCount] = new Attribute(AttributeType.PartCount, 10f, min: 0f);
    }

    /// <summary>
    /// Rotates towards the target if the turret have one.
    /// Shoots if the turret is looking towards the target
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
            
        // Don't do anything if the turret doesn't have a target
        if (TargetEnemy is null)
        {
            FireCountdown -= delta;
            return;
        }
        
        // Rotates the turret each frame
        LookAtTarget(delta);

        if (!IsLookingAtTarget())
        {
            FireCountdown -= delta;
            return;
        }
            
        if (FireCountdown <= 0)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            Attack((float) delta);
        }
            
        FireCountdown -= delta;
    }

    /// <summary>
    /// Creates a spray of bullets firing in a cone shape
    /// </summary>
    /// <param name="delta">The time since last frame (in seconds)</param>
    protected override void Attack(float delta)
    {
        //attackEffect?.Play();

        // Any decimal count is a chance for an additional part
        int count = ((int)Stats[AttributeType.PartCount].Value) + (GD.Randf() < (Stats[AttributeType.PartCount].Value % 1f) ? 1 : 0);
            
        // float oneSegment = spreadSize / (spreadAmount - 1);
        for (var i = 0; i < count; i++)
        {
            var bullet = _bulletPrefab.Instantiate<Bullet>();
            bullet.GlobalPosition = FirePoint.GlobalPosition;
            bullet.Name = "_" + bullet.Name;
                
            float bulletAngle = FirePoint.GlobalRotation + (float)GD.RandRange(Stats[AttributeType.PartSpread].Value / -2, Stats[AttributeType.PartSpread].Value / 2);
            bullet.GlobalRotation = bulletAngle;
                
            var bulletDirection = new Vector2(Mathf.Sin(bulletAngle), -Mathf.Cos(bulletAngle));
            bullet.Seek(FirePoint.GlobalPosition + bulletDirection * Stats[AttributeType.Range].Value * 20, this);
                
            GetTree().Root.AddChild(bullet);
            Shoot(bullet);
        }
            
        base.Attack(this);
    }
}