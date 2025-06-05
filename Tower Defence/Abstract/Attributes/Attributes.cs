using System.Collections.Generic;
using Godot;

namespace Abstract.Attributes;

[GlobalClass]
public partial class Attributes : Resource
{
    public static readonly Attributes Global = new();

    [Export]
    private Godot.Collections.Dictionary<AttributeType, Attribute> _attributes;
    private Godot.Collections.Dictionary<AttributeType, Attribute> _defaults = new()
    {
        [AttributeType.KnockbackDuration] = new Attribute(AttributeType.KnockbackDuration, 0.2f)
    };
    
    public ICollection<AttributeType> Keys => _attributes.Keys;

    public Attributes()
    {
        _attributes = new Godot.Collections.Dictionary<AttributeType, Attribute>();
    }
    
    public Attributes(Godot.Collections.Dictionary<AttributeType, Attribute> attributes)
    {
        _attributes = attributes;
    }

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