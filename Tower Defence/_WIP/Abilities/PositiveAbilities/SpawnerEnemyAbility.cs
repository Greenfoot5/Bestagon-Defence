using System.Collections;
using Abstract;
using Enemies;
using Gameplay.Waves;
using Godot;
using Levels._Nodes;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    public partial class SpawnerEnemyAbility : EnemyAbility
    {
        /// <summary>
        /// The enemy to spawn
        /// </summary>
        [ExportGroup("Ability Stats")]
        [Export]
        private PackedScene spawn;

        /// <summary>
        /// How many enemies to spawn
        /// </summary>
        [Export]
        private float count;
        /// <summary>
        /// How long to wait between spawning each enemy
        /// </summary>
        [Export]
        private float spawnTimer;

        /// <summary>
        /// Spawn from the start
        /// </summary>
        [Export]
        private bool doSpawnFromStart;

        /// <summary>
        /// Activates the ability
        /// </summary>
        /// <param name="target">The enemy to spawn</param>
        public override void Activate(GodotObject target)
        {
            if (target == null)
            {
                return;
            }
            
            
        }
        
        /// <summary>
        /// Handles the instantiation in a timely manner
        /// </summary>
        /// <param name="transform">The transformer of the spawner</param>
        private IEnumerator SpawnEnemies(Enemy target)
        {
            for (var i = 0; i < count; i++)
            {
                // Spawn from the enemy's location or from the start of the map
                if (doSpawnFromStart) {
                    var spawned = (Node2D) spawn.Instantiate();
                    // TODO - Check paths index properly
                    // spawned.Position = Waypoints.paths[0][0];
                    
                    // TODO - User timer
                    // yield return new WaitForSeconds(spawnTimer);
                }
                else
                {
                    Vector2 pos = target.Position;
                    // TODO - Use timer
                    // yield return new WaitForSeconds(spawnTimer);
                    var enemy = (Enemy)spawn.Instantiate();
                    enemy.Position = pos;
                    enemy.WaypointIndex = enemy.WaypointIndex;
                }

                WaveSpawner.EnemiesAlive += 1;
            }
            yield break;
        }

        public override void OnCounterEnd(GodotObject target) { }
    }
}