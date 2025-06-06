using System.Collections.Generic;
using Abstract.Attributes;
using Enemies;
using Godot;

namespace Turrets.Laser
{
    /// <summary>
    /// Extends DynamicTurret to add Laser functionality
    /// </summary>
    public partial class Laser : DynamicTurret
    {
        [Export]
        private Line2D line;
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
        public double DurationCountdown;
        /// <summary> How long left until the next attack </summary>
        public double CooldownCountdown;

        public Laser()
        {
            Stats[AttributeType.LaserDuration] = new Attribute(AttributeType.LaserDuration, 1f);
            Stats[AttributeType.LaserCooldown] = new Attribute(AttributeType.LaserCooldown, 1f);
        }

        /// <summary>
        /// Fires the laser if the turret have a target and are looking at them.
        /// Otherwise, rotate to target if there is one.
        /// </summary>
        public override void _PhysicsProcess(double delta)
        {
            DurationCountdown -= delta;
            
            if (CooldownCountdown >= 0 && DurationCountdown <= 0)
            {
                CooldownCountdown -= delta;

                if (TargetEnemy is not null)
                    LookAtTarget(delta);
                
                line.Visible = false;
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
            if (DurationCountdown <= 0)
            {
                if (IsLookingAtTarget())
                {
                    DurationCountdown = Stats[AttributeType.LaserDuration].Value;
                    CooldownCountdown = Stats[AttributeType.LaserCooldown].Value;
                }

                return;
            }
            
            Attack((float)delta);
        }
        
        /// <summary>
        /// Fires the laser towards the enemy and deals damage
        /// </summary>
        // TODO - Animate the laser slightly (make it pulse)
        protected override void Attack(float delta)
        {
            // Get all enemies the laser hits
            // var results = new List<Collider2D>();
            // Physics2D.OverlapCapsule(direction/2 + triangleCentre, new Vector2(lineRenderer.endWidth, range.GetStat()), 
                // CapsuleDirection2D.Vertical, transform.Rotation.Y, new ContactFilter2D().NoFilter(), results);
            // List<Enemy> enemies = results.Select(result => result.transform.GetComponent<Enemy>()).ToList();
            // enemies.RemoveAll(x => x == null);

            var enemies = new List<Enemy>();

            while (Ray.GetCollider() != null)
            {
                Ray.AddException((CollisionObject2D)Ray.GetCollider());
                if (Ray.GetCollider() is Enemy enemy)
                {
                    Ray.ForceRaycastUpdate();
                    enemies.Add(enemy);
                }
            }
            
            Ray.ClearExceptions();
            
            base.Attack(this);
            HitMany(enemies, this);
            
            // Deal damage to every enemy hit
            foreach (Enemy enemy in enemies)
            {
                enemy.TakeDamage(Stats[AttributeType.Damage].Value * delta, this);
            }

            // Enable visuals
            if (!line.Visible)
            {
                line.Visible = true;
                // impactEffect.Play();
            }

            // Set impact effect rotation
            // Transform impactEffectTransform = impactEffect.transform;
            // var aimDir = (Vector2)((Vector2)firePointPosition - (Vector2)impactEffectTransform.position).normalized;
            // impactEffectTransform.Rotation = Quaternion.LookRotation(aimDir);

            // Set impact effect position
            //impactEffectTransform.position = endPosition + aimDir * 0.2f;
        }
    }
}
