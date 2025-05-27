using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Turrets;

namespace Abstract.Data;

/// <summary>
/// A way to disable duplicates in random selections
/// </summary>
public enum DuplicateTypes
{
    None,
    ByName,
    ByType
}

public enum Strain
{
    TurretBlueprint,
    ModuleChainHandler
}
    
/// <summary>
/// A list of items and their weight.
/// Can get a random item and total weight of the values
/// </summary>
[Serializable]
[Tool]
public partial class WeightedList : Resource
{
    private Array<TurretBlueprint> blueprints = [];
    private Array<ModuleChainHandler> handlers = [];
    private Array<float> weights = [];
    
    private Strain strain;
    
    private int _size;
    
    public int Count
    {
        get => _size;
        set
        {
            _size = value;
            blueprints.Resize(_size);
            handlers.Resize(_size);
            weights.Resize(_size);
            NotifyPropertyListChanged();
        }
    }

    public WeightedList(WeightedList list)
    {
        Count = list.Count;
        blueprints = new Array<TurretBlueprint>(list.blueprints);
        handlers = new Array<ModuleChainHandler>(list.handlers);
        weights = new Array<float>(list.weights);
        strain = list.strain;
    }

    public WeightedList(Strain strain)
    {
        this.strain = strain;
    }

    public WeightedList()
    {
        strain = Strain.TurretBlueprint;
    }

    /// <summary>
    /// Gets random items from the list using the weights
    /// </summary>
    /// <param name="duplicateType">Which duplication type to use when checking against previously picked</param>
    /// <param name="rng">The random generator to use</param>
    /// <param name="previousPicks">The previous picks to check against</param>
    /// <returns>A random item</returns>
    /// <exception cref="NullReferenceException">The list isn't suitable to grant all items</exception>
    public T GetRandomItem<[MustBeVariant] T>(DuplicateTypes duplicateType = DuplicateTypes.None, Squirrel3 rng = null, ICollection<T> previousPicks = null)
        where T : Resource, ISubtypeable
    {
        rng ??= new Squirrel3();
        previousPicks ??= new Array<T>();
        var items = new WeightedList(this);
            
        foreach (T pick in previousPicks)
        {
            GD.Print(pick.ToString());
            switch (duplicateType)
            {
                case DuplicateTypes.ByName:
                    for (var k = 0; k < items.Count; k++)
                    {
                        if (items.GetItemAsResource(k).ToString() == pick.ToString())
                        {
                            GD.Print("Removing by name " + items.GetItemAsResource(k));
                            items.RemoveAt(k);
                        }
                    }

                    break;
                case DuplicateTypes.ByType:
                    for (var k = 0; k < items.Count; k++)
                    {
                        if (items.GetItemAsSubtypable(k).GetSubtype() == pick.GetSubtype())
                        {
                            items.RemoveAt(k);
                        }
                    }

                    break;
                case DuplicateTypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(duplicateType), duplicateType, null);
            }
        }

        float total = items.GetTotalWeight();
        if (items.Count < 1) throw new NullReferenceException("WeightedList is not large enough");
        if (total == 0) throw new NullReferenceException("Total Weight is 0");

        // Downgrade duplicateTypes if we can't generate
        if (total == 0)
        {
            GD.PushWarning("Could not use " + duplicateType + " duplicate checking, downgrading and regenerating.");
            duplicateType = duplicateType switch
            {
                DuplicateTypes.ByName => DuplicateTypes.ByType,
                DuplicateTypes.ByType => DuplicateTypes.None,
                _ => duplicateType
            };
            // TODO - Check if Rider conversion to iteration is better
            return GetRandomItem(duplicateType, rng, previousPicks);
        }
            
        float picked = rng.Next() * total;
        var j = 0;
        while (picked >= 0)
        {
            if (picked < items.weights[j])
            {
                return strain switch
                {
                    Strain.TurretBlueprint => items.blueprints[j] as T,
                    Strain.ModuleChainHandler => items.handlers[j] as T,
                    _ => null
                };
            }

            picked -= items.weights[j];
            j++;
        }

        throw new NullReferenceException("Failed to pick an item");
    }

    /// <summary>
    /// Gets random items from the list using the weights
    /// </summary>
    /// <param name="count">How many to pick</param>
    /// <param name="duplicateType">Which duplication type to use when checking against previously picked</param>
    /// <param name="rng">The random generator to use</param>
    /// <returns>A random item</returns>
    /// <exception cref="NullReferenceException">The list isn't suitable to grant all items</exception>
    public T[] GetRandomItems<[MustBeVariant] T>(int count, DuplicateTypes duplicateType = DuplicateTypes.None, Squirrel3 rng = null)
        where T : Resource, ISubtypeable
    {
        rng ??= new Squirrel3();
        Math.Clamp(count, 0, int.MaxValue);
        float total = GetTotalWeight();
        if (Count < count) throw new NullReferenceException("WeightedList is not large enough");
        if (total == 0) throw new NullReferenceException("Total Weight is 0");
        
        var output = new T[count];
        // Make a copy we can remove items from
        var items = new WeightedList(this);

        for (var i = 0; i < count; i++)
        {
            // Downgrade duplicateTypes if we can't generate
            if (total == 0)
            {
                GD.PushWarning("Could not use " + duplicateType + " duplicate checking, downgrading and regenerating.");
                duplicateType = duplicateType switch
                {
                    DuplicateTypes.ByName => DuplicateTypes.ByType,
                    DuplicateTypes.ByType => DuplicateTypes.None,
                    _ => duplicateType
                };
                return GetRandomItems<T>(count, duplicateType, rng);
            }
                
            float picked = rng.Next() * total;
            var j = 0;
            while (picked >= 0 && output[i] == null)
            {
                if (picked < items.weights[j])
                {
                    output[i] = strain switch
                    {
                        Strain.TurretBlueprint => blueprints[j] as T,
                        Strain.ModuleChainHandler => handlers[j] as T,
                        _ => null
                    };
                }
                else
                {
                    picked -= items.weights[j];
                    j++;
                }
            }


            switch (duplicateType)
            {
                case DuplicateTypes.ByName:
                    for (var k = 0; k < items.Count; k++)
                    {
                        if (items.GetItemAsResource(k).ToString() == output[i].ToString())
                            items.RemoveAt(k);
                    }
                    break;
                case DuplicateTypes.ByType:
                    for (var k = 0; k < items.Count; k++)
                    {
                        if (items.GetItemAsSubtypable(k).GetSubtype() == output[i].GetSubtype())
                            items.RemoveAt(k);
                    }
                    break;
                case DuplicateTypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(duplicateType), duplicateType, null);
            }

            total = items.GetTotalWeight();
        }

        return output;
    }
    
    public Resource GetItemAsResource(int key)
    {
        return strain switch
        {
            Strain.TurretBlueprint => blueprints[key],
            Strain.ModuleChainHandler => handlers[key],
            _ => null
        };
    }
    
    public ISubtypeable GetItemAsSubtypable(int key)
    {
        return strain switch
        {
            Strain.TurretBlueprint => blueprints[key],
            Strain.ModuleChainHandler => handlers[key],
            _ => null
        };
    }
    
    public ModuleChainHandler GetHandler(int key)
    {
        return handlers[key];
    }
    
    /// <summary>
    /// Gets the total weight of all elements in the list combined
    /// </summary>
    /// <returns>The total overall weight</returns>
    /// <exception cref="NullReferenceException">The list is empty</exception>
    public float GetTotalWeight()
    {
        if (Count == 0) throw new NullReferenceException("WeightedList is empty");

        return weights.Where(weight => weight > 0).Sum();
    }
        
    public void RemoveAt(int index)
    {
        blueprints.RemoveAt(index);
        handlers.RemoveAt(index);
        weights.RemoveAt(index);
        Count--;
    }
    
    public void Add(TurretBlueprint blueprint, ModuleChainHandler handler, float weight)
    {
        blueprints.Add(blueprint);
        handlers.Add(handler);
        weights.Add(weight);
        Count++;
    }

    public void RemoveUnweighted()
    {
        // We can remove everything if the total weight is 0
        if (GetTotalWeight() == 0)
            Clear();
            
        var i = 0;
        while (i < Count)
        {
            if (weights[i] <= 0)
                RemoveAt(i);
            else
                i++;
        }
    }
    
    /// <summary>
    /// Empties/Cleans the list of all elements
    /// </summary>
    public void Clear()
    {
        blueprints.Clear();
        handlers.Clear();
        weights.Clear();
        Count = 0;
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }
    
    public override Array<Dictionary> _GetPropertyList()
    {
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
                { "name", $"Count" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.None },
                { "usage", (int)PropertyUsageFlags.Array + (int)PropertyUsageFlags.Default },
                { "class_name", "Items,list_" }
            }
        ];

        for (var i = 0; i < _size; i++)
        {
            if (strain == Strain.TurretBlueprint)
                properties.Add(new Dictionary()
                {
                    { "name", $"list_{i}/Blueprint" },
                    { "type", (int)Variant.Type.Object },
                    { "hint", (int)PropertyHint.ResourceType },
                    { "hint_string", nameof(TurretBlueprint) },
                    { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
                });
            else if (strain == Strain.ModuleChainHandler)
                properties.Add(new Dictionary()
                {
                    { "name", $"list_{i}/Handler" },
                    { "type", (int)Variant.Type.Object },
                    { "hint", (int)PropertyHint.ResourceType },
                    { "hint_string", nameof(ModuleChainHandler) },
                    { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
                });
            properties.Add(new Dictionary()
            {
                { "name", $"list_{i}/Weight" },
                { "type", (int)Variant.Type.Float },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "0,20,,or_greater,hide_slider" },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
            });
        }
    
        return properties;
    }
    
    public override Variant _Get(StringName property)
    {
        var propertyName = property.ToString();
        if (propertyName.StartsWith("list_"))
        {
            string[] split = propertyName.Split('/');
            int index = int.Parse(split[0]["list_".Length..]);
            switch (split[1])
            {
                case "Weight":
                    return weights[index];
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
            case "Count":
                return Count;
        }

        return default;
    }

    public override bool _Set(StringName property, Variant value)
    {
        var propertyName = property.ToString();
        if (propertyName.StartsWith("list_"))
        {
            string[] split = propertyName.Split('/');
            int index = int.Parse(split[0]["list_".Length..]);
            switch (split[1])
            {
                case "Weight":
                    weights[index] = value.As<float>();
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
            case "Count":
                Count = value.As<int>();
                return true;
        }
        
        return false;
    }
}