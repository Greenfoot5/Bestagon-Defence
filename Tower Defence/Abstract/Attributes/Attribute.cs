using System;
using System.Collections.Generic;
using Godot;

namespace Abstract.Attributes;

[GlobalClass]
public partial class Attribute : Resource
{
    // Displays with fewer decimal places if so
    private const int LargeValue = 50;
    private const float Tolerance = 0.001f;

    [Export]
    public AttributeType Name;
    [Export]
    public float Base;
    [Export]
    private Godot.Collections.Dictionary<Variant, AttributeModifier> _modifiers = new();
    
    private float _value;
    public float Value => CalculateValue(Base, Attributes.Global[Name]._modifiers);

    [Export]
    public float Min { get; set; } = -Mathf.Inf;
    [Export]
    public float Max { get; set; } = Mathf.Inf;
    
    [Signal]
    public delegate void AttributeUpdatedEventHandler(float newValue);

    public Attribute()
    {
        Name = AttributeType.Nil;
        Base = 1;
        _value = 1;
    }
    
    public Attribute(float @base, float min = -Mathf.Inf, float max = Mathf.Inf)
    {
        Base = @base;
        _value = @base;
        Min = min;
        Max = max;
    }
    
    public Attribute(AttributeType attributeType, float @base, float min = -Mathf.Inf, float max = Mathf.Inf)
    {
        Name = attributeType;
        Base = @base;
        _value = @base;
        Min = min;
        Max = max;
    }
    
    public Attribute(Attribute attribute)
    {
        Name = attribute.Name;
        Base = attribute.Base;
        _modifiers = attribute._modifiers.Duplicate(true);
        _value = attribute.Value;
    }

    /// <summary>
    /// Gets the value without min/max checks
    /// Also ignores any global modifiers
    /// </summary>
    /// <returns>The current unrestricted attribute value</returns>
    public float GetTrueValue()
    {
        return _value;
    }
    
    public AttributeModifier this[Variant key] => _modifiers.TryGetValue(key, out AttributeModifier item) ? item : new AttributeModifier();
    
    public ICollection<Variant> Keys => _modifiers.Keys;

    public void Add(Variant key, AttributeModifier mod)
    {
        mod.Uid = key;
        _modifiers[key] = mod;
        float newVal = CalculateValue(Base, _modifiers);
        
        if (!(Math.Abs(newVal - _value) > Tolerance)) return;
        
        _value = newVal;
        EmitSignal(SignalName.AttributeUpdated, _value);
    }

    public bool Remove(Variant key)
    {
        bool result = _modifiers.Remove(key);
        float newVal = CalculateValue(Base, _modifiers);
        
        if (!(Math.Abs(newVal - _value) > Tolerance)) return result;
        
        _value = newVal;
        EmitSignal(SignalName.AttributeUpdated, _value);
        
        return result;
    }

    public bool Contains(Variant key)
    {
        return _modifiers.ContainsKey(key);
    }
    
    public override string ToString()
    {
        return Value > LargeValue ? $"{Value:#,##0.#}" : $"{Value:#0.0#}";
    }
    
    private float CalculateValue(float start, Godot.Collections.Dictionary<Variant, AttributeModifier> modifiers)
    {
        float val = start;
        float additive = 1;
        float multiplicative = 1;
        float min = Min;
        float addiMin = 1;
        float multMin = 1;
        float max = Max;
        float addiMax = 1;
        float multMax = 1;
        foreach (AttributeModifier mod in modifiers.Values)
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
                    throw new ArgumentOutOfRangeException(nameof(modifiers), message:"Invalid modifier found: " + mod.Value);
            }
        }
    
        val *= additive * multiplicative;
        min *= addiMin * multMin;
        max *= addiMax * multMax;
        
        if (val < min)
            return min;

        if (val > max)
            return max;
        
        return val;
    }
}