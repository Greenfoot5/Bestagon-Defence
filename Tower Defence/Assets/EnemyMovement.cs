using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;

    private Transform _target;
    private int _waypointIndex;

    private void Start()
    {
        _target = Waypoints.Points[_waypointIndex];
    }

    private void Update()
    {
        var dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.05f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if (_waypointIndex >= Waypoints.Points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }
        _waypointIndex++;
        _target = Waypoints.Points[_waypointIndex];
    }
}
