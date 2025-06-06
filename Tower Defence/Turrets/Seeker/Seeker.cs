using Abstract.Attributes;
using Godot;

namespace Turrets.Seeker;

public partial class Seeker : DynamicTurret
{
    [Export]
    private PackedScene shot;
    
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
        Shoot(ship);

        base.Attack(this);
    }
}