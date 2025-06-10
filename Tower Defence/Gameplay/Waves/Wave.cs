using Godot;

namespace BestagonDefense.Gameplay.Waves;

/// <summary>
/// A group of Enemy Sets that represents a full wave
/// </summary>
[GlobalClass]
public partial class Wave : Resource
{
    [Export]
    public EnemySet[] EnemySets;
    [Export]
    public float[] SetDelays;
}