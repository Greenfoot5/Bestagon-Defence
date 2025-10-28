using System;
using BestagonDefence.Turrets;
using Godot;
using Godot.Collections;

namespace BestagonDefence.Abstract.Data;

/// <summary>
/// A list of items and their weight.
/// Can get a random item and total weight of the values
/// </summary>
[Serializable]
[Tool]
public partial class WeightedCurveList : Resource
{
    /// <summary>
    /// The type of the WeightedCurveList
    /// </summary>
    private Strain strain;
    
    // Data Containers
    private Array<TurretBlueprint> blueprints = [];
    private Array<ModuleChainHandler> handlers = [];
    private Array<Curve> curves = [];
    
    private int _size;
    /// <summary>
    /// The Size of the List
    /// </summary>
    public int Size
    {
        get => _size;
        set
        {
            _size = value;
            blueprints.Resize(_size);
            handlers.Resize(_size);
            curves.Resize(_size);
            NotifyPropertyListChanged();
        }
    }
    
    /// <summary>
    /// Creates a WeightedCurveList of the specified strain
    /// </summary>
    /// <param name="strain">The strain of the WCL</param>
    public WeightedCurveList(Strain strain)
    {
        this.strain = strain;
    }

    /// <summary>
    /// Creates a WCL of strain TurretBlueprint
    /// </summary>
    public WeightedCurveList()
    {
        strain = Strain.TurretBlueprint;
    }

    /// <summary>
    /// Converts the WeightedCurveList to a WeightedList at a certain time
    /// </summary>
    /// <param name="time">The time to get the weight from the AnimationCurves</param>
    /// <returns>The WeightedList for a specific time</returns>
    public WeightedList ToWeightedList(float time)
    {
        var weightedList = new WeightedList(strain);
        
        for (var i = 0; i < Size; i++)
        {
            if (curves[i].Sample(time) > 0)
                weightedList.Add(blueprints[i], handlers[i], curves[i].Sample(time));
        }

        return weightedList;
    }
    
    /// <summary>
    /// Gets the property list to display in the editor
    /// </summary>
    /// <returns>An array of properties to display and how</returns>
    public override Array<Dictionary> _GetPropertyList()
    {
        // Base List properties
        Array<Dictionary> properties =
        [
            new()
            {
                { "name", "Strain" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Enum },
                { "hint_string", "Turret Blueprint, Module Chain Handler" },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage + (int)PropertyUsageFlags.ReadOnly }
            },
            new()
            {
                { "name", "Size" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.None },
                { "usage", (int)PropertyUsageFlags.Array + (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage },
                { "class_name", "Items,list_" }
            }
        ];

        // List Data
        for (var i = 0; i < _size; i++)
        {
            switch (strain)
            {
                case Strain.TurretBlueprint:
                    properties.Add(new Dictionary()
                    {
                        { "name", $"list_{i}/Blueprint" },
                        { "type", (int)Variant.Type.Object },
                        { "hint", (int)PropertyHint.ResourceType },
                        { "hint_string", nameof(TurretBlueprint) },
                        { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
                    });
                    break;
                case Strain.ModuleChainHandler:
                    properties.Add(new Dictionary()
                    {
                        { "name", $"list_{i}/Handler" },
                        { "type", (int)Variant.Type.Object },
                        { "hint", (int)PropertyHint.ResourceType },
                        { "hint_string", nameof(ModuleChainHandler) },
                        { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            properties.Add(new Dictionary()
            {
                { "name", $"list_{i}/Curve" },
                { "type", (int)Variant.Type.Object },
                { "hint", (int)PropertyHint.ResourceType },
                { "hint_string", "Curve" },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
            });
        }
    
        return properties;
    }
    
    /// <summary>
    /// Gets the value for a given property
    /// </summary>
    /// <param name="property">The property to get the data for</param>
    /// <returns>The data for that property (if it exists)</returns>
    public override Variant _Get(StringName property)
    {
        var propertyName = property.ToString();
        if (propertyName.StartsWith("list_"))
        {
            string[] split = propertyName.Split('/');
            int index = int.Parse(split[0]["list_".Length..]);
            switch (split[1])
            {
                case "Curve":
                    return curves[index];
                case "Blueprint":
                    return blueprints[index];
                case "Handler":
                    return handlers[index];
                default:
                    GD.PrintErr("Invalid property name in WeightedList for WeightedItem: " + split[1]);
                    break;
            }
        }
        else switch (propertyName)
        {
            case "Strain":
                return Variant.From(strain);
            case "Size":
                return Size;
        }

        return default;
    }

    /// <summary>
    /// Sets the value for a given property
    /// </summary>
    /// <param name="property">The property to update</param>
    /// <param name="value">The new value of the property</param>
    /// <returns>true if the value was updated</returns>
    public override bool _Set(StringName property, Variant value)
    {
        var propertyName = property.ToString();
        if (propertyName.StartsWith("list_"))
        {
            string[] split = propertyName.Split('/');
            int index = int.Parse(split[0]["list_".Length..]);
            switch (split[1])
            {
                case "Curve":
                    curves[index] = value.As<Curve>();
                    NotifyPropertyListChanged();
                    return true;
                case "Blueprint":
                    blueprints[index] = value.As<TurretBlueprint>();
                    NotifyPropertyListChanged();
                    return true;
                case "Handler":
                    handlers[index] = value.As<ModuleChainHandler>();
                    NotifyPropertyListChanged();
                    return true;
                default:
                    GD.PrintErr("Invalid property name in WeightedList for WeightedItem: " + split[1]);
                    break;
            }
        }
        else switch (propertyName)
        {
            case "Strain":
                strain = value.As<Strain>();
                NotifyPropertyListChanged();
                return true;
            case "Size":
                Size = value.As<int>();
                return true;
        }
        
        return false;
    }
}