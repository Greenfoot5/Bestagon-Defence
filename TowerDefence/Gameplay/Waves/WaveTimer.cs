using Godot;

namespace BestagonDefence.Gameplay.Waves;

public partial class WaveTimer : Timer
{
    [Export]
    public WaveData WaveData;
    
    public override void _Ready()
    {
        WaveSpawner.OnEnemyDied += TryStart;
    }

    public override void _ExitTree()
    {
        WaveSpawner.OnEnemyDied -= TryStart;
    }

    private void TryStart()
    {
        if (WaveSpawner.EnemiesAlive <= 0 && WaveSpawner.SpawnerState == SpawnerState.Waiting)
        {
            Start(WaveData.TimeBetweenWaves);
            
            // Increment Wave
            GameStats.Rounds++;
        }
    }
}
