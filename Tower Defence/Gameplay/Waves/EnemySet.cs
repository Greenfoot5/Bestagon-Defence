

using Godot;

namespace Gameplay.Waves
{
    /// <summary>
    /// A set of a single enemy type making up part of a wave
    /// </summary>
    public partial class EnemySet : Resource
    {
        public PackedScene enemy;
        public int count;
        public float rate;
    }
}
