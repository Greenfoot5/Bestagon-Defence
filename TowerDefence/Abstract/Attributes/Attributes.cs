using System.Collections.Generic;
using Godot;

namespace BestagonDefence.Abstract.Attributes;

/// <summary>
/// A collection of attributes
/// </summary>
[GlobalClass]
public partial class Attributes : Resource
{
    /// <summary>
    /// Can be referenced to see global modifiers for any attribute
    /// </summary>
    public static readonly Attributes Global = new();

    /// <summary>
    /// The current attributes in the collection
    /// </summary>
    [Export]
    private Godot.Collections.Dictionary<AttributeType, Attribute> _attributes;
    
    /// <summary>
    /// The default values in the collection
    /// </summary>
    private Godot.Collections.Dictionary<AttributeType, Attribute> _defaults = new()
    {
        [AttributeType.KnockbackDuration] = new Attribute(AttributeType.KnockbackDuration, 0.2f)
    };
    
    // TODO - Utilise to add warnings when certain keys aren't present in an attributes set
    public ICollection<AttributeType> Keys => _attributes.Keys;

    /// <summary>
    /// Creates a new empty collection
    /// </summary>
    public Attributes()
    {
        _attributes = new Godot.Collections.Dictionary<AttributeType, Attribute>();
    }
    
    /// <summary>
    /// Clones an existing dictionary of attributes
    /// </summary>
    /// <param name="attributes">The dictionary of AttributeType, Attribute, to clone</param>
    public Attributes(Godot.Collections.Dictionary<AttributeType, Attribute> attributes)
    {
        _attributes = attributes;
    }
    
    /// <summary>
    /// Clones an existing Attributes
    /// </summary>
    /// <param name="attributes">The Attributes collection to clone</param>
    public Attributes(Attributes attributes)
    {
        _attributes = new Godot.Collections.Dictionary<AttributeType, Attribute>(attributes._attributes);
    }

    /// <summary>
    /// Gets an attribute for the given key
    /// </summary>
    /// <param name="key">They AttributeType of the Attribute to fetch</param>
    public Attribute this[AttributeType key]
    {
        get
        {
            if (_attributes.TryGetValue(key, out Attribute item))
                return item;

            if (_defaults.TryGetValue(key, out Attribute @default))
            {
                _attributes.Add(key, new Attribute(@default));
                return _attributes[key];
            }

            return new Attribute();
        }

        set
        {
            if (_attributes.TryGetValue(key, out Attribute _))
            {
                _attributes[key].CopyFrom(value);
                return;
            }

            _attributes.Add(key, new Attribute(value));
            _attributes[key].Name = key;
        }
    }
}