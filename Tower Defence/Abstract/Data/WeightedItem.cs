using System;
using Godot;

namespace Abstract.Data;

/// <summary>
/// An item with a float weight
/// </summary>
/// <typeparam name="T">The type of item to store</typeparam>
[Serializable]
[Tool]
public partial class WeightedItem<[MustBeVariant] T> : Resource where T : Resource, ISubtypeable
{
    public T Item;
    public float Weight;
    
    [ExportToolButton("Refresh Item")]
    public Callable RefreshButton => Callable.From(RefreshData);

    public void RefreshData()
    {
        ResourceLoader.Load<WeightedItem<T>>(ResourcePath, cacheMode: ResourceLoader.CacheMode.ReplaceDeep);
    }

    public WeightedItem(T item, float weight)
    {
        Item = item;
        Weight = weight;
    }

    public WeightedItem()
    {
        Weight = 0;
    }

    public override Godot.Collections.Array<Godot.Collections.Dictionary> _GetPropertyList()
    {
        Godot.Collections.Array<Godot.Collections.Dictionary> properties =
        [
            new()
            {
                { "name", "Item" },
                { "type", (int)Variant.Type.Object },
                { "hint", (int)PropertyHint.ResourceType },
                { "hint_string", typeof(T).Name },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
            },

            new()
            {
                { "name", "Weight" },
                { "type", (int)Variant.Type.Float },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "0,100,,or_greater" },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
            }

        ];

        return properties;
    }
        
    public override Variant _Get(StringName property)
    {
        return property.ToString() switch
        {
            "Item" => Variant.From(Item),
            "Weight" => Weight,
            _ => default
        };
    }
    
    public override bool _Set(StringName property, Variant value)
    {
        switch (property.ToString())
        {
            case "Item":
                Item = value.As<T>();
                NotifyPropertyListChanged();
                return true;
            case "Weight":
                Weight = value.As<float>();
                NotifyPropertyListChanged();
                return true;
            default:
                return false;
        }
    }
}