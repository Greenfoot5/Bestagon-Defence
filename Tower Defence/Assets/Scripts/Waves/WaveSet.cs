using UnityEngine;

[System.Serializable]
public class WaveSet
{
    public Wave[] waves = new Wave[12];
    public float[] delays = new float[12];
    public int difficulty;
}
