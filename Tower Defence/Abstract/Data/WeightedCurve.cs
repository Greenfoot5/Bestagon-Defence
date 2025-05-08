using Godot;

namespace Abstract.Data;

/// <summary>
/// An item with a variable weight
/// </summary>
/// <typeparam name="T">The type of the item</typeparam>
[System.Serializable]
[Tool]
public partial class WeightedCurve<[MustBeVariant] T> : Resource where T : Resource, ISubtypeable
{
    public T Item;
    public Curve Curve;
    
    public Curve Value => Curve;
    
    [ExportToolButton("Refresh Item(s)")]
    public Callable RefreshButton => Callable.From(RefreshData);

    public void RefreshData()
    {
        ResourceLoader.Load<WeightedCurve<T>>(ResourcePath, cacheMode: ResourceLoader.CacheMode.ReplaceDeep);
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
                { "name", "Curve" },
                { "type", (int)Variant.Type.Object },
                { "hint", (int)PropertyHint.ResourceType },
                { "hint_string", "Curve" },
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
            "Curve" => Curve,
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
            case "Curve":
                Curve = value.As<Curve>();
                NotifyPropertyListChanged();
                return true;
            default:
                return false;
        }
    }
}