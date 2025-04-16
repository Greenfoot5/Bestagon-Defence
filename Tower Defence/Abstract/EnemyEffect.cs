using Enemies;
using Godot;

namespace Abstract
{
    // TODO - Setup with timer
    public abstract partial class EnemyEffect : Node
    {
        /// <summary>
        /// The amount of ticks the effect starts with
        /// </summary>
        [Export]
        public int tickCount;
        /// <summary>
        /// How long each tick lasts in seconds
        /// </summary>
        [Export]
        public float tickDuration;
        /// <summary>
        /// The number of ticks the effect currently has left
        /// </summary>
        public int ticksLeft;

        private double _durationLeft;
        
        /// <summary>
        /// The effect type to use when checking duplicates and immunities
        /// </summary>
        [Export]
        public string effectType;
        /// <summary>
        /// The tier of the effect
        /// </summary>
        [Export]
        public int tier;
        /// <summary>
        /// If the effect should be stopped after the current tick
        /// </summary>
        public bool isCancelled;

        /// <summary>
        /// The enemy the effect has been applied to
        /// </summary>
        protected Enemy Target;

        public override void _Process(double delta)
        {
            if (isCancelled)
            {
                Remove();
                return;
            }
            
            if (_durationLeft > 0)
            {
                _durationLeft -= delta;
                return;
            }
            
            ticksLeft--;
            if (ticksLeft >= 0)
            {
                _durationLeft = tickDuration;
                DoEffect();
                return;
            }

            Remove();
        }

        public virtual bool Apply(Enemy target)
        {
            Target = target;
            ticksLeft = tickCount;
            
            if (Target.Stats.UniqueEffects.Contains(effectType))
                return false;

            if (Target.Stats.ActiveEffects.ContainsKey(effectType))
            {
                if (Target.Stats.ActiveEffects[effectType].tier > tier && !Target.Stats.ActiveEffects[effectType].isCancelled)
                {
                    return false;
                }

                if (Target.Stats.ActiveEffects[effectType].tier == tier)
                {
                    Target.Stats.ActiveEffects[effectType].ticksLeft = tickCount;
                    Target.Stats.ActiveEffects[effectType]._durationLeft = 0f;
                    return false;
                }

                if (Target.Stats.ActiveEffects[effectType].tier < tier)
                {
                    Target.Stats.ActiveEffects[effectType].isCancelled = true;
                    Target.Stats.ActiveEffects.Remove(effectType);
                }
            }
            
            Target.Stats.ActiveEffects.Add(effectType, this);
            Target.OnDeath += CancelFromDeath;

            return true;
        }

        protected virtual void Remove()
        {
            Target.Stats.ActiveEffects.Remove(effectType);
            Target.OnDeath -= CancelFromDeath;
        }

        protected abstract void DoEffect();

        private void CancelFromDeath()
        {
            isCancelled = true;
        }
    }
}
