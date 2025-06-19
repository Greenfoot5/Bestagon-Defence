using Godot;

namespace BestagonDefense.Gameplay.Waves;

/// <summary>
/// Contains the data for waves on a spawner
/// </summary>
[GlobalClass]
public partial class WaveData : Resource
{
    /// <summary>
    /// The base enemy scene to spawn
    /// </summary>
    [Export]
    public PackedScene Enemy;
    
    /// <summary>
    /// Wave based multiplier for health
    /// </summary>
    [ExportGroup("Wave Scaling")]
    [Export]
    public Curve Health;
    /// <summary>
    /// Wave based multiplier for count
    /// </summary>
    [Export]
    public Curve EnemyCount;
    
    /// <summary>
    /// How long to wait between waves
    /// </summary>
    [Export]
    public float TimeBetweenWaves = 5f;
    /// <summary>
    /// The time at the beginning before the start of the game
    /// </summary>
    [Export]
    public float PreparationTime = 8f;
    
    /// <summary>
    /// The index of the wave to start from after the final wave
    /// </summary>
    [Export]
    public int RepeatIndex;

    /// <summary>
    /// How many waves that should be spawned before repeating
    /// </summary>
    [Export]
    public int Length;
}