using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Abstract.Data;

/// <summary>
/// A list of items and their weight.
/// Can get a random item and total weight of the values
/// </summary>
/// <typeparam name="T">The type of the list</typeparam>
[Serializable]
[Tool]
public partial class WeightedCurveList<[MustBeVariant] T> : Resource where T : Resource, ISubtypeable
{
    public Array<WeightedCurve<T>> List;
    
    private int _size;
    
    public int Size
    {
        get => _size;
        set
        {
            _size = value;
            List.Resize(_size);
            NotifyPropertyListChanged();
        }
    }
    
    [ExportToolButton("Refresh Item(s)")]
    public Callable RefreshButton => Callable.From(RefreshData);

    public void RefreshData()
    {
        ResourceLoader.Load<WeightedCurveList<T>>(ResourcePath, cacheMode: ResourceLoader.CacheMode.ReplaceDeep);
    }
    
    /// <summary>
    /// Basic constructor for the list
    /// </summary>
    /// <param name="list">The list to create</param>
    public WeightedCurveList(Array<WeightedCurve<T>> list)
    {
        List = list;
    }

    public WeightedCurveList()
    {
        List = [];
    }
        
    /// <summary>
    /// Converts the WeightedCurveList to a WeightedList at a certain time
    /// </summary>
    /// <param name="time">The time to get the weight from the AnimationCurves</param>
    /// <returns>The WeightedList for a specific time</returns>
    public WeightedList<T> ToWeightedList(float time)
    {
        var weightedList = new WeightedList<T>([new WeightedItem<T>(List[0].Item, List[0].Value.Sample(time))]);
        weightedList.RemoveAt(0);

        var i = 0;
        foreach (WeightedCurve<T> item in List.Where(item => item.Value.Sample(time) > 0))
        {
            weightedList[i] = new WeightedItem<T>(item.Item, item.Value.Sample(time));
            i++;
        }

        return weightedList;
    }
    
    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties =
        [
            new()
            {
                { "name", $"Size" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.None },
                { "usage", (int)PropertyUsageFlags.Array + (int)PropertyUsageFlags.Default },
                { "class_name", "Items,list_" }
            }
        ];
    
        for (var i = 0; i < _size; i++)
        {
            properties.Add(new Dictionary()
            {
                { "name", $"list_{i}/Item" },
                { "type", (int)Variant.Type.Object },
                { "hint", (int)PropertyHint.ResourceType },
                { "hint_string", typeof(T).Name },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage }
            });
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
    
    public override Variant _Get(StringName property)
    {
        var propertyName = property.ToString();
        if (propertyName.StartsWith("list_"))
        {
            string[] split = propertyName.Split('/');
            int index = int.Parse(split[0]["list_".Length..]);
            List[index] ??= new WeightedCurve<T>();
            switch (split[1])
            {
                case "Curve":
                    return List[index].Curve;
                case "Item":
                    return List[index].Item;
                default:
                    GD.PrintErr("Invalid property name in WeightedList for WeightedItem: " + split[1]);
                    break;
            }
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
            List[index] ??= new WeightedCurve<T>();
            switch (split[1])
            {
                case "Curve":
                    List[index].Curve = value.As<Curve>();
                    NotifyPropertyListChanged();
                    return true;
                case "Item":
                    List[index].Item = value.As<T>();
                    NotifyPropertyListChanged();
                    return true;
                default:
                    GD.PrintErr("Invalid property name in WeightedList for WeightedItem: " + split[1]);
                    break;
            }
        }
        return false;
    }
}