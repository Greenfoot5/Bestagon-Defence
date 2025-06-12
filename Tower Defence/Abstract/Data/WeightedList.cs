using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BestagonDefense.Turrets;
using Godot;
using Godot.Collections;

namespace BestagonDefense.Abstract.Data;
    
/// <summary>
/// A list of items and their weight.
/// Can get a random item and total weight of the values
/// </summary>
[Serializable]
[Tool]
public partial class WeightedList : Resource
{
    /// <summary>
    /// The type of the WeightedList
    /// </summary>
    private Strain strain;
    
    // Data Containers
    private Array<TurretBlueprint> blueprints = [];
    private Array<ModuleChainHandler> handlers = [];
    private Array<float> weights = [];
    
    private int _size;
    /// <summary>
    /// The Count (Size) of the List
    /// </summary>
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

    /// <summary>
    /// Creates a clone of another list
    /// </summary>
    /// <param name="list">The WeightedList to clone</param>
    public WeightedList(WeightedList list)
    {
        Count = list.Count;
        blueprints = new Array<TurretBlueprint>(list.blueprints);
        handlers = new Array<ModuleChainHandler>(list.handlers);
        weights = new Array<float>(list.weights);
        strain = list.strain;
    }

    /// <summary>
    /// Creates an empty WeightedList of a specific strain
    /// </summary>
    /// <param name="strain">The strain of the WeightedList</param>
    public WeightedList(Strain strain)
    {
        this.strain = strain;
    }

    /// <summary>
    /// Creates an empty WeightedList of TurretBlueprint strain
    /// </summary>
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
        where T : Resource, ISubtypable
    {
        rng ??= new Squirrel3();
        previousPicks ??= new Array<T>();
        var items = new WeightedList(this);
            
        foreach (T pick in previousPicks)
        {
            switch (duplicateType)
            {
                case DuplicateTypes.ByName:
                    for (var k = 0; k < items.Count; k++)
                    {
                        if (items.GetItemAsResource(k).ToString() == pick.ToString())
                            items.RemoveAt(k);
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
        where T : Resource, ISubtypable
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
    
    /// <summary>
    /// Returns the item at a specified key
    /// </summary>
    /// <param name="key">The key of the item to return</param>
    /// <returns>The blueprint/handler at the given key</returns>
    public Resource GetItemAsResource(int key)
    {
        return strain switch
        {
            Strain.TurretBlueprint => blueprints[key],
            Strain.ModuleChainHandler => handlers[key],
            _ => null
        };
    }
    
    /// <summary>
    /// Returns the subtypable item at a specified key
    /// </summary>
    /// <param name="key">The key of the item to return</param>
    /// <returns>The subtypable at the given key</returns>
    public ISubtypable GetItemAsSubtypable(int key)
    {
        return strain switch
        {
            Strain.TurretBlueprint => blueprints[key],
            Strain.ModuleChainHandler => handlers[key],
            _ => null
        };
    }
    
    /// <summary>
    /// Returns the handler at a key
    /// Ignores strain
    /// </summary>
    /// <param name="key">The key of the item in the List</param>
    /// <returns>The ModuleChainHandler at the key</returns>
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
    
    /// <summary>
    /// Removes a key & value pair at a specific position index
    /// </summary>
    /// <param name="index">The position index to remove the key, value pair from</param>
    public void RemoveAt(int index)
    {
        blueprints.RemoveAt(index);
        handlers.RemoveAt(index);
        weights.RemoveAt(index);
        Count--;
    }
    
    /// <summary>
    /// Adds a new value to the List
    /// </summary>
    /// <param name="blueprint">The TurretBlueprint to add. May be null</param>
    /// <param name="handler">The ModuleChainHandler to add. May be null</param>
    /// <param name="weight">The weight of the value</param>
    public void Add([MaybeNull] TurretBlueprint blueprint, [MaybeNull] ModuleChainHandler handler, float weight)
    {
        blueprints.Add(blueprint);
        handlers.Add(handler);
        weights.Add(weight);
        Count++;
    }

    /// <summary>
    /// Removes all entries in the list of a weight less than or equal to 0
    /// </summary>
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

    /// <summary>
    /// Checks if the List is empty
    /// </summary>
    /// <returns>true if there are no entries in the list</returns>
    public bool IsEmpty()
    {
        return Count == 0;
    }
    
    /// <summary>
    /// Gets the property list to display in the editor
    /// </summary>
    /// <returns>An array of properties to display and how</returns>
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