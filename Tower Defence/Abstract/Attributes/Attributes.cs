using Godot;
using Godot.Collections;

namespace Abstract.Attributes;

public partial class Attributes : RefCounted
{
    private Dictionary<AttributeType, Attribute> _attributes;
    private Dictionary<AttributeType, Attribute> _defaults = new()
    {
        [AttributeType.KnockbackDuration] = new Attribute(0.2f)
    };

    public Attributes()
    {
        _attributes = new Dictionary<AttributeType, Attribute>();
    }
    
    public Attributes(Dictionary<AttributeType, Attribute> attributes)
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
                return;
            _attributes.Add(key, new Attribute(value));
            _attributes[key].Name = key;
        }
    }
    
    public static Attributes global = new();
}