using UnityEngine;

/*
 * A class to contain all of our waypoint data
 */

public class Waypoints : MonoBehaviour
{
    // Has a array of all the waypoints
    public static Transform[] Points;

    private void Awake()
    {
        // Creates and adds all waypoints to the array
        Points = new Transform[transform.childCount];
        
        for (var i = 0; i < Points.Length; i++)
        {
            Points[i] = transform.GetChild(i);
        }
    }
}
