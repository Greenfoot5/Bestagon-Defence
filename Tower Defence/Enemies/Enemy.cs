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
        public EnemyStats Stats = new();
        
        public float Health { get; private set; }

        /// <summary>
        /// The left health bar
        /// </summary>
        [ExportGroup("Visuals")]
        [Export]
        public ProgressBar LeftBar;
        /// <summary>
        /// The right health bar
        /// </summary>
        [Export]
        public ProgressBar RightBar;
        [Export]
        public Sprite2D sprite;

        /// <summary>
        /// The root game object to rotate to change the enemy's looking direction
        /// </summary>
        [ExportGroup("Other")]
        [Export]
        public Node2D RotationRoot;
        
        /// <summary>
        /// The next position the enemy moves towards
        /// </summary>
        [ExportGroup("Movement")]
        [Export]
        private Node2D _target;
        public int waypointIndex;
        
        /// <summary>
        /// How many waypoints the enemy has passed, and the percentage to the next one
        /// </summary>
        public float mapProgress;
        private float _maxDistance;

        // If the enemy has died
        private bool _isDead;
        public delegate void DeathEvent();
        public event DeathEvent OnDeath;
        
        /// <summary>
        /// Initialises relevant variables
        /// </summary>
        public override void _Ready()
        {
            Health = Stats.Attributes[AttributeType.MaxHealth].Value;
            _target = Waypoints.points[waypointIndex];
            sprite.Texture = Stats.sprite;
        }


        public override void _Process(double delta)
        {
            // If the enemy is moving backwards
            if (Stats.Attributes[AttributeType.Speed].GetTrueValue() < 0)
            {
                MoveBackwards();
                return;
            }
            
            // Get the direction of the target, and the distance to move this frame
            Vector2 position = Position;
            Vector2 location = _target.Position;
            var distanceThisFrame = (float)(Stats.Attributes[AttributeType.Speed].Value * delta);

            Position = position.MoveToward(location, distanceThisFrame);
            
            Vector2 difference = location - position; // Distance & direction to next target

            // If within this frame the enemy will pass the waypoint, it's a guaranteed hit
            if (difference.LengthSquared() <= Stats.distanceToWaypoint * Stats.distanceToWaypoint)
            {
                GetNextWaypoint();
            }
            else
            {
                float sqrDistance = (Position - _target.Position).LengthSquared();
                mapProgress = waypointIndex + 1 - (sqrDistance / (_maxDistance * _maxDistance));
            }

            if (Stats.DoesRotation) { }
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
            mapProgress = waypointIndex - (Stats.distanceToWaypoint / _maxDistance);
        
            // If the enemy hasn't reached the previous waypoint, there's no point knocking it back further
            if (!(Position.DistanceTo(Waypoints.points[waypointIndex - 1].Position) <= Stats.distanceToWaypoint)) return;

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
            if (Stats.Attributes[AttributeType.KnockbackResistance].Value <= 0)
            {
                return;
            }
            
            Vector2 v = _target.Position - Position;
            Vector2 w = turretLocation - Position;
            float multiplier = v.Normalized().Dot(w.Normalized());
            
            // Actually deal knockback
            // Multiply by -1 to knock backwards
            float knockback = amount * Stats.Attributes[AttributeType.KnockbackResistance].Value * multiplier * -1;
            Variant uid = GD.Randi();
            Stats.Attributes[AttributeType.Speed].Add(uid, new AttributeModifier(knockback, Operation.Multiplicative));

            GetTree().CreateTimer(Stats.Attributes[AttributeType.KnockbackDuration].Value).Timeout += () => { Stats.Attributes[AttributeType.Speed].Remove(uid); };
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

            LeftBar.Value = Health / Stats.Attributes[AttributeType.MaxHealth].Value;
            RightBar.Value = Health / Stats.Attributes[AttributeType.MaxHealth].Value;

            if (Health <= 0)
            {
                Die();
            } else if (Health > Stats.Attributes[AttributeType.MaxHealth].Value)
            {
                Health = Stats.Attributes[AttributeType.MaxHealth].Value;
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
            
            DeathBitManager.DropEnergy(Position, Stats.DeathMoney);

            OnDeath?.Invoke();

            // Spawn death effect
            var effect = (Node2D) Stats.DeathEffect.Instantiate();
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
            GameStats.Lives -= Stats.DeathLives;
            GameStats.Energy += Stats.EndPathMoney;
        
            QueueFree();
        }
    }
}
