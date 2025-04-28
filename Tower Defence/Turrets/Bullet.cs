using System.Collections.Generic;
using Abstract.Attributes;
using Enemies;
using Godot;

namespace Turrets
{
    /// <summary>
    /// The bullet shot from a turret
    /// </summary>
    public partial class Bullet : Area2D
    {
        [Export]
        public Attributes Stats = new(
            new Godot.Collections.Dictionary<AttributeType, Attribute> { 
                [AttributeType.Speed] = new(AttributeType.Speed, 30f),
                [AttributeType.ExplosionRadius] = new(AttributeType.ExplosionRadius, 0f, min:0f),
                [AttributeType.Knockback] = new(AttributeType.Knockback, 0f, min:0f),
                [AttributeType.Damage] = new(AttributeType.Damage, 5f),
            });

        
        [Export]
        public Turret Source;
        public Enemy Target;
        public Vector2 TargetLocation;
        public bool UseLocation;

        // TODO - Convert to Enum?
        /// <summary>
        /// Hits all enemies on path
        /// </summary>
        [ExportGroup("Types")]
        // TODO - Implement isEtheral
        [Export]
        public bool IsEthereal;
        /// <summary>
        /// Hits the first enemy it touches, rather than just target
        /// </summary>
        [Export]
        // TODO - Implement willHitFirst
        public bool WillHitFirst;
    
        /// <summary>
        /// The effect spawned when the bullet hits a target
        /// </summary>
        [Export]
        private PackedScene _impactEffect;
        /// <summary>
        /// Explosion effect
        /// </summary>
        [Export]
        private PackedScene _explodeEffect;
        /// <summary>
        /// Explosion collision shape
        /// </summary>
        // [Export]
        // private CollisionShape2D explodeArea;
        
        private readonly List<ulong> _hitEnemies = [];
        
        /// <summary>
        /// Sets the new transform the bullet shoot go towards
        /// </summary>
        /// <param name="newTarget">The transform of the new target</param>
        /// <param name="turret">The turret telling the bullet to seek a target</param>
        public void Seek(Enemy newTarget, Turret turret)
        {
            Target = newTarget;
            Source = turret;
            UseLocation = false;
            Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
        }
        
        /// <summary>
        /// Sets the location the bullet goes towards
        /// </summary>
        /// <param name="location">The location the bullet goes towards</param>
        /// <param name="turret">The turret telling the bullet to seek the location</param>
        public void Seek(Vector2 location, Turret turret)
        {
            TargetLocation = location;
            Source = turret;
            UseLocation = true;
            Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
        }

        /// <summary>
        /// Moves the bullet towards the target and check if it hits
        /// </summary>
        public override void _Process(double delta)
        {
            // Check the bullet still have a target to move towards
            if ((!IsInstanceValid(Target)) && !UseLocation)
                QueueFree();
            else if (UseLocation)
                SeekTarget(TargetLocation, false, delta);
            else
                SeekTarget(Target.Position, true, delta);
        }

        /// <summary>
        /// Moves the bullet towards a target location
        /// </summary>
        /// <param name="location">The location to move towards</param>
        /// <param name="isEnemy">If the location is an enemy</param>
        /// <param name="delta">The length of the frame</param>
        private void SeekTarget(Vector2 location, bool isEnemy, double delta)
        {
            // Get the direction of the target, and the distance to move this frame
            Vector2 position = Position;
            var distanceThisFrame = (float)(Stats[AttributeType.Speed].Value * delta);
            
            // Move bullet towards target
            Position = Position.MoveToward(location, distanceThisFrame);
            
            Vector2 difference = location - Position;
            const float targetSize = 0.25f;
            // Has the bullet "hit" the target?
            if (difference.LengthSquared() <= targetSize * targetSize)
            {
                HitTarget(isEnemy); 
                return;
            }
            
            // Rotate to target
            Rotation = (location - position).Normalized().Angle();
        }

        /// <summary>
        /// Called when the bullet hits the target
        /// </summary>
        private void HitTarget(bool isEnemy, Enemy enemy = null)
        {
            enemy ??= Target;
            
            // TODO - Impact Effects
            
            // var effect = impactEffect.Instantiate<Node2D>();
            // effect.Name = "_" + effect.Name;
            // effect.Position = Position;
            // effect.Rotation = Rotation;
            //
            // GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };

            if (isEnemy)
            {
                // If the bullet has AoE damage or not
                if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                    Explode();
                else
                    Damage(enemy);
            }
            else
            {
                if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                    Explode();
            }

            // Destroy so the bullet only hits the target once
            QueueFree();
        }

    
        /// <summary>
        /// Used to deal damage to a single enemy
        /// </summary>
        /// <param name="enemy">The enemy to deal damage to</param>
        private void Damage(Enemy enemy)
        {
            if (enemy == null) return;
            
            Source.Hit(enemy, Source, this);

            if (Stats[AttributeType.Knockback].Value > 0)
            {
                // TODO - Get Movement
                // em.GetComponent<EnemyMovement>().TakeKnockback(knockbackAmount.GetTrueStat(), source.Position);
            }

            enemy.TakeDamage(Stats[AttributeType.Damage].Value, this);
        }
    
        /// <summary>
        /// Used to deal damage to multiple enemies
        /// </summary>
        private void Explode()
        {
            if (_explodeEffect is not null)
            {
                // Spawn explode effect
                
                var effect = _explodeEffect.Instantiate<Node2D>();
                effect.Name = "_" + effect.Name;
                effect.Position = Position;
                effect.Rotation = Rotation;

                GetTree().CreateTimer(1).Timeout += () => { effect.QueueFree(); };
                
                // GodotObject effectIns = Instantiate(explodeEffect, Position, transform.Rotation);
                // var visualEffect = effectIns.GetComponent<VisualEffect>();
                // visualEffect.SetFloat("size", explosionRadius.GetStat() * 2.5f);
                // visualEffect.Play();
            }

            // TODO - What if not a circle?
            // ((CircleShape2D)explodeArea.Shape).Radius *= Stats[AttributeType.ExplosionRadius].Value;

            // Gets all the enemies in the AoE and calls Damage on them
            foreach (Area2D area in GetOverlappingAreas())
            {
                var enemy = (Enemy)area;
                Damage(enemy);
            }
        }
        
        /// <summary>
        /// Deals damage to hit enemies the first time when the bullet should
        /// </summary>
        /// <param name="col">The collider that was touched</param>
        private void OnAreaEntered(Area2D col)
        {
            if (col is not Enemy enemy) return;

            if (_hitEnemies.Contains(col.GetInstanceId())) return;
            
            _hitEnemies.Add(col.GetInstanceId());

            if (Target != null && Target.GetInstanceId() == col.GetInstanceId())
            {
                HitTarget(true, enemy);
                return;
            }

            if (IsEthereal)
            {
                if (!WillHitFirst)
                    Damage(Target);
                else
                    HitTarget(true, enemy);
            }
        }
    }
}
