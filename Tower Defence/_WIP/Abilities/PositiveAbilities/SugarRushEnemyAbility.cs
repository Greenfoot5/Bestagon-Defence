using System.Collections;
using Abstract;
using Enemies;
using Godot;


namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    public partial class SugarRushEnemyAbility : EnemyAbility
    {
        /// <summary>
        /// The multiplier to the speed
        /// </summary>
        [ExportGroup("Ability Stats")]
        [Export]
        private float speedMultiplier = 1.5f;
        /// <summary>
        /// How long the effect lasts
        /// </summary>
        [Export]
        private float duration = 1f;

        /// <summary>
        /// Heals an enemy
        /// </summary>
        /// <param name="target">The enemy to heal</param>
        public override void Activate(GodotObject target)
        {
            if (target == null)
            {
                return;
            }
            
            var enemyComponent = (Enemy)target;

            Runner.Run(EatSugar(enemyComponent));

        }

        public override void OnCounterEnd(GodotObject target) { }

        /// <summary>
        /// There's nothing to clear up after the counter finishes
        /// </summary>
        private IEnumerator EatSugar(Enemy enemyComponent)
        {
            // enemyComponent.Speed.MultiplyModifier(speedMultiplier);
            // TODO - Use timer & Use Attributes
            // yield return new WaitForSeconds(duration);
            // enemyComponent.Speed.DivideModifier(speedMultiplier);
            yield return null;
        }
    }
}
