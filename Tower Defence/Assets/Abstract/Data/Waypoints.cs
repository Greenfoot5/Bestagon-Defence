using UnityEngine;

/*
 * A class to contain all of our waypoint data
 */

public class Waypoints : MonoBehaviour
{
    // Has a array of all the waypoints
    public static Transform[] points;

    private void Awake()
    {
        // Creates and adds all waypoints to the array
        points = new Transform[transform.childCount];
        
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
