using BestagonDefence.Abstract;
using BestagonDefence.Abstract.Attributes;
using BestagonDefence.Enemies;
using Godot;

namespace BestagonDefence.Modules.Slow;

[GlobalClass]
public partial class SlowEnemyEffect : EnemyEffect
{
    /// <summary>
    /// Multiplicative percentage modifier enemy's speed
    /// </summary>
    [Export]
    private Modifier _slowPercentage;

    /// <summary>
    /// Applies the slow effect to the target
    /// </summary>
    /// <param name="target">The target to slow</param>
    /// <returns>false if it failed to apply</returns>
    public override bool Apply(Enemy target)
    {
        if (!base.Apply(target))
            return false;
            
        Target.EnemyStats.Attributes[AttributeType.Speed].Add(Name, _slowPercentage);

        return true;
    }

    /// <summary>
    /// Removes the slow effect from the target
    /// </summary>
    protected override void Remove()
    {
        base.Remove();

        Target.EnemyStats.Attributes[AttributeType.Speed].Remove(Name);
    }

    /// <summary>
    /// Does nothing (would perform the effect of the slow
    /// </summary>
    protected override void DoEffect() { }
}