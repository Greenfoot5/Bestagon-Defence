using Godot;

namespace BestagonDefence.Gameplay.Waves;

/// <summary>
/// A group of Enemy Sets that represents a full wave
/// </summary>
[GlobalClass]
public partial class Wave : Resource
{
    /// <summary>
    /// The enemy sets to spawn in the wave
    /// </summary>
    [Export]
    public EnemySet[] EnemySets;
    /// <summary>
    /// The delays between each set
    /// Length should be 1 less than EnemySets
    /// </summary>
    [Export]
    public float[] SetDelays;
}