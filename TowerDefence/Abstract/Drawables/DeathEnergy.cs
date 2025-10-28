using Godot;

namespace BestagonDefence.Abstract.Drawables;

/// <summary>
/// A container for a death energy drop
/// </summary>
public class DeathEnergy : Drawable
{
    /// <summary>
    /// What type of energy it is
    /// </summary>
    public int Value { get; private set; }

    // Animation
    private bool _isAnimating;
    private float _startTime;
    private Vector2 _startScale;
    private float _animationScale;

    /// <summary>
    /// Creates a new DeathEnergy
    /// </summary>
    /// <param name="position">The position to draw</param>
    /// <param name="spawnWave">Which wave dropped the energy</param>
    /// <param name="scale">How large to draw the drop</param>
    /// <param name="value">How much the drop is worth</param>
    /// <param name="texture">The image to draw for the drop</param>
    public DeathEnergy(Vector2 position, int spawnWave, Vector2 scale, int value, Texture2D texture) 
        : base(position, GD.Randi() % 180, spawnWave, scale, texture)
    {
        Value = value;
    }

    /// <summary>
    /// Updates the draw for the drop
    /// </summary>
    /// <param name="delta">How long since the last draw</param>
    /// <returns>If the drop should continue to be drawn</returns>
    public override bool _Process(float delta)
    {
        if (!_isAnimating)
            return true;
        
        float x = Time.GetTicksMsec() - _startTime;
        x /= 1000;
        // -0.8007x^{2}+0.5654x+1.092
        _animationScale = -17f * (x * x) + 6.85f * x + 1;
        Scale = _startScale * Mathf.Max(0f, _animationScale);
        return Scale.X > 0 || Scale.Y > 0;
    }
    
    /// <summary>
    /// Starts animating the drop
    /// </summary>
    public void Collect()
    {
        if (_isAnimating) return;
        
        _isAnimating = true;
        _startTime = Time.GetTicksMsec();
        _startScale = Scale;
    }
}