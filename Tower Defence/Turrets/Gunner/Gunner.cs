using Abstract.Attributes;
using Godot;
using Levels._Nodes;

namespace Turrets.Gunner;

/// <summary>
/// Extends DynamicTurret to add Shooting functionality.
/// </summary>
public partial class Gunner : DynamicTurret
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
        
    // Spin up stats
    private float _fireRateIncrease = 1f;

    public Gunner()
    {
        Stats[AttributeType.SpinMultiplier] = new Attribute(AttributeType.SpinMultiplier, 1.1f);
        Stats[AttributeType.SpinCooldown] = new Attribute(AttributeType.SpinCooldown, 1.08f);
    }

    /// <summary>
    /// Rotates towards the target if the turret have one.
    /// Shoots if the turret is looking towards the target
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (FireCountdown > 1 / Stats[AttributeType.FireRate].Value)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
        }
            
        // If there's no fire rate, the turret shouldn't do anything
        // However, it should rapidly cool down
        if (Stats[AttributeType.FireRate].Value == 0)
        {
            UpdateFireRate(false);
            return;
        }
            
        // Don't do anything if the turret doesn't have a target
        if (TargetEnemy is null)
        {
            if (FireCountdown <= 0f)
            {
                UpdateFireRate(false);
                FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            }
                
            FireCountdown -= delta;

            return;
        }
        
        // Rotates the turret each frame
        LookAtTarget(delta);

        if (!IsLookingAtTarget())
        {
            if (FireCountdown <= 0f)
            {
                UpdateFireRate(false);
                FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            }

            FireCountdown -= delta;
            return;
        }
            
            
        if (FireCountdown <= 0)
        {
            UpdateFireRate(true);
                
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
                
            Attack((float) delta);
        }

        FireCountdown -= delta;
    }
        
    /// <summary>
    /// Updates the fire rate so there's no duplicated code in Attack().
    /// Also handles all edge cases with the increase being too low or high
    /// </summary>
    /// <param name="isIncrease">To increase or decrease the fireRate</param>
    private void UpdateFireRate(bool isIncrease)
    {
        if (isIncrease)
        {
            _fireRateIncrease += Stats[AttributeType.SpinMultiplier].Value;
        }
        else
        {
            _fireRateIncrease -= Stats[AttributeType.SpinCooldown].Value;
        }

        _fireRateIncrease = Mathf.Clamp(_fireRateIncrease, 0f, 6f);
            
        Stats[AttributeType.FireRate].Add("this", new AttributeModifier(_fireRateIncrease - 1, Operation.Multiplicative));

        // Update the stats of the turret if it's selected
        if (BuildableTile.SelectedTile == GetParent())
        {
            BuildableTile.SelectedTile = BuildableTile.SelectedTile;
        }
    }

    /// <summary>
    /// Create the bullet and give it a target
    /// </summary>
    protected override void Attack(float delta)
    {
        // Creates the bullet
        var bullet = (Bullet)_bulletPrefab.Instantiate();
        bullet.Stats[AttributeType.Damage] = Stats[AttributeType.Damage];
        bullet.GlobalPosition = FirePoint.GlobalPosition;
        bullet.Area.Rotation = FirePoint.GlobalRotation;
        bullet.Name = "_" + bullet.Name;
        bullet.Seek(TargetEnemy, this);
        GetTree().Root.AddChild(bullet);
        Shoot(bullet);

        base.Attack(this);
    }
}