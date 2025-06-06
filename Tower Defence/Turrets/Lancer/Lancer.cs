using Abstract.Attributes;
using Godot;

namespace Turrets.Lancer
{
    /// <summary>
    /// Extends Turret to add lancing functionality
    /// </summary>
    public partial class Lancer : Turret
    {
        /// <summary>
        /// The bullet prefab to spawn each attack
        /// </summary>
        [Export]
        private PackedScene bulletPrefab;
        // <summary>
        // The effect to fire when the bullet is shot
        // </summary>
        // [Export]
        // private VisualEffect attackEffect;

        [Export]
        private RayCast2D _ray;
        
        // Reference
        /// <summary>
        /// The transform at which attack from (e.g. instantiate bullets)
        /// </summary>
        [Export]
        private Node2D firePoint;
        /// <summary>
        /// The part to rotate
        /// </summary>
        [Export]
        public Node2D PartToRotate;

        public Lancer()
        {
            Stats[AttributeType.BulletRange] = new Attribute(AttributeType.BulletRange, 3f);
        }

        /// <summary>
        /// Check for new enemies in attack range
        /// </summary>
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            
            // Don't do anything if no enemy is in range
            if (_ray.GetCollider() == null)
            {
                FireCountdown -= delta;
                return;
            }
            
            // If our attack is off cooldown
            if (FireCountdown <= 0)
            {
                FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
                Attack((float) delta);
            }
            
            FireCountdown -= delta;
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack(float delta)
        {
            // attackEffect.Play();
            // Creates the bullet
            var bullet = bulletPrefab.Instantiate<Bullet>();
            bullet.Position = firePoint.GlobalPosition;
            bullet.Rotation = firePoint.GlobalRotation;
            bullet.Name = "_" + bullet.Name;
            
            base.Attack(this);
            
            // Get the end point of the line renderer
            Vector2 direction = new Vector2(_ray.TargetPosition.Y, -_ray.TargetPosition.X) * Stats[AttributeType.BulletRange].Value;
            Vector2 target = firePoint.Position + direction;
            
            bullet.Seek(ToGlobal(target), this);
            
            Shoot(bullet);
            GetTree().Root.AddChild(bullet);
        }
    }
}
