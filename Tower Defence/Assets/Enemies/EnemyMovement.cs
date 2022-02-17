using Abstract.Data;
using Levels._Nodes;
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

        public float mapProgress;
        private float _maxDistance;
        
        /// <summary>
        /// Initialises relevant variables
        /// </summary>
        private void Start()
        {
            // Set the next target to the first waypoint.
            // _waypointIndex will always be 0 at the start
            _target = Waypoints.points[_waypointIndex];

            _enemy = GetComponent<Enemy>();
        }

        /// <summary>
        /// Every scene, the enemy needs to move
        /// </summary>
        private void Update()
        {
            // If the enemy aren't going to move forward, the enemy shouldn't move at all.
            if (_enemy.speed < 0)
            {
                return;
            }
            
            // Get the direction and move in that direction
            Vector3 dir = _target.position - transform.position;
            transform.Translate(dir.normalized * (_enemy.speed * Time.deltaTime), Space.World);
            mapProgress = _waypointIndex + 1 - (distanceToWaypoint / _maxDistance);
        
            // If the enemy is within the set distance, get the next waypoint
            if (Vector3.Distance(transform.position, _target.position) <= distanceToWaypoint)
            {
                GetNextWaypoint();
            }
        }
    
        /// <summary>
        /// Gets the next waypoint in the waypoints array
        /// </summary>
        private void GetNextWaypoint()
        {
            // If the enemy has reached the end, destroy
            if (_waypointIndex >= Waypoints.points.Length - 1)
            {
                EndPath();
                return;
            }
        
            // Get the next waypoint
            _waypointIndex++;
            _target = Waypoints.points[_waypointIndex];
            mapProgress = _waypointIndex;
            _maxDistance = Vector3.Distance(transform.position, _target.position);
        }
    
        /// <summary>
        /// Called when the enemy reach the final waypoint
        /// </summary>
        private void EndPath()
        {
            _enemy.FinishPath();
        }
    }
}
