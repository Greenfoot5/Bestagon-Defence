using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// A script to move the enemy along our designated path
    /// Set out by a waypoints array
    /// </summary>

    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        // Waypoint indexes
        private Transform _target;
        private int _waypointIndex;

        private Enemy _enemy;
    
        // The distance from enemy to waypoint before it's considered reached
        [SerializeField] 
        private float distanceToWaypoint = 0.05f;
    
        private void Start()
        {
            // Set the next target to the first waypoint.
            // _waypointIndex will always be 0 at the start
            _target = Waypoints.points[_waypointIndex];

            _enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// Every scene, we need to move the enemy
        /// </summary>
        private void Update()
        {
            // Get the direction and move in that direction
            var dir = _target.position - transform.position;
            transform.Translate(dir.normalized * (_enemy.speed * Time.deltaTime), Space.World);
        
            // If we're within the set distance, get the next waypoint
            if (Vector3.Distance(transform.position, _target.position) <= distanceToWaypoint)
            {
                GetNextWaypoint();
            }
        
            // Reset speed in case we've been slowed
            // _enemy.speed = _enemy.startSpeed;
        }
    
        /// <summary>
        /// Gets the next waypoint in the waypoints array
        /// </summary>
        private void GetNextWaypoint()
        {
            // If we've reached the end, destroy
            if (_waypointIndex >= Waypoints.points.Length - 1)
            {
                EndPath();
                return;
            }
        
            // Get the next waypoint
            _waypointIndex++;
            _target = Waypoints.points[_waypointIndex];
        }
    
        /// <summary>
        /// Called when we reach the final waypoint
        /// </summary>
        private void EndPath()
        {
            // Let our other systems know the enemy reached the end
            GameStats.lives -= _enemy.deathLives;
            WaveSpawner.enemiesAlive--;
            GameStats.money += _enemy.endPathMoney;
        
            Destroy(gameObject);
        }
    }
}
