using System.Collections;
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
        
        [SerializeField] 
        [Tooltip("The distance from enemy to waypoint before it's considered reached")]
        private float distanceToWaypoint = 0.05f;
        
        [Tooltip("How many waypoints the enemy has passed, and the percentage to the next one")]
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
            // If the enemy is moving backwards
            if (_enemy.speed.GetTrueStat() < 0)
            {
                MoveBackwards();
                return;
            }
            
            // Get the direction and move in that direction
            Vector3 dir = _target.position - transform.position;
            transform.Translate(dir.normalized * (_enemy.speed.GetStat() * Time.deltaTime), Space.World);
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

        private void MoveBackwards()
        {
            // Get the direction and move in that direction
            Vector3 dir = Waypoints.points[_waypointIndex - 1].position - transform.position;
            transform.Translate(dir.normalized * (Mathf.Abs(_enemy.speed.GetTrueStat()) * Time.deltaTime), Space.World);
            mapProgress = _waypointIndex - (distanceToWaypoint / _maxDistance);
        
            // If the enemy hasn't reached the previous waypoint, there's no point knocking it back further
            if (!(Vector3.Distance(transform.position, Waypoints.points[_waypointIndex - 1].position) <= distanceToWaypoint)) return;
            
            // If the enemy has reached the end, destroy
            if (_waypointIndex - 1 < 0)
            {
                return;
            }

            // Get the next waypoint
            _waypointIndex--;
            _target = Waypoints.points[_waypointIndex];
            mapProgress = _waypointIndex;
            _maxDistance = Vector3.Distance(Waypoints.points[_waypointIndex + 1].position, _target.position);
        }
    
        /// <summary>
        /// Called when the enemy reach the final waypoint
        /// </summary>
        private void EndPath()
        {
            _enemy.FinishPath();
        }

        public void TakeKnockback(float amount, Vector3 turretLocation)
        {
            Vector3 v = _target.position - transform.position;
            Vector3 w = turretLocation - transform.position;
            float multiplier = Vector3.Dot(v.normalized, w.normalized);
            
            // Actually deal knockback
            float knockback = amount * _enemy.knockbackModifier * multiplier;

            StartCoroutine(DealKnockback(knockback));
        }

        private IEnumerator DealKnockback(float speedMultiplier)
        {
            Debug.Log("Added Knockback: " + speedMultiplier);
            _enemy.speed.MultiplyModifier(speedMultiplier);
            yield return new WaitForSeconds(0.2f);
            _enemy.speed.DivideModifier(speedMultiplier);
        }
    }
}
