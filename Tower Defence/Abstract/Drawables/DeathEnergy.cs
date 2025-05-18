using Godot;

namespace Gameplay;

public class DeathEnergy : Drawable
{
    /// <summary>
    /// What type of energy it is
    /// </summary>
    public int Value { get; private set; }

    // Animation
    public bool _isAnimating;
    private float _startTime;
    private Vector2 _startScale;
    private float _animationScale;

    public DeathEnergy(Vector2 position, int spawnWave, Vector2 scale, int value, Texture2D texture) 
        : base(position, GD.Randi() % 180, spawnWave, scale, texture)
    {
        Value = value;
    }

    public override bool Update(float delta)
    {
        if (!_isAnimating)
            return true;
        
        float x = Time.GetTicksMsec() - _startTime;
        x /= 1000;
        // -0.8007x^{2}+0.5654x+1.092
        _animationScale = -17f * (x * x) + 6.85f * x + 1;
        Scale = _startScale * Mathf.Max(0f, _animationScale);
        return Scale.X > 0 || Scale.Y > 0;

        //     return Matrix4x4.TRS(Position, _rotation, _scale * Mathf.Max(0f, _animationScale));
    }

    public void Collect()
    {
        if (_isAnimating) return;
        
        _isAnimating = true;
        _startTime = Time.GetTicksMsec();
        _startScale = Scale;
    }
}