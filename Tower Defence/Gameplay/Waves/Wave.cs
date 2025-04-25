using Godot;

namespace Gameplay.Waves
{
    /// <summary>
    /// A group of Enemy Sets that represents a full wave
    /// </summary>
    [GlobalClass]
    public partial class Wave : Resource
    {
        [Export]
        public EnemySet[] enemySets;
        [Export]
        public float[] setDelays;
    }
}
