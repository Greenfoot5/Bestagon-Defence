using Enemies;
using Gameplay;
using Gameplay.Waves;
using Godot;

public partial class WaveTimer : Timer
{
    [Export]
    public WaveData WaveData;
    
    public override void _Ready()
    {
        Enemy.OnEnemyDeath += TryStart;
    }

    private void TryStart()
    {
        if (WaveSpawner.EnemiesAlive <= 0 && WaveSpawner.SpawnerState == WaveSpawner.State.Waiting)
        {
            Start(WaveData.TimeBetweenWaves);
            
            // Increment Wave
            GameStats.Rounds++;
            GD.Print("Next wave is round " + GameStats.Rounds);
        }
    }
}
