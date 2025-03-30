

using Godot;

namespace Levels._Nodes
{
    /// <summary>
    /// A directory of all the waypoints in a map
    /// </summary>
    public abstract partial class Waypoints : BuildableTile
    {
        /// <summary>
        /// Has an array of all the waypoints
        /// </summary>
        public static Node2D[] points;
        
        /// <summary>
        /// Stores all the waypoints in the local array
        /// </summary>
        public override void _Ready()
        {
            // Creates and adds all waypoints to the array
            points = new Node2D[GetChildCount()];
        
            for (var i = 0; i < points.Length; i++)
            {
                points[i] = (Node2D)GetChild(i);
            }
        }
    }
}
