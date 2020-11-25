using UnityEngine;

/*
 * A script to move the enemy along our designated path
 * Set out by a waypoints array
 */

public class EnemyMovement : MonoBehaviour
{
    // The speed the enemy should move at
    public float speed = 2f;
    
    // Waypoint indexes
    private Transform _target;
    private int _waypointIndex;
    
    // The distance from enemy to waypoint before it's considered reached
    [SerializeField] 
    private float distanceToWaypoint = 0.05f;
    
    // Called when the scene starts
    private void Start()
    {
        // Set the next target to the first waypoint.
        // _waypointIndex will always be 0 at the start
        _target = Waypoints.Points[_waypointIndex];
    }
    
    // Every scene, we need to move the enemy
    private void Update()
    {
        // Get the direction and move in that direction
        var dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        
        // If we're within the set distance, get the next waypoint
        if (Vector3.Distance(transform.position, _target.position) <= distanceToWaypoint)
        {
            GetNextWaypoint();
        }
    }
    
    // Gets the next waypoint in the waypoints array
    private void GetNextWaypoint()
    {
        // If we've reached the end, destroy
        if (_waypointIndex >= Waypoints.Points.Length - 1)
        {
            Destroy(gameObject);
            // Destroy can take a little time to finish
            return;
        }
        
        // Get the next waypoint
        _waypointIndex++;
        _target = Waypoints.Points[_waypointIndex];
    }
}
