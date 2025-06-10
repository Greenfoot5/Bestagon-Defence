using BestagonDefense.Abstract;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Enemies;
using Godot;

namespace BestagonDefense.Modules.Slow;

[GlobalClass]
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
            
        Target.EnemyStats.Attributes[AttributeType.Speed].Add(Name, _slowPercentage);

        return true;
    }

    protected override void Remove()
    {
        base.Remove();

        Target.EnemyStats.Attributes[AttributeType.Speed].Remove(Name);
    }

    protected override void DoEffect() { }
}