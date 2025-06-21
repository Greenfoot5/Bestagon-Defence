using Godot;

namespace BestagonDefense.Turrets;

public abstract partial class PlacedObject : Node2D
{
    // TODO - Use localised
    /// <summary>
    /// The display name of the object
    /// </summary>
    public string DisplayName;

    /// <summary>
    /// Called when the object is selected
    /// </summary>
    public abstract void Selected();

    /// <summary>
    /// Called when the object is deselected
    /// </summary>
    public abstract void Deselected();
}