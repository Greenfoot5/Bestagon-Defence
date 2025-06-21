using Godot;

namespace BestagonDefense.Abstract.Attributes;

/// <summary>
/// A value modifier to an attribute(s)
/// </summary>
[GlobalClass]
[Tool]
public partial class Modifier : Resource
{
    public Variant Uid { get; set; }
    /// <summary>
    /// The value to modify an attribute by
    /// </summary>
    [Export]
    public float Value { get; private set; }
    /// <summary>
    /// How to use the value to modify an attribute
    /// </summary>
    [Export]
    public Operation Op { get; private set; }

    /// <summary>
    /// Creates a default AttributeModifier
    /// </summary>
    public Modifier()
    {
        Value = 0;
        Op = Operation.Additive;
    }
    
    /// <summary>
    /// Creates a new Additive AttributeModifier with a value
    /// </summary>
    /// <param name="value">The base value for the AttributeModifier</param>
    public Modifier(float value)
    {
        Op = Operation.Additive;
        Value = value;
    }

    /// <summary>
    /// Creates a new AttributeModifier
    /// </summary>
    /// <param name="value">The value of the modifier</param>
    /// <param name="op">The operation the modifier performs</param>
    public Modifier(float value, Operation op)
    {
        Op = op;
        Value = value;
    }
}

