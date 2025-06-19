using System;
using Godot;
using Environment = System.Environment;

namespace BestagonDefense.Abstract.Data;

/// <summary>
/// Random generator from https://www.Youtube.com/watch?v=LWFzPP8ZbdU
/// </summary>
public class Squirrel3
{
    private const uint Noise1 = 0xb5297a4d;
    private const uint Noise2 = 0x68e31da4;
    private const uint Noise3 = 0x1b56c4e9;
    private const uint Cap = uint.MaxValue;

    /// <summary>
    /// Number of randoms generated so far
    /// </summary>
    private int _n;
    /// <summary>
    /// Seed of the generator
    /// </summary>
    private readonly int _seed;
        
    /// <summary>
    /// Creates a new generator with a "random" seed
    /// </summary>
    public Squirrel3()
    {
        _seed = Environment.TickCount;
    }
    
    /// <summary>
    /// Creates a new generator with a pre-set seed
    /// </summary>
    /// <param name="seed">The seed</param>
    public Squirrel3(int seed)
    {
        _seed = seed;
    }
        
    /// <summary>
    /// Creates a new generator with pre-set state
    /// </summary>
    /// <param name="state">The state in order Seed, N</param>
    public Squirrel3(Tuple<int, int> state)
    {
        _seed = state.Item1;
        _n = state.Item2;
    }
        
    /// <summary>
    /// Creates a new generator with a pre-set state
    /// </summary>
    /// <param name="seed">The seed</param>
    /// <param name="n">Number of picks from the seed</param>
    public Squirrel3(int seed, int n)
    {
        _seed = seed;
        _n = n;
    }
    
    /// <summary>
    /// Gets the next random float
    /// </summary>
    /// <returns>A random float</returns>
    public float Next()
    {
        _n++;
        return Rnd(_n, _seed) / (float)Cap;
    }
        
    /// <summary>
    /// Generates a random number in a range
    /// </summary>
    /// <param name="min">Inclusive minimum</param>
    /// <param name="max">Exclusive minimum</param>
    /// <returns>An integer in the range</returns>
    public int Range(int min, int max)
    {
        return (int)Mathf.Lerp(min, max, Next());
    }
        
    /// <summary>
    /// Generates a random number in a range
    /// </summary>
    /// <param name="min">Inclusive minimum</param>
    /// <param name="max">Exclusive minimum</param>
    /// <returns>A float in the range</returns>
    public float Range(float min, float max)
    {
        return Mathf.Lerp(min, max, Next());
    }

    /// <summary>
    /// Gets the current state of the random generator
    /// </summary>
    /// <returns>A tuple of seed & count of randoms generated</returns>
    public Tuple<int, int> GetState()
    {
        return new Tuple<int, int>(_seed, _n);
    }

    /// <summary>
    /// Generates a random long
    /// </summary>
    /// <param name="n">The count of randoms generated</param>
    /// <param name="seed">The seed of the generator</param>
    /// <returns>The random long at `n` for the specified seed</returns>
    private static long Rnd(long n, int seed = 0)
    {
        n *= Noise1;
        n += seed;
        n ^= n >> 8;
        n += Noise2;
        n ^= n << 8;
        n *= Noise3;
        n ^= n >> 8;
        return n % Cap;
    }
}