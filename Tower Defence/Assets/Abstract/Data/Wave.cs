namespace Abstract.Data
{
    /// <summary>
    /// A group of Enemy Sets that represents a full wave
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        public EnemySet[] enemySets;
        public float[] setDelays;
        // TODO - implement a smarter wave spawner
        //public int difficulty;
    }
}
