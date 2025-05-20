using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Hunter;
using Turrets.Lancer;
using Turrets.Laser;
using Turrets.Shooter;
using Turrets.Smasher;
using UI.Glyphs;

namespace Abstract;

/// <summary>
/// A class to reference between turret types and turret glyphs
/// </summary>
[Serializable]
[GlobalClass]
[Tool]
public partial class TypeSpriteLookup : Resource
{
    private static readonly List<Type> Types =
    [
        typeof(Turret), // Represents no specific turret type
        typeof(Choker),
        typeof(Gunner),
        typeof(Hunter),
        typeof(Lancer),
        typeof(Laser),
        typeof(Shooter),
        typeof(Smasher),
    ];
    
    private static readonly List<StringName> Names =
    [
        nameof(Turret), // Represents no specific turret type
        nameof(Choker),
        nameof(Gunner),
        nameof(Hunter),
        nameof(Lancer),
        nameof(Laser),
        nameof(Shooter),
        nameof(Smasher),
    ];

    /// <summary>
    /// The list of TurretGlyphs to use
    /// </summary>
    private Array<TurretGlyph> _sprites = [];

    public TypeSpriteLookup()
    {
        _sprites.Resize(Types.Count);
    }
        
    /// <summary>
    /// Get all the types the game currently has stored
    /// </summary>
    /// <returns>All the types current stored</returns>
    public static List<Type> GetAllTypes()
    {
        return Types;
    }
    
    /// <summary>
    /// Gets the glyph for a specific turret type
    /// </summary>
    /// <param name="t">The turret type to get the glyph for</param>
    /// <returns>The turret's glyph</returns>
    public TurretGlyph GetForType(Type t)
    {
        try
        {
            return _sprites[Types.IndexOf(t)]; 
        }
        catch (Exception ex)
        {
            if (ex is not IndexOutOfRangeException && ex is not ArgumentOutOfRangeException) throw;
            
            GD.PushError("Cant find sprite of type " + t);
            return _sprites[0];

        }
    }
    
    /// <summary>
    /// Gets the Type for a given string (must match type name exactly)
    /// </summary>
    /// <param name="t">The string to match for</param>
    /// <returns>The string's Type</returns>
    public static Type GetTypeFromString(string t)
    {
        try
        {
            return Types[Names.IndexOf(t)]; 
        }
        catch (Exception ex)
        {
            if (ex is not IndexOutOfRangeException && ex is not ArgumentOutOfRangeException) throw;
            
            GD.PushError("Cant find sprite of type " + t);
            return Types[0];

        }
    }
    
    public override Array<Dictionary> _GetPropertyList()
    {
        Array<Dictionary> properties = [];

        foreach (Type t in Types)
        {
            properties.Add(new Dictionary()
            {
                { "name", t.Name },
                { "type", (int)Variant.Type.Object },
                { "hint", (int)PropertyHint.ResourceType },
                { "hint_string", nameof(TurretGlyph) },
                { "usage", (int)PropertyUsageFlags.Editor + (int)PropertyUsageFlags.Storage + (int)PropertyUsageFlags.StoreIfNull }
            });
        }
    
        return properties;
    }
    
    public override Variant _Get(StringName property)
    {
        var propertyName = property.ToString();
        for (var i = 0; i < Types.Count; i++)
        {
            if (Types[i].Name == propertyName)
            {
                return _sprites[i];
            }
        }
        return default;
    }

    public override bool _Set(StringName property, Variant value)
    {
        var propertyName = property.ToString();
        for (var i = 0; i < Types.Count; i++)
        {
            if (Types[i].Name == propertyName)
            {
                _sprites[i] = value.As<TurretGlyph>();
                NotifyPropertyListChanged();
                return true;
            }
        }
        
        return false;
    }
}