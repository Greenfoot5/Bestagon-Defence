using Abstract.Attributes;
using Godot;

namespace Turrets.Laser
{
    /// <summary>
    /// Extends DynamicTurret to add Laser functionality
    /// </summary>
    // TODO - Make work
    public partial class Laser : DynamicTurret
    {
        // Lasers
        // <summary>
        // The line renderer that displays the laser
        // </summary>
        // [Export]
        // private LineRenderer lineRenderer;
        // [Export]
        // <summary>
        // The particle effect that's spawned at the end of the laser's line
        // </summary>
        // private ParticleSystem impactEffect;

        /// <summary> How long left until the next attack </summary>
        public double durationCountdown;
        /// <summary> How long left until the next attack </summary>
        public double cooldownCountdown;

        public Laser()
        {
            Stats[AttributeType.LaserDuration] = new Attribute(AttributeType.LaserDuration, 1f);
            Stats[AttributeType.LaserCooldown] = new Attribute(AttributeType.LaserCooldown, 1f);
        }

        /// <summary>
        /// Fires the laser if the turret have a target and are looking at them.
        /// Otherwise rotate to target if there is one.
        /// </summary>
        public override void _Process(double delta)
        {
            durationCountdown -= delta;
            
            if (cooldownCountdown >= 0 && durationCountdown <= 0)
            {
                cooldownCountdown -= delta;

                if (TargetEnemy is not null)
                    LookAtTarget(delta);
                
                // if (!lineRenderer.enabled) return;
                
                // lineRenderer.enabled = false;
                // impactEffect.Stop();
                return;
            }
            
            // Don't do anything if the turret doesn't have a target
            // or fire rate is <= 0
            if (TargetEnemy is null || Stats[AttributeType.LaserDuration].Value < 0)
            {
                return;
            }
        
            // Rotates the turret each frame
            LookAtTarget(delta);
            
            // One of the two laser timers expired
            if (durationCountdown <= 0)
            {
                if (IsLookingAtTarget())
                {
                    durationCountdown = Stats[AttributeType.LaserDuration].Value;
                    cooldownCountdown = Stats[AttributeType.LaserCooldown].Value;
                }

                return;
            }
            
            Attack();
        }
        
        /// <summary>
        /// Fires the laser towards the enemy and deals damage
        /// </summary>
        // TODO - Animate the laser slightly (make it pulse)
        protected override void Attack()
        {
            Vector2 triangleCentre = Position;
            
            // Get the end point of the line renderer
            // Vector2 direction = (firePoint.up * range.GetStat());
            // Vector2 endPosition = (direction + triangleCentre);
            
            // Get all enemies the laser hits
            // var results = new List<Collider2D>();
            // Physics2D.OverlapCapsule(direction/2 + triangleCentre, new Vector2(lineRenderer.endWidth, range.GetStat()), 
                // CapsuleDirection2D.Vertical, transform.Rotation.Y, new ContactFilter2D().NoFilter(), results);
            // List<Enemy> enemies = results.Select(result => result.transform.GetComponent<Enemy>()).ToList();
            // enemies.RemoveAll(x => x == null);
            
            base.Attack(this);
            // HitMany(enemies, this);
            
            // Deal damage to every enemy hit
            // foreach (Enemy enemy in enemies)
            // {
                // enemy.TakeDamage(damage.GetStat() * delta, this);
            // }

            // Enable visuals
            // if (!lineRenderer.enabled)
            // {
                // lineRenderer.enabled = true;
                // impactEffect.Play();
            // }
            
            // Set Laser positions
            Vector2 firePointPosition = Position;
            // lineRenderer.SetPosition(0, firePointPosition);
            // lineRenderer.SetPosition(1, endPosition);
            
            // Set impact effect rotation
            // Transform impactEffectTransform = impactEffect.transform;
            // var aimDir = (Vector2)((Vector2)firePointPosition - (Vector2)impactEffectTransform.position).normalized;
            // impactEffectTransform.Rotation = Quaternion.LookRotation(aimDir);
            
            // Set impact effect position
            //impactEffectTransform.position = endPosition + aimDir * 0.2f;
        }
    }
}
