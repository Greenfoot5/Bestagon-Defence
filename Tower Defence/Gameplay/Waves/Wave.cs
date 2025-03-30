using Godot;

namespace Gameplay.Waves
{
    /// <summary>
    /// A group of Enemy Sets that represents a full wave
    /// </summary>
    public partial class Wave : Resource
    {
        public EnemySet[] enemySets;
        public float[] setDelays;
        // TODO - implement a smarter wave spawner
        //public int difficulty;
    }
}
