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
                [AttributeType.Speed] = new(30f),
                [AttributeType.ExplosionRadius] = new(0f, min:0f),
                [AttributeType.Knockback] = new(0f, min:0f),
                [AttributeType.Damage] = new(5f),
            });

        
        [Export]
        public Turret source;
        public Enemy target;
        public Vector2 targetLocation;
        public bool useLocation;

        // TODO - Convert to Enum?
        /// <summary>
        /// Hits all enemies on path
        /// </summary>
        [ExportGroup("Types")]
        [Export]
        public bool isEthereal;
        /// <summary>
        /// Hits the first enemy it touches, rather than just target
        /// </summary>
        [Export]
        public bool willHitFirst;
    
        /// <summary>
        /// The effect spawned when the bullet hits a target
        /// </summary>
        [Export]
        private PackedScene impactEffect;
        /// <summary>
        /// Explosion effect
        /// </summary>
        [Export]
        private PackedScene explodeEffect;
        /// <summary>
        /// Explosion collision shape
        /// </summary>
        // [Export]
        // private CollisionShape2D explodeArea;
        
        private readonly List<ulong> _hitEnemies = new();
        
        /// <summary>
        /// Sets the new transform the bullet shoot go towards
        /// </summary>
        /// <param name="newTarget">The transform of the new target</param>
        /// <param name="turret">The turret telling the bullet to seek a target</param>
        public void Seek(Enemy newTarget, Turret turret)
        {
            target = newTarget;
            source = turret;
            useLocation = false;
            Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
        }
        
        /// <summary>
        /// Sets the location the bullet goes towards
        /// </summary>
        /// <param name="location">The location the bullet goes towards</param>
        /// <param name="turret">The turret telling the bullet to seek the location</param>
        public void Seek(Vector2 location, Turret turret)
        {
            targetLocation = location;
            source = turret;
            useLocation = true;
            Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
        }

        /// <summary>
        /// Moves the bullet towards the target and check if it hits
        /// </summary>
        public override void _Process(double delta)
        {
            // Check the bullet still have a target to move towards
            if (target == null && !useLocation)
                QueueFree();
            else if (useLocation)
                SeekTarget(targetLocation, false, delta);
            else
                SeekTarget(target.Position, true, delta);
        }
        
        /// <summary>
        /// Moves the bullet towards a target location
        /// </summary>
        /// <param name="location">The location to move towards</param>
        /// <param name="isEnemy">If the location is an enemy</param>
        private void SeekTarget(Vector2 location, bool isEnemy, double delta)
        {
            // Get the direction of the target, and the distance to move this frame
            Vector2 position = Position;
            var distanceThisFrame = (float)(Stats[AttributeType.Speed].Value * delta);
            GD.Print(Stats[AttributeType.Speed]);
            
            // Move bullet towards target
            Position = Position.MoveToward(location, distanceThisFrame);

            Vector2 difference = location - position;
            
            // Has the bullet "hit" the target?
            if (difference.LengthSquared() <= target.Stats.HitboxSize * target.Stats.HitboxSize)
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
            enemy ??= target;
            
            var effect = impactEffect.Instantiate<Node2D>();
            effect.Name = "_" + effect.Name;
            effect.Position = Position;
            effect.Rotation = Rotation;

            GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };

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
            
            source.Hit(enemy, source, this);

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
            if (explodeEffect is not null)
            {
                // Spawn explode effect
                
                var effect = explodeEffect.Instantiate<Node2D>();
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
            // TODO - OverlapCircleAll
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
            if (!(isEthereal || willHitFirst) || col is not Enemy enemy) return;

            if (_hitEnemies.Contains(col.GetInstanceId())) return;
            
            _hitEnemies.Add(col.GetInstanceId());

            if (target != null && target.GetInstanceId() == col.GetInstanceId()) return;

            if (!willHitFirst)
                Damage(target);
            else
                HitTarget(true, enemy);
        }
    }
}
