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
        // [Export]
        // <summary>
        // The particle effect that's spawned at the end of the laser's line
        // </summary>
        // private ParticleSystem impactEffect;

        /// <summary> How long left until the next attack </summary>
        private double _durationCountdown;
        /// <summary> How long left until the next attack </summary>
        private double _cooldownCountdown;

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
            _durationCountdown -= delta;
            
            if (_cooldownCountdown >= 0 && _durationCountdown <= 0)
            {
                _cooldownCountdown -= delta;

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
            if (_durationCountdown <= 0)
            {
                if (IsLookingAtTarget())
                {
                    _durationCountdown = Stats[AttributeType.LaserDuration].Value;
                    _cooldownCountdown = Stats[AttributeType.LaserCooldown].Value;
                }

                return;
            }
            
            Attack((float)delta);
        }
        
        /// <summary>
        /// Fires the laser towards the enemy and deals damage
        /// </summary>
        protected override void Attack(float delta)
        {
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
