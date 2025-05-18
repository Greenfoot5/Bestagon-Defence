using Godot;
using Vector2 = Godot.Vector2;

namespace Gameplay;

/// <summary>
/// Represents a "thing" we can draw without depending on a node
/// </summary>
public abstract class Drawable
{
    /// <summary>
    /// The current position of the Drawable
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// The current rotation of the drawable
    /// </summary>
    public float Rotation;
    /// <summary>
    /// When the turret spawned in, 
    /// </summary>
    public int StartTime;
    
    // Visuals
    /// <summary>
    /// The scale to apply to the texture
    /// </summary>
    public Vector2 Scale;
    /// <summary>
    /// The texture to use
    /// </summary>
    public Texture2D Texture;

    protected Drawable(Vector2 position, float rotation, int startTime, Vector2 scale, Texture2D texture)
    {
        Position = position;
        Rotation = rotation;
        StartTime = startTime;
        Scale = scale;
        Texture = texture;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    public abstract bool Update(float delta);
}