namespace BestagonDefence.Abstract.Attributes;

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
    /// Base * Additive * Multiplicative + AddAfter
    /// </summary>
    AddAfter,
    
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