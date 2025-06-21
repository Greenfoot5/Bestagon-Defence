namespace BestagonDefense.Gameplay.Waves;

/// <summary>
/// State for a(ll) spawner(s)
/// </summary>
public enum SpawnerState
{ 
    // Countdown to next spawn
    Countdown,
    // Spawning Enemies
    Spawning,
    // Waiting for all enemies to die
    Waiting,
}
