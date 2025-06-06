using Abstract.Attributes;
using Godot;

namespace Turrets.Hunter
{
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
        /// The effect to fire when the bullet is shot
        /// </summary>
        // [Export]
        // private VisualEffect attackEffect;
        public override void _Ready()
        {
            base._Ready();

            ClearLine();
        }

        /// <summary>
        /// Rotates towards the target if the turret have one.
        /// Shoots if the turret is looking towards the target
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

        private void UpdateLine()
        {
            scopeLine.RemovePoint(1);
            scopeLine.AddPoint(new Vector2(0, -TargetEnemy.GlobalPosition.DistanceTo(GlobalPosition) / GlobalScale.Y));
            var widthMult = (float)(FireCountdown / (1f / Stats[AttributeType.FireRate].Value));
            scopeLine.WidthCurve.SetPointValue(1, (1f - scopeLine.WidthCurve.Sample(0)) * widthMult);
        }

        private void ClearLine()
        {
            scopeLine.ClearPoints();
            scopeLine.AddPoint(new Vector2(0, 0));
            scopeLine.AddPoint(new Vector2(0, 0));
        }

        /// <summary>
        /// Create the bullet and give it a target
        /// </summary>
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
}
