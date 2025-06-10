using System.Collections;
using BestagonDefense.Turrets;
using Godot;

namespace BestagonDefense.Abstract;

// TODO - Setup loop with Timer
public abstract partial class TurretEffect : Resource
{
    public int tickCount;
    public int tickDuration;
    public int ticksLeft;
    public int tier;
    public bool isCancelled;

    protected Turret Target;

    public TurretEffect(Turret target)
    {
        Target = target;
    }

    public TurretEffect()
    {
        Target = null;
    }

    public abstract void Apply();
    public abstract void Remove();
    public abstract void DoEffect();

    public IEnumerator Tick()
    {
        if (isCancelled)
            yield break;
            
        //yield return new WaitForSeconds(tickDuration);
        ticksLeft--;

        DoEffect();

        if (ticksLeft <= 0)
        {
            Remove();
        }
    }
}