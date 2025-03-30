using Godot;

namespace Abstract.Attributes;

/// <summary>
/// A value modifier to an attribute
/// </summary>
[GlobalClass]
public partial class AttributeModifier : Resource
{
    public Variant Uid { get; set; }
    public float Value { get; set; }
    public Operation Op { get; set; }

    public AttributeModifier()
    {
        Value = 0;
        Op = Operation.Additive;
    }
    
    public AttributeModifier(float value)
    {
        Op = Operation.Additive;
        Value = value;
    }

    public AttributeModifier(float value, Operation op)
    {
        Op = op;
        Value = value;
    }
}

/// <summary>
/// Different methods of modifying an attribute
/// </summary>
public enum Operation
{
    /// <summary>
    /// Base + ModifierValueOne + ModifierValueTwo
    /// </summary>
    Add,
    /// <summary>
    /// Base * (ModifierValueOne + ModifierValueTwo)
    /// </summary>
    Additive,
    /// <summary>
    /// Base * ModifierValueOne * ModifierValueTwo
    /// </summary>
    Multiplicative,
    /// <summary>
    /// Base * (1 - ModifierValueOne) * (1 - ModifierValueTwo)
    /// </summary>
    OneMinusMultiplicative,
    /// <summary>
    /// Adds to the attribute's minimum
    /// To take away, use a negative value
    /// </summary>
    AddMin,
    /// <summary>
    /// AttributeMin * (ModifierValueOne + ModifierValueTwo)
    /// </summary>
    AdditiveMin,
    /// <summary>
    /// AttributeMin * ModifierValueOne * ModifierValueTwo
    /// </summary>
    MultiplicativeMin,
    /// <summary>
    /// Adds to the attribute's maximum value
    /// To take away, use a negative value
    /// </summary>
    AddMax,
    /// <summary>
    /// AttributeMax * (ModifierValueOne + ModifierValueTwo)
    /// </summary>
    AdditiveMax,
    /// <summary>
    /// AttributeMax * ModifierValueOne * ModifierValueTwo
    /// </summary>
    MultiplicativeMax,
}