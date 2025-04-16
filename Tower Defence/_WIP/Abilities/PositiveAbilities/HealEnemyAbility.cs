using Abstract.Attributes;
using Enemies;
using Godot;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    public partial class HealEnemyAbility : EnemyAbility
    {
        /// <summary>
        /// If the healing is a % or value heal
        /// </summary>
        [ExportGroup("Ability Stats")]
        [Export]
        private bool isPercentage = true;
        /// <summary>
        /// How much to heal for (value healing)
        /// </summary>
        [Export]
        private int healAmount = 20;
        /// <summary>
        /// What percentage to heal by (% healing)
        /// </summary>
        [Export]
        private float healPercentage = 0.2f;
        
        /// <summary>
        /// Heals an enemy
        /// </summary>
        /// <param name="target">The enemy to heal</param>
        public override void Activate(GodotObject target)
        {
            // Check the target is an enemy
            var enemy = (Enemy)target;
            if (enemy == null)
            {
                return;
            }
            
            // Heal the target
            if (isPercentage)
            {
                enemy.TakeDamage(enemy.Stats.Attributes[AttributeType.MaxHealth].Value * healPercentage, this);
            }
            else
            {
                enemy.TakeDamage(healAmount, this);
            }
        }
        
        /// <summary>
        /// There's nothing to clear up after the counter finishes
        /// </summary>
        public override void OnCounterEnd(GodotObject target) { }
    }
}
