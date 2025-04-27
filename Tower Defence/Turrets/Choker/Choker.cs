using Abstract.Attributes;
using Godot;

namespace Turrets.Choker
{
    /// <summary>
    /// Extends DynamicTurret to add Shooting functionality.
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
            Stats[AttributeType.PartSpread] = new Attribute(0.5f, min: 0f);
            Stats[AttributeType.PartCount] = new Attribute(10f, min: 0f);
        }

        /// <summary>
        /// Rotates towards the target if the turret have one.
        /// Shoots if the turret is looking towards the target
        /// </summary>
        public override void _Process(double delta)
        {
            Update();
            
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
            //attackEffect?.Play();

            // Any decimal count is a chance for an additional part
            int count = ((int)Stats[AttributeType.PartCount].Value) + (GD.Randf() < (Stats[AttributeType.PartCount].Value % 1f) ? 1 : 0);
            
            // float oneSegment = spreadSize / (spreadAmount - 1);
            for (var i = 0; i < count; i++)
            {
                var bullet = _bulletPrefab.Instantiate<Bullet>();
                bullet.Position = FirePoint.Position;
                bullet.Rotation = FirePoint.Rotation;
                bullet.Name = "_" + bullet.Name;
                
                // TODO - Angles
                // float bulletAngle = firePoint.eulerAngles.z + Random.Range(partSpread.GetStat() / -2, partSpread.GetStat() / 2);
                // bullet.eulerAngles = new Vector2(0, 0, bulletAngle);

                // float radian = (bulletAngle + 90) * Mathf.Deg2Rad;
                // var bulletDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
                //bullet.position = 
                // bullet.Seek((Vector2)firePoint.Position + bulletDirection * range.GetStat(), this);
                Shoot(bullet);
            }
            
            base.Attack(this);
        }
    }
}
