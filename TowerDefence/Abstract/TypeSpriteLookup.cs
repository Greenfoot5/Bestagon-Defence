using System;
using System.Collections.Generic;
using BestagonDefence.Turrets;
using BestagonDefence.Turrets.Choker;
using BestagonDefence.Turrets.Gunner;
using BestagonDefence.Turrets.Hunter;
using BestagonDefence.Turrets.Lancer;
using BestagonDefence.Turrets.Laser;
using BestagonDefence.Turrets.Seeker;
using BestagonDefence.Turrets.Smasher;
using BestagonDefence.UI.Glyphs;
using Godot;
using Godot.Collections;

namespace BestagonDefence.Abstract;

/// <summary>
/// A class to reference between turret types and turret glyphs
/// </summary>
[Serializable]
[GlobalClass]
[Tool]
public partial class TypeSpriteLookup : Resource
{
    /// <summary>
    /// A list of Types for all the turrets
    /// </summary>
    private static readonly List<Type> Types =
    [
        typeof(Turret), // Represents no specific turret type
        typeof(Choker),
        typeof(Gunner),
        typeof(Hunter),
        typeof(Lancer),
        typeof(Laser),
        typeof(Seeker),
        typeof(Shooter),
        typeof(Smasher),
    ];
    
    /// <summary>
    /// A list of StringNames of all the turrets
    /// </summary>
    private static readonly List<StringName> Names =
    [
        nameof(Turret), // Represents no specific turret type
        nameof(Choker),
        nameof(Gunner),
        nameof(Hunter),
        nameof(Lancer),
        nameof(Laser),
        nameof(Seeker),
        nameof(Shooter),
        nameof(Smasher),
    ];

    /// <summary>
    /// The list of TurretGlyphs to use
    /// </summary>
    private Array<TurretGlyph> _sprites = [];

    /// <summary>
    /// Creates a new lookup
    /// </summary>
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
    
    /// <summary>
    /// Creates a field for each sprite to be set in the editor
    /// </summary>
    /// <returns>An array of properties to display and how</returns>
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
    
    /// <summary>
    /// Gets the texture for a given type for the editor
    /// </summary>
    /// <param name="property">The property to get the data for</param>
    /// <returns>The data for that property (if it exists)</returns>
    public override Variant _Get(StringName property)
    {
        var propertyName = property.ToString();
        for (var i = 0; i < Types.Count; i++)
        {
            if (Types[i] == null)
                return default;
            if (Types[i].Name == propertyName)
            {
                return _sprites[i];
            }
        }
        return default;
    }

    /// <summary>
    /// Sets the texture for a given type for the editor
    /// </summary>
    /// <param name="property">The property to update</param>
    /// <param name="value">The new value of the property</param>
    /// <returns>true if the value was updated</returns>
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