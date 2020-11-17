using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        target = Waypoints.points[waypointIndex];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.05f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }
}
