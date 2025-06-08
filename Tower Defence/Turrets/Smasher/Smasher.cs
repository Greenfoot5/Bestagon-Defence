using Abstract.Attributes;
using Godot;

namespace Turrets.Smasher
{
    /// <summary>
    /// Extends Turret to add smashing functionality
    /// </summary>
    // TODO - Find and hit targets
    public partial class Smasher : Turret
    {
        /// <summary>
        /// The effect to play when the smasher attacks
        /// </summary>
        [Export]
        private GpuParticles2D smashEffect;

        /// <summary>
        /// Check for new enemies in radius and attacks if there are.
        /// </summary>
        public override void _Process(double delta)
        {
            base._Process(delta);
            
            // Don't do anything if no enemy is in range
            // Collider2D[] results = Physics2D.OverlapCircleAll(Position, range.GetStat());
            // if (!results.Any(x => 
            //         x != null && x.CompareTag(enemyTag)))
            // {
            //     fireCountdown -= delta;
            //     return;
            // }
            
            // If our attack is off cooldown
            if (FireCountdown <= 0 && Stats[AttributeType.FireRate].Value != 0)
            {
                FireCountdown = 1 / Stats[AttributeType.FireRate].Value;
                Attack((float) delta);
            }
            
            FireCountdown -= delta;
        }

        protected override void UpdateRange(Attribute attribute)
        {
            base.UpdateRange(attribute);
            
            // Update the smash effect size
            float scale = Stats[AttributeType.Range].Value * 0.05f;
            ((ParticleProcessMaterial)smashEffect.ProcessMaterial).Scale = new Vector2(scale, scale);
        }
        
        /// <summary>
        /// Deals damage to all enemies in range
        /// </summary>
        protected override void Attack(float delta)
        {
            smashEffect.Emitting = true;
            
            base.Attack(this);
            
            // Gets all the enemies in the AoE and calls Damage on them
            // ReSharper disable once Unity.PreferNonAllocApi
            // Collider2D[] results = Physics2D.OverlapCircleAll(Position, range.GetStat());
            
            // foreach (Collider2D collider2d in results)
            // {
            //     if (!collider2d.CompareTag(enemyTag)) continue;
            //     
            //     var enemy = collider2d.GetComponent<Enemy>();
            //     
            //     // Take damage depending on how close the enemy is to the turret's centre
            //     Vector2 position = Position;
            //     float distance = 1 - (position - collider2d.ClosestPoint(position)).sqrMagnitude /
            //         (range.GetTrueStat() * range.GetTrueStat()) + 0.25f;
            //     float damagePercentage = Mathf.Clamp(distance, 0.2f, 1f);
            //     
            //     // Only deal damage if it will actually damage the enemy
            //     if (!(damagePercentage > 0)) continue;
            //     
            //     Hit(enemy, this);
            //     enemy.TakeDamage(damage.GetTrueStat() * damagePercentage, this);
            // }
        }
    }
}
