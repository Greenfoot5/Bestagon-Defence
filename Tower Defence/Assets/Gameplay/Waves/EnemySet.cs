using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// A set of a single enemy type making up part of a wave
    /// </summary>
    [System.Serializable]
    public class EnemySet
    {
        public GameObject enemy;
        public int count;
        public float rate;
    }
}
