using System;
using Godot;

namespace BestagonDefence.Abstract.Attributes;

/// <summary>
/// Container for a stat that can be modified through modifiers
/// </summary>
[GlobalClass]
public partial class Attribute : Resource
{
    // Displays with fewer decimal places if so
    private const int LargeValue = 50;
    private const float Tolerance = 0.001f;

    /// <summary>
    /// Name of the attribute, should be the same as the Attribute's key
    /// </summary>
    [Export]
    public AttributeType Name;

    private float _base;
    /// <summary>
    /// Base value for the attribute prior to any modifiers applied
    /// </summary>
    [Export]
    public float Base
    {
        get => _base;
        private set
        {
            _base = value;
            UpdateValue();
        }
    }

    /// <summary>
    /// The modifiers applied to the attribute
    /// </summary>
    [Export]
    private Godot.Collections.Dictionary<Variant, Modifier> _modifiers = new();

    /// <summary>
    /// The current value of the attribute after modifiers have been applied
    /// </summary>
    public float Value { get; private set; }

    /// <summary>
    /// The minimum possible value for the attribute
    /// </summary>
    [Export]
    public float Min { get; private set; } = -Mathf.Inf;
    /// <summary>
    /// The maximum possible value for the attribute
    /// </summary>
    [Export]
    public float Max { get; private set; } = Mathf.Inf;
    
    /// <summary>
    /// The value of all the modifiers applied to the value
    /// </summary>
    private float Modifier => CalculateMod();
    
    /// <summary>
    /// Triggers when the value of the attribute is updated
    /// </summary>
    [Signal]
    public delegate void OnAttributeUpdatedEventHandler(Attribute attribute);

    /// <summary>
    /// Creates a new attribute
    /// </summary>
    public Attribute()
    {
        Name = AttributeType.Nil;
        _base = 1f;
        CallDeferred("UpdateValue");
    }
    
    /// <summary>
    /// Creates a new attribute and fills in starting variables
    /// </summary>
    /// <param name="attributeType">The type of attribute</param>
    /// <param name="base">The base value of the attribute</param>
    /// <param name="min">The minimum value of the attribute</param>
    /// <param name="max">The maximum value of the attribute</param>
    public Attribute(AttributeType attributeType, float @base, float min = -Mathf.Inf, float max = Mathf.Inf)
    {
        Name = attributeType;
        _base = @base;
        Min = min;
        Max = max;
        CallDeferred("UpdateValue");
    }
    
    /// <summary>
    /// Creates a clone of the attribute
    /// </summary>
    /// <param name="attribute">The attribute to clone</param>
    public Attribute(Attribute attribute)
    {
        Name = attribute.Name;
        _base = attribute.Base;
        _modifiers = attribute._modifiers.Duplicate(true);
        Min = attribute.Min;
        Max = attribute.Max;
        CallDeferred("UpdateValue");
    }

    /// <summary>
    /// Copies the data from another attribute
    /// </summary>
    /// <param name="attribute">The attribute to copy</param>
    public void CopyFrom(Attribute attribute)
    {
        if (attribute.Name != Name)
        {
            GD.PushError("Attempted to copy from different attribute type!");
            return;
        }

        _base = attribute.Base;
        _modifiers = attribute._modifiers.Duplicate(true);
        Min = attribute.Min;
        Max = attribute.Max;
        UpdateValue();
    }

    /// <summary>
    /// Updates the value of the attribute if it's different
    /// </summary>
    private void UpdateValue()
    {
        float newVal = CalculateValue();
        
        if (!(Math.Abs(newVal - Value) > Tolerance)) return;
        
        Value = newVal;
    }

    /// <summary>
    /// Adds a new or updates an existing modifier to the attribute
    /// </summary>
    /// <param name="key">The key for the modifier, if it already exists, updates the AttributeModifier</param>
    /// <param name="mod">The AttributeModifier to apply to the attribute</param>
    public void Add(Variant key, Modifier mod)
    {
        mod.Uid = key;
        _modifiers[key] = mod;
        UpdateValue();
        
        EmitSignal(SignalName.OnAttributeUpdated, this);
    }

    /// <summary>
    /// Removes a modifier from the attribute
    /// </summary>
    /// <param name="key">The key of the modifier to remove</param>
    /// <returns></returns>
    public bool Remove(Variant key)
    {
        bool result = _modifiers.Remove(key);
        UpdateValue();
        
        EmitSignal(SignalName.OnAttributeUpdated, this);
        return result;
    }

    /// <summary>
    /// Checks if the attribute already contains a modifier with this key
    /// </summary>
    /// <param name="key">The key to check against</param>
    /// <returns>`true` if a modifier is applied to the attribute with this key</returns>
    public bool Contains(Variant key)
    {
        return _modifiers.ContainsKey(key);
    }
    
    /// <summary>
    /// Converts the value of the attribute for easy display
    /// </summary>
    /// <returns>A formatted string of the attribute value</returns>
    public override string ToString()
    {
        return Value > LargeValue ? $"{Value:#,##0.#}" : $"{Value:#0.0#}";
    }
    
    /// <summary>
    /// Calculates a value of the attribute from a given base value
    /// </summary>
    /// <returns>The value of the attribute</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if a modifier has an invalid operation</exception>
    private float CalculateValue()
    {
        float val = Base;
        float addAfter = 0;
        float additive = 1;
        float multiplicative = 1;
        float min = Min;
        float addiMin = 1;
        float multMin = 1;
        float max = Max;
        float addiMax = 1;
        float multMax = 1;
        foreach (Modifier mod in _modifiers.Values)
        {
            switch (mod.Op)
            {
                case Operation.Add:
                    val += mod.Value;
                    break;
                case Operation.Additive:
                    additive += mod.Value;
                    break;
                case Operation.Multiplicative:
                    multiplicative += mod.Value;
                    break;
                case Operation.OneMinusMultiplicative:
                    multiplicative += 1 - mod.Value;
                    break;
                case Operation.AddAfter:
                    addAfter += mod.Value;
                    break;
                case Operation.AddMin:
                    min += mod.Value;
                    break;
                case Operation.AdditiveMin:
                    addiMin += mod.Value;
                    break;
                case Operation.MultiplicativeMin:
                    multMin += mod.Value;
                    break;
                case Operation.AddMax:
                    max += mod.Value;
                    break;
                case Operation.AdditiveMax:
                    addiMax += mod.Value;
                    break;
                case Operation.MultiplicativeMax:
                    multMax += mod.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_modifiers), message:"Invalid modifier found: " + mod.Value);
            }
        }
    
        val *= additive * multiplicative;
        min *= addiMin * multMin;
        max *= addiMax * multMax;
        val += addAfter;
        
        if (val < min)
            return min;

        if (val > max)
            return max;
        
        return val;
    }

    /// <summary>
    /// Calculates the value of the attribute's modifiers
    /// </summary>
    /// <returns>The value of the attribute's modifiers</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if a modifier has an invalid operation</exception>
    private float CalculateMod()
    {
        float val = 1;
        float after = 0;
        float additive = 1;
        float multiplicative = 1;
        float min = Min;
        float addiMin = 1;
        float multMin = 1;
        float max = Max;
        float addiMax = 1;
        float multMax = 1;
        foreach (Modifier mod in _modifiers.Values)
        {
            switch (mod.Op)
            {
                case Operation.Add:
                    val += mod.Value;
                    break;
                case Operation.Additive:
                    additive += mod.Value;
                    break;
                case Operation.Multiplicative:
                    multiplicative += mod.Value;
                    break;
                case Operation.OneMinusMultiplicative:
                    multiplicative += 1 - mod.Value;
                    break;
                case Operation.AddAfter:
                    after += mod.Value;
                    break;
                case Operation.AddMin:
                    min += mod.Value;
                    break;
                case Operation.AdditiveMin:
                    addiMin += mod.Value;
                    break;
                case Operation.MultiplicativeMin:
                    multMin += mod.Value;
                    break;
                case Operation.AddMax:
                    max += mod.Value;
                    break;
                case Operation.AdditiveMax:
                    addiMax += mod.Value;
                    break;
                case Operation.MultiplicativeMax:
                    multMax += mod.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_modifiers), message:"Invalid modifier found: " + mod.Value);
            }
        }
    
        val *= additive * multiplicative;
        min *= addiMin * multMin;
        max *= addiMax * multMax;
        val += after;
        
        if (val < min)
            return min;

        if (val > max)
            return max;
        
        return val;
    }
}