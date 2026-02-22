using BestagonDefence.Abstract.Attributes;
using Godot;

namespace BestagonDefence.Turrets.Seeker;

/// <summary>
/// Extends dynamic turret to add Seeking functionality
/// </summary>
public partial class Seeker : DynamicTurret
{
    /// <summary>
    /// The seeker shot to spawn
    /// </summary>
    [Export]
    private PackedScene shot;

    private int shotCount;
    
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
            
            
        if (FireCountdown <= 0 && shotCount < Stats[AttributeType.SeekerCount].Value)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            Attack((float) delta);
        }
            
        FireCountdown -= delta;
    }
    
    /// <summary>
    /// Spawns a new seeker
    /// </summary>
    /// <param name="delta">The time since the last frame</param>
    protected override void Attack(float delta)
    {
        // TODO - Attack effect
        // attackEffect.SetFloat("zRotation", -firePoint.Rotation.eulerAngles.z);
        // attackEffect.Play();
            
        // Creates the bullet
        var ship = (Bullet)shot.Instantiate();
        ship.Stats[AttributeType.Damage] = Stats[AttributeType.Damage];
        ship.Area.GlobalPosition = FirePoint.GlobalPosition;
        ship.Area.Rotation = PartToRotate.Rotation;
        ship.Seek(TargetEnemy, this);
        GetTree().Root.AddChild(ship);
        shotCount++;
        Shoot(ship);

        base.Attack(this);
    }

    public void RemoveShot()
    {
        shotCount--;
    }
}