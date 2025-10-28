using System;
using Godot;

namespace BestagonDefence.Gameplay.Waves;

[GlobalClass]
public partial class Waves : Resource
{
    [Export]
    private Wave[] waves = [];
    
    /// <summary>
    /// Gets a wave, index starts at 1
    /// </summary>
    /// <param name="index">The index of the wave (starts at 1)</param>
    public Wave this[int index]
    {
        get
        {
            if (index <= 0 || index > waves.Length)
                throw new IndexOutOfRangeException();
            return waves[index - 1];
        }

        set
        {
            if (index <= 0 || index > waves.Length)
                throw new IndexOutOfRangeException();
            waves[index - 1] = value;
        }
    }
}