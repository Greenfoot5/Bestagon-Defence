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
        private Line2D line;

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
        public override void _Process(double delta)
        {
            base._Process(delta);
        
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
                Attack();
            }
        }

        private void UpdateLine()
        {
            line.RemovePoint(1);
            line.AddPoint(new Vector2(0, -TargetEnemy.GlobalPosition.DistanceTo(GlobalPosition) / GlobalScale.Y));
            var widthMult = (float)(FireCountdown / (1f / Stats[AttributeType.FireRate].Value));
            line.WidthCurve.SetPointValue(1, (1f - line.WidthCurve.Sample(0)) * widthMult);
        }

        private void ClearLine()
        {
            line.ClearPoints();
            line.AddPoint(new Vector2(0, 0));
            line.AddPoint(new Vector2(0, 0));
        }

        /// <summary>
        /// Create the bullet and give it a target
        /// </summary>
        protected override void Attack()
        {
            // TODO - Attack effect
            // attackEffect.SetFloat("zRotation", -firePoint.Rotation.eulerAngles.z);
            // attackEffect.Play();
            
            TargetEnemy.TakeDamage(Stats[AttributeType.Damage].Value, this);

            base.Attack(this);
        }
    }
}
