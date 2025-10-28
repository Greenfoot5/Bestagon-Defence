using Godot;
using Vector2 = Godot.Vector2;

namespace BestagonDefence.Abstract.Drawables;

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
    public readonly int StartTime;
    
    // Visuals
    /// <summary>
    /// The scale to apply to the texture
    /// </summary>
    public Vector2 Scale;
    /// <summary>
    /// The texture to use
    /// </summary>
    public readonly Texture2D Texture;

    /// <summary>
    /// Creates a new drawable
    /// </summary>
    /// <param name="position">The position to draw the drawable at</param>
    /// <param name="rotation">The rotation of the drawable</param>
    /// <param name="startTime">When the drawable was created</param>
    /// <param name="scale">How large to draw the drawable</param>
    /// <param name="texture">The texture to draw with</param>
    protected Drawable(Vector2 position, float rotation, int startTime, Vector2 scale, Texture2D texture)
    {
        Position = position;
        Rotation = rotation;
        StartTime = startTime;
        Scale = scale;
        Texture = texture;
    }

    /// <summary>
    /// Updates the drawable
    /// </summary>
    /// <param name="delta">Time since last frame</param>
    /// <returns>false if the object should stop being drawn</returns>
    public abstract bool _Process(float delta);
}