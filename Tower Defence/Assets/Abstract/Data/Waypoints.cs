using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// A directory of all the waypoints in a map
    /// </summary>
    public class Waypoints : MonoBehaviour
    {
        // Has a array of all the waypoints
        public static Transform[] points;
        
        /// <summary>
        /// Stores all the waypoints in the local array
        /// </summary>
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
}
