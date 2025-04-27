using System;
using System.Linq;
using Abstract.Attributes;
using Enemies;
using Godot;
using Attribute = Abstract.Attributes.Attribute;

namespace Turrets
{
    public abstract partial class DynamicTurret : Turret
    {
        /// <summary>
        /// The various targeting methods a turret can use to find a target
        /// </summary>
        public enum TargetingMethod
        {
            Closest = 0,
            Weakest,
            Strongest,
            First,
            Last
        }
        
        // How long between each target update
        private const float UpdateTargetTimer = 0.5f;
        
        /// <summary>
        /// What TargetingMethod the turret uses to pick its next target
        /// </summary>
        [Export]
        public TargetingMethod TargetPriorityMethod = TargetingMethod.Closest;
        /// <summary>
        /// If the turret should always be searching for the best target (according to it's TargetingMethod)
        /// </summary>
        [Export]
        private bool _aggressiveRetargeting;
        
        /// <summary>
        /// The enemy script of the current target
        /// </summary>
        protected Enemy TargetEnemy;

        /// <summary>
        /// The transform to attack from (i.e. for bullets)
        /// </summary>
        [Export]
        protected Node2D FirePoint;

        /// <summary>
        /// The raycast to check if looking at enemy
        /// </summary>
        [Export]
        protected ShapeCast2D ShapeCast;
        
        /// <summary>
        /// The transform to perform any rotations on
        /// </summary>
        [Export]
        public Node2D PartToRotate;

        protected DynamicTurret()
        {
            Stats[AttributeType.RotationSpeed] = new Attribute(3f, min:0f);
        }
        
        /// <summary>
        /// Begins the target searching
        /// </summary>
        public override void _Ready()
        {
            base._Ready();
            // Start finding targets
            var targeting = new Timer();
            targeting.WaitTime = UpdateTargetTimer;
            targeting.Autostart = true;
            targeting.OneShot = false;
            targeting.Timeout += UpdateTarget;
            AddChild(targeting);
            UpdateTarget();
        }

        /// <summary>
        /// Update our current target to check if it's still the most valuable, or pick a new one.
        /// </summary>
        private void UpdateTarget()
        {
            // If the turret is not aggressively retargeting, check if the target is still in range
            if (!_aggressiveRetargeting && IsInstanceValid(TargetEnemy))
            {
                float distanceToEnemy = Position.DistanceSquaredTo(TargetEnemy.Position);
                if (distanceToEnemy <= Stats[AttributeType.Range].Value * Stats[AttributeType.Range].Value)
                    return;
            }

            // Create a list of enemies within range
            Enemy[] enemiesInRange = (from enemy in Range.GetOverlappingAreas()
                where enemy is Enemy && IsInstanceValid(enemy)
                select (Enemy)enemy).ToArray();
            // Set the current value to be too high or too low.
            // Value is based on targeting method
            float currentValue = Mathf.Inf;
            if (TargetPriorityMethod is TargetingMethod.Strongest or TargetingMethod.First)
            {
                currentValue = -Mathf.Inf;
            }

            Enemy mostValuableEnemy = null;

            // Check there are enemies in range, and if not, the turret has no target
            if (enemiesInRange.Length == 0)
            {
                TargetEnemy = null;
                return;
            }
            
            if (!IsInstanceValid(TargetEnemy))
            {
                TargetEnemy = enemiesInRange[0];
            }

            // Loop through the enemies and find the most valuable
            foreach (Enemy enemy in enemiesInRange)
            {
                switch (TargetPriorityMethod)
                {
                    case TargetingMethod.Closest:
                        // Find if the enemy is closer than our current most valuable
                        float squaredDistanceToEnemy = Position.DistanceSquaredTo(TargetEnemy.Position);
                        if (squaredDistanceToEnemy < currentValue)
                        {
                            currentValue = squaredDistanceToEnemy;
                            mostValuableEnemy = enemy;
                        }

                        break;
                    case TargetingMethod.Weakest:
                        // Find if the enemy has less health than our current most valuable
                        float health = enemy.Health;
                        if (health < currentValue)
                        {
                            currentValue = health;
                            mostValuableEnemy = enemy;
                        }

                        break;
                    case TargetingMethod.Strongest:
                        // Find if the enemy has more health than our current most valuable
                        float enemyHealth = enemy.Health;
                        if (enemyHealth > currentValue)
                        {
                            currentValue = enemyHealth;
                            mostValuableEnemy = enemy;
                        }

                        break;
                    case TargetingMethod.First:
                        // Find if the enemy has the most map progress than our current most valuable
                        float mapProgress = enemy.mapProgress;
                        if (mapProgress > currentValue)
                        {
                            currentValue = mapProgress;
                            mostValuableEnemy = enemy;
                        }

                        break;
                    case TargetingMethod.Last:
                        // Find if the enemy has the lease map progress than our current most valuable
                        float pathProgress = enemy.mapProgress;
                        if (pathProgress < currentValue)
                        {
                            currentValue = pathProgress;
                            mostValuableEnemy = enemy;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // We have found a valid target
            if (mostValuableEnemy is not null)
            {
                if (TargetEnemy is not null)
                    TargetEnemy.OnDeath -= UpdateTarget;
                TargetEnemy = mostValuableEnemy;
                TargetEnemy.OnDeath += UpdateTarget;
            }
            // Set our target to null if there is none
            else
            {
                if (TargetEnemy is null)
                    return;
                TargetEnemy.OnDeath -= UpdateTarget;
                TargetEnemy = null;
            }
        }
        
        /// <summary>
        /// Rotates the turret towards our target
        /// </summary>
        protected void LookAtTarget(double delta)
        {
            if (!IsInstanceValid(TargetEnemy)) return;
            
            float rotationAngleNeed = PartToRotate.GetAngleTo(TargetEnemy.Position) + float.Pi / 2;
            
            double zAngle = Mathf.Clamp(rotationAngleNeed, -Stats[AttributeType.RotationSpeed].Value * delta,
                Stats[AttributeType.RotationSpeed].Value * delta);
            PartToRotate.Rotation += (float)zAngle;
        }

        /// <summary>
        /// Check the turret is currently looking at our target.
        /// Used to see if the turret can shoot or needs to rotate more
        /// </summary>
        /// <returns>If the turret is currently looking at the target</returns>
        protected bool IsLookingAtTarget()
        {
            if (TargetEnemy == null) return false;
            
            // Setup the raycast
            for (var i = 0; i < ShapeCast.GetCollisionCount(); i++)
            {
                var collider = (Enemy)ShapeCast.GetCollider(i);
                if (collider == TargetEnemy)
                {
                    return true;
                }
            }

            return false;
        }
    }
}