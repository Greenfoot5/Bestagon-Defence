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

    private float _base;
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

    [Export]
    private Godot.Collections.Dictionary<Variant, AttributeModifier> _modifiers = new();

    public float Value { get; private set; }

    public float Modifier => CalculateMod(_modifiers);

    [Export]
    public float Min { get; private set; } = -Mathf.Inf;
    [Export]
    public float Max { get; private set; } = Mathf.Inf;
    
    [Signal]
    public delegate void AttributeUpdatedEventHandler(Attribute attribute);

    public Attribute()
    {
        Name = AttributeType.Nil;
        Base = 1f;
        UpdateValue();
    }
    
    public Attribute(AttributeType attributeType, float @base, float min = -Mathf.Inf, float max = Mathf.Inf)
    {
        Name = attributeType;
        Base = @base;
        Min = min;
        Max = max;
        UpdateValue();
    }
    
    public Attribute(Attribute attribute)
    {
        Name = attribute.Name;
        Base = attribute.Base;
        _modifiers = attribute._modifiers.Duplicate(true);
        Min = attribute.Min;
        Max = attribute.Max;
        UpdateValue();
    }

    public void CopyFrom(Attribute attribute)
    {
        if (attribute.Name != Name)
        {
            GD.PushError("Attempted to copy from different attribute type!");
            return;
        }

        Base = attribute.Base;
        _modifiers = attribute._modifiers.Duplicate(true);
        Min = attribute.Min;
        Max = attribute.Max;
        UpdateValue();
    }

    private void UpdateValue()
    {
        float newVal = CalculateValue(Base, _modifiers);
        
        if (!(Math.Abs(newVal - Value) > Tolerance)) return;
        
        Value = newVal;
    }
    
    public AttributeModifier this[Variant key] => _modifiers.TryGetValue(key, out AttributeModifier item) ? item : new AttributeModifier();
    
    public ICollection<Variant> Keys => _modifiers.Keys;

    public void Add(Variant key, AttributeModifier mod)
    {
        mod.Uid = key;
        _modifiers[key] = mod;
        UpdateValue();
        // Value = newVal;
        EmitSignal(SignalName.AttributeUpdated, this);
    }

    public bool Remove(Variant key)
    {
        bool result = _modifiers.Remove(key);
        UpdateValue();
        
        // Value = newVal;
        EmitSignal(SignalName.AttributeUpdated, this);
        
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
        float addAfter = 0;
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
                    throw new ArgumentOutOfRangeException(nameof(modifiers), message:"Invalid modifier found: " + mod.Value);
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

    private float CalculateMod(Godot.Collections.Dictionary<Variant, AttributeModifier> modifiers)
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
                    throw new ArgumentOutOfRangeException(nameof(modifiers), message:"Invalid modifier found: " + mod.Value);
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