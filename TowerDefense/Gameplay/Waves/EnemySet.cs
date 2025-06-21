using BestagonDefense.Enemies;
using Godot;

namespace BestagonDefense.Gameplay.Waves;

/// <summary>
/// A set of a single enemy type making up part of a wave
/// </summary>
[GlobalClass]
public partial class EnemySet : Resource
{
    /// <summary>
    /// The enemy to spawn
    /// </summary>
    [Export]
    public EnemyStats Enemy;
    /// <summary>
    /// How many to spawn
    /// </summary>
    [Export]
    public int Count;
    /// <summary>
    /// The delay (in seconds) between each enemy in this set
    /// </summary>
    [Export]
    public float Rate;
}