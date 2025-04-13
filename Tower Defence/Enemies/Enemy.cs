using System.Collections.Generic;
using Abstract;
using Abstract.Attributes;
using Gameplay;
using Godot;
using Levels._Nodes;

namespace Enemies
{
    /// <summary>
    /// The base skeleton for the enemy, holding its stats and abilities
    /// </summary>
    public partial class Enemy : Area2D
    {
        [Export]
        public Attributes Stats = new(
            new Godot.Collections.Dictionary<AttributeType, Attribute> { 
                [AttributeType.Speed] = new(2f, min:0.8f),
                [AttributeType.MaxHealth] = new(20f),
            });
        
        public float Health { get; private set; }
    
        /// <summary>
        /// The amount of money to grant the player when the enemy is killed
        /// </summary>
        [ExportGroup("Death Stats")]
        [Export]
        public int DeathMoney = 10;
        /// <summary>
        /// The amount of lives lost if the enemy finishes the path
        /// </summary>
        [Export]
        public int DeathLives = 1;
        /// <summary>
        /// The amount of money to grant the player if the enemy finishes the path
        /// </summary>
        [Export]
        public int EndPathMoney = 10;

        /// <summary>
        /// The left health bar
        /// </summary>
        [ExportGroup("Health Bar")]
        [Export]
        public ProgressBar LeftBar;
        /// <summary>
        /// The right health bar
        /// </summary>
        [Export]
        public ProgressBar RightBar;

        /// <summary>
        /// The root game object to rotate to change the enemy's looking direction
        /// </summary>
        [ExportGroup("Other")]
        [Export]
        public Node2D RotationRoot;
        /// <summary>
        /// If the enemy rotates towards the next waypoint
        /// </summary>
        [Export]
        public bool DoesRotation = true;
        /// <summary>
        /// The particle effect prefab to spawn when the enemy dies
        /// </summary>
        [Export]
        public PackedScene DeathEffect;
        /// <summary>
        /// The radius of a circle the bullet can collide with this target
        /// </summary>
        [Export]
        public float HitboxSize = 0.25f;

        /// <summary>
        /// If the enemy is a boss
        /// </summary>
        [ExportGroup("Bosses")]
        [Export]
        // TODO - Maybe put in immunities?
        public bool IsBoss;
        
        /// <summary>
        /// The next position the enemy moves towards
        /// </summary>
        [ExportGroup("Movement")]
        [Export]
        private Node2D _target;
        public int waypointIndex;
        
        /// <summary>
        /// The distance from enemy to waypoint before it's considered reached
        /// </summary>
        [Export] 
        private float distanceToWaypoint = 0.05f;
        
        /// <summary>
        /// How many waypoints the enemy has passed, and the percentage to the next one
        /// </summary>
        public float mapProgress;
        private float _maxDistance;
        
        /// <summary>
        /// A list of the effect names (internal names) that the enemy is immune to
        /// 
        /// During runtime, also contains any unique effects applied to the enemy as they are immune to it
        /// </summary>
        [ExportGroup("Effect Immunities")]
        // TODO - List Export
        // [Export]
        public List<string> UniqueEffects;
        public readonly Dictionary<string, EnemyEffect> ActiveEffects = new();

        // If the enemy has died
        private bool _isDead;
        public delegate void DeathEvent();
        public event DeathEvent OnDeath;
        
        /// <summary>
        /// Initialises relevant variables
        /// </summary>
        public override void _Ready()
        {
            Health = Stats[AttributeType.MaxHealth].Value;
            _target = Waypoints.points[waypointIndex];
        }


        public override void _Process(double delta)
        {
            // If the enemy is moving backwards
            if (Stats[AttributeType.Speed].GetTrueValue() < 0)
            {
                MoveBackwards();
                return;
            }
            
            // Get the direction of the target, and the distance to move this frame
            Vector2 position = Position;
            Vector2 location = _target.Position;
            var distanceThisFrame = (float)(Stats[AttributeType.Speed].Value * delta);

            Position = position.MoveToward(location, distanceThisFrame);
            
            Vector2 difference = location - position; // Distance & direction to next target

            // If within this frame the enemy will pass the waypoint, it's a guaranteed hit
            if (difference.LengthSquared() <= distanceToWaypoint * distanceToWaypoint)
            {
                GetNextWaypoint();
            }
            else
            {
                float sqrDistance = (Position - _target.Position).LengthSquared();
                mapProgress = waypointIndex + 1 - (sqrDistance / (_maxDistance * _maxDistance));
            }

            if (DoesRotation) { }
            // Attempt at rotation
            // TODO - Transform.up
            // _enemy.RotationRoot.transform.up = (location - position).normalized;
        }
        
        /// <summary>
        /// Gets the next waypoint in the waypoints array
        /// </summary>
        private void GetNextWaypoint()
        {
            // If the enemy has reached the end, destroy
            if (waypointIndex >= Waypoints.points.Length - 1)
            {
                FinishPath();
                return;
            }
        
            // Get the next waypoint
            waypointIndex++;
            _target = Waypoints.points[waypointIndex];
            mapProgress = waypointIndex;
            _maxDistance = Position.DistanceTo(_target.Position);
        }
        
        /// <summary>
        /// Moves the enemy backwards along the path.
        /// </summary>
        private void MoveBackwards()
        {
            // If the enemy has reached the start, we can't go backwards further
            if (waypointIndex - 1 < 0)
            {
                return;
            }
                
            // Get the direction and move in that direction
            Vector2 dir = Waypoints.points[waypointIndex - 1].Position - Position;
            // TODO - Perform translation in godot
            // transform.Translate(dir.normalized * (Mathf.Abs(_enemy.speed.GetTrueStat()) * delta), Space.World);
            mapProgress = waypointIndex - (distanceToWaypoint / _maxDistance);
        
            // If the enemy hasn't reached the previous waypoint, there's no point knocking it back further
            if (!(Position.DistanceTo(Waypoints.points[waypointIndex - 1].Position) <= distanceToWaypoint)) return;

            // Get the next waypoint
            waypointIndex--;
            _target = Waypoints.points[waypointIndex];
            mapProgress = waypointIndex;
            _maxDistance = _target.Position.DistanceTo(Waypoints.points[waypointIndex + 1].Position);
        }
        
        /// <summary>
        /// Called when a turret wants to deal knockback to an enemy
        /// </summary>
        /// <param name="amount">The amount of knockback to deal</param>
        /// <param name="turretLocation">The location of the turret</param>
        public void TakeKnockback(float amount, Vector2 turretLocation)
        {
            if (Stats[AttributeType.KnockbackResistance].Value <= 0)
            {
                return;
            }
            
            Vector2 v = _target.Position - Position;
            Vector2 w = turretLocation - Position;
            float multiplier = v.Normalized().Dot(w.Normalized());
            
            // Actually deal knockback
            // Multiply by -1 to knock backwards
            float knockback = amount * Stats[AttributeType.KnockbackResistance].Value * multiplier * -1;
            Variant uid = GD.Randi();
            Stats[AttributeType.Speed].Add(uid, new AttributeModifier(knockback, Operation.Multiplicative));

            GetTree().CreateTimer(Stats[AttributeType.KnockbackDuration].Value).Timeout += () => { Stats[AttributeType.Speed].Remove(uid); };
        }
    
        /// <summary>
        /// Called whenever the enemy takes damage.
        /// This activates any ability with the OnDamage trigger
        /// </summary>
        /// <param name="amount">The amount of damage to deal</param>
        /// <param name="source">The GodotObject that hurt the enemy</param>
        public void TakeDamage(float amount, GodotObject source)
        {
            // Edit the health
            Health -= amount;

            LeftBar.Value = Health / Stats[AttributeType.MaxHealth].Value;
            RightBar.Value = Health / Stats[AttributeType.MaxHealth].Value;

            if (Health <= 0)
            {
                Die();
            } else if (Health > Stats[AttributeType.MaxHealth].Value)
            {
                Health = Stats[AttributeType.MaxHealth].Value;
            }
        }

        /// <summary>
        /// Called when the enemy dies
        /// allows the game to clean up anything when removing the GodotObject
        /// </summary>
        private void Die()
        {
            // Make sure we're not already dead.
            if (_isDead)
                return;
            _isDead = true;
            
            DeathBitManager.DropEnergy(Position, DeathMoney);

            OnDeath?.Invoke();

            // Spawn death effect
            var effect = (Node2D) DeathEffect.Instantiate();
            effect.Position = Position;
            effect.Name = "_" + effect.Name;
            GetTree().CreateTimer(5).Timeout += () => effect.QueueFree();
            
            QueueFree();
        }

        /// <summary>
        /// Called when the enemy reaches the end of the map's path
        /// Activates any finishPath abilities
        /// </summary>
        private void FinishPath()
        {
            // Let our other systems know the enemy reached the end
            GameStats.Lives -= DeathLives;
            GameStats.Energy += EndPathMoney;
        
            QueueFree();
        }
    }
}
