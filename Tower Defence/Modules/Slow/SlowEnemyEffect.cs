using Abstract;
using Abstract.Attributes;
using Enemies;
using Godot;


namespace Modules.Slow
{
    public partial class SlowEnemyEffect : EnemyEffect
    {
        /// <summary>
        /// Multiplicative percentage modifier enemy's speed
        /// </summary>
        [Export]
        private AttributeModifier _slowPercentage;

        public override bool Apply(Enemy target)
        {
            if (!base.Apply(target))
                return false;
            
            Target.Stats.Attributes[AttributeType.Speed].Add(Name, _slowPercentage);

            return true;
        }

        protected override void Remove()
        {
            base.Remove();

            Target.Stats.Attributes[AttributeType.Speed].Remove(Name);
        }

        protected override void DoEffect() { }
    }
}