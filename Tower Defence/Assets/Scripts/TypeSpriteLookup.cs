using System;
using System.Collections.Generic;
using Turrets;
using UI;
using UnityEngine;

[Serializable]
public class TypeSpriteLookup
{
    private static readonly List<Type> Types = new List<Type>
    {
        null,
        typeof(Shooter),
        typeof(Laser),
        typeof(Smasher)
    };

    [SerializeField] private List<TurretGlyphSo> sprites;

    public static List<Type> GetAllTypes()
    {
        return Types;
    }

    public TurretGlyphSo GetForType(Type t)
    {
        try
        {
            return sprites[Types.IndexOf(t)]; 
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("Can't find sprite of type " + t);
            return sprites[0];
        }
    } 
}