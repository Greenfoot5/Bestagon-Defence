using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
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
        typeof(Lancer),
        typeof(Laser),
        typeof(Shooter),
        typeof(Smasher),
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