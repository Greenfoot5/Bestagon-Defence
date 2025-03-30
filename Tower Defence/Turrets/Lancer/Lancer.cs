using System.Collections;
using Abstract.Attributes;
using Enemies;
using Godot;

namespace Turrets.Lancer
{
    /// <summary>
    /// Extends Turret to add lancing functionality
    /// </summary>
    public partial class Lancer : Turret
    {
        /// <summary>How long between each target update</summary>
        private const float UpdateTargetTimer = 0.5f;
        
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

        /// <summary>
        /// The current target
        /// </summary>
        [Export]
        private Node2D _target;
        /// <summary>
        /// The Enemy script of the current target
        /// </summary>
        [Export]
        private Enemy _targetEnemy;
        
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
        public Node2D partToRotate;

        public Lancer()
        {
            Stats[AttributeType.BulletRange] = new Attribute(3f);
        }

        /// <summary>
        /// Begins the target searching
        /// </summary>
        public override void _Ready()
        {
            // Start finding targets
            // StartCoroutine(TargetCoroutine());
        }
        
        /// <summary>
        /// Calls our targeting method every UpdateTargetTimer.
        /// </summary>
        private IEnumerator TargetCoroutine()
        {
            while (Visible)
            {
                HasATarget();
                // yield return new WaitForSeconds(UpdateTargetTimer);
            }
            yield break;
        }
        
        /// <summary>
        /// Check if there is an enemy in range
        /// </summary>
        /// <returns>If the turret is currently looking at the target</returns>
        private bool HasATarget()
        {
            // Setup the raycast
            // var results = new List<RaycastHit2D>();
            // var contactFilter = new ContactFilter2D()
            // {
                // layerMask = LayerMask.GetMask("Enemies")
            // };
            // Physics2D.Raycast(Position, firePoint.up, contactFilter, results, range.GetStat());

            // Loop through the hits to see if the turret can hit the target
            var foundEnemy = false;
            // foreach (RaycastHit2D unused in results.Where(hit => hit.transform.CompareTag("Enemy")))
            // {
                // foundEnemy = true;
            // }
            return foundEnemy;
        }

        /// <summary>
        /// Check for new enemies in attack range
        /// </summary>
        public override void _Process(double delta)
        {
            base.Update();
            
            // Don't do anything if no enemy is in range
            if (!HasATarget())
            {
                FireCountdown -= delta;
                return;
            }
            
            // If our attack is off cooldown
            if (FireCountdown <= 0)
            {
                FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
                Attack();
            }
            
            FireCountdown -= delta;
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack()
        {
            // attackEffect.Play();
            // Creates the bullet
            var bullet = bulletPrefab.Instantiate<Bullet>();
            bullet.Position = firePoint.Position;
            bullet.Rotation = firePoint.Rotation;
            bullet.Name = "_" + bullet.Name;
            
            base.Attack(this);
            
            // Get the end point of the line renderer
            // Vector2 direction = (firePoint.up * bulletRange.GetStat());
            // Vector2 endPosition = (direction + Position);
            
            // bullet.Seek(endPosition, this);
            
            Shoot(bullet);
        }
    }
}
