using Abstract.Attributes;
using Godot;

namespace Turrets.Shooter
{
    /// <summary>
    /// Extends DynamicTurret to add Shooting functionality.
    /// </summary>
    public partial class Shooter : DynamicTurret
    {
        /// <summary>
        /// The bullet prefab to spawn each attack
        /// </summary>
        [Export]
        private PackedScene bulletPrefab;

        /// <summary>
        /// The effect to fire when the bullet is shot
        /// </summary>
        // [Export]
        // private VisualEffect attackEffect;

        /// <summary>
        /// Rotates towards the target if the turret have one.
        /// Shoots if the turret is looking towards the target
        /// </summary>
        public override void _PhysicsProcess(double delta)
        {
            base._Process(delta);
            
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
                Attack();
            }
            
            FireCountdown -= delta;
        }

        /// <summary>
        /// Create the bullet and give it a target
        /// </summary>
        protected override void Attack()
        {
            // TODO - Attack effect
            // attackEffect.SetFloat("zRotation", -firePoint.Rotation.eulerAngles.z);
            // attackEffect.Play();
            
            // Creates the bullet
            var bullet = (Bullet)bulletPrefab.Instantiate();
            bullet.Stats[AttributeType.Damage] = Stats[AttributeType.Damage];
            bullet.GlobalPosition = FirePoint.GlobalPosition;
            bullet.Name = "_" + bullet.Name;
            bullet.Seek(TargetEnemy, this);
            GetTree().Root.AddChild(bullet);
            Shoot(bullet);

            base.Attack(this);
        }
    }
}
