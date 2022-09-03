using System.Collections;
using Abstract;
using Enemies;
using Gameplay.Waves;
using Levels._Nodes;
using UnityEngine;

namespace _WIP.Abilities.PositiveAbilities
{
    /// <summary>
    /// Heals enemy/ies on activation
    /// </summary>
    [CreateAssetMenu(fileName = "Spawner", menuName = "Enemy Abilities/Spawner")]
    public class SpawnerEnemyAbility : EnemyAbility
    {
        [Header("Ability Stats")]
        [Tooltip("The enemy to spawn")]
        [SerializeField]
        private GameObject spawn;

        [Tooltip("How many enemies to spawn")]
        [SerializeField]
        private float count;
        [Tooltip("How long to wait between spawning each enemy")]
        [SerializeField]
        private float spawnTimer;

        [Tooltip("Spawn from the start")]
        [SerializeField]
        private bool doSpawnFromStart;

        /// <summary>
        /// Spawns the enemy
        /// </summary>
        /// <param name="target">The enemy to heal</param>
        public override void Activate(GameObject target)
        {
            if (target == null)
            {
                return;
            }
            
            var movement = spawn.GetComponent<EnemyMovement>();
            var targetMovement = target.GetComponent<EnemyMovement>();
            if (movement == null)
            {
                Debug.LogWarning("Attempting to spawn " + spawn.name + " via enemy ability, but it does not have an EnemyMovement component!");
                return;
            }

            if (targetMovement == null)
            {
                return;
            }

            Runner.Run(SpawnEnemies(target.transform.position, targetMovement));
        }

        private IEnumerator SpawnEnemies(Vector3 position, EnemyMovement targetMovement)
        {
            for (var i = 0; i < count; i++)
            {
                // Spawn from the enemy's location or from the start of the map
                if (doSpawnFromStart)
                {
                    Instantiate(spawn, Waypoints.points[0].transform);
                }
                else
                {
                    GameObject enemy = Instantiate(spawn, position, new Quaternion());
                    enemy.GetComponent<EnemyMovement>().waypointIndex = targetMovement.waypointIndex;
                }

                WaveSpawner.enemiesAlive += 1;
                yield return new WaitForSeconds(spawnTimer);
            }
        }

        public override void OnCounterEnd(GameObject target) { }
    }
}