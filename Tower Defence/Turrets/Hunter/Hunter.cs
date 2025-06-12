using BestagonDefense.Abstract.Attributes;
using Godot;

namespace BestagonDefense.Turrets.Hunter;

/// <summary>
/// Extends DynamicTurret to add Shooting functionality.
/// </summary>
public partial class Hunter : DynamicTurret
{
    /// <summary>
    /// The bullet prefab to spawn each attack
    /// </summary>
    [Export]
    private Line2D scopeLine;
        
    /// <summary>
    /// The line to use to "shoot the shot"
    /// </summary>
    [Export]
    private Line2D shotLine;
    /// <summary>
    /// The animation player for the shot line
    /// </summary>
    [Export]
    private AnimationPlayer shotAnimator;

    /// <summary>
    /// Clears the line in case it was set in the editor
    /// </summary>
    public override void _Ready()
    {
        base._Ready();

        ClearLine();
    }

    /// <summary>
    /// Rotates towards the target if the turret have one.
    /// Scopes in on a target
    /// Attacks target once it has scoped in enough
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        // Rotates the turret each frame
        LookAtTarget(delta);

        if (IsLookingAtTarget())
        {
            FireCountdown -= delta;
            UpdateLine();
        }
        else
        {
            ClearLine();
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
        }
            
        if (FireCountdown <= 0)
        {
            FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
            Attack((float) delta);
        }
    }

    /// <summary>
    /// Updates the Scope Line2D points
    /// </summary>
    private void UpdateLine()
    {
        scopeLine.RemovePoint(1);
        scopeLine.AddPoint(new Vector2(0, -TargetEnemy.GlobalPosition.DistanceTo(GlobalPosition) / GlobalScale.Y));
        var widthMult = (float)(FireCountdown / (1f / Stats[AttributeType.FireRate].Value));
        scopeLine.WidthCurve.SetPointValue(1, (1f - scopeLine.WidthCurve.Sample(0)) * widthMult);
    }

    /// <summary>
    /// Clears the scope Line2D points
    /// </summary>
    private void ClearLine()
    {
        scopeLine.ClearPoints();
        scopeLine.AddPoint(new Vector2(0, 0));
        scopeLine.AddPoint(new Vector2(0, 0));
    }

    /// <summary>
    /// Perform an attack on the target
    /// </summary>
    /// <param name="delta">The time since last frame (in seconds)</param>
    protected override void Attack(float delta)
    {
        // Attack effect
        shotLine.ClearPoints();
        shotLine.AddPoint(ToLocal(TargetEnemy.GlobalPosition));
        shotLine.AddPoint(ToLocal(FirePoint.GlobalPosition));
        shotAnimator.Queue("Shot");
            
        TargetEnemy.TakeDamage(Stats[AttributeType.Damage].Value, this);

        base.Attack(this);
    }
}