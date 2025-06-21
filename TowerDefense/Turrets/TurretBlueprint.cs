using System;
using BestagonDefense.Abstract;
using BestagonDefense.Abstract.Data;
using BestagonDefense.UI.Glyphs;
using Godot;
using Godot.Collections;

namespace BestagonDefense.Turrets;

/// <summary>
/// Allows us to save turret data in an so without creating a prefab per turret
/// </summary>
[GlobalClass]
[Tool]
public partial class TurretBlueprint : Resource, ISubtypable
{
    /// <summary>
    /// The icon that appears on the selection card
    /// </summary>
    [ExportGroup("Shop Info")]
    [Export]
    public Texture2D ShopIcon;
    /// <summary>
    /// The turret name that appears on the selection card
    /// </summary>
    [Export]
    public string DisplayName;
    /// <summary>
    /// The tagline of the turret. It's not a description, just a witty little remark
    /// </summary>
    [Export]
    public string Tagline;
    /// <summary>
    /// The glyph for the turret
    /// </summary>
    public TurretGlyph Glyph;
        
    /// <summary>
    /// The main colour of the turret.
    /// </summary>
    [Export]
    public Color Accent;
        
    /// <summary>
    /// The prefab to use when the turret is built
    /// </summary>
    [ExportGroup("Turret Info")]
    [Export]
    public PackedScene Prefab;
    /// <summary>
    /// Any modules that come pre-applied when the turret is placed
    /// </summary>
    [Export]
    public Array<ModuleChainHandler> ModuleHandlers = [];
    
    /// <summary>
    /// The prefab to use when the turret is built
    /// </summary>
    [ExportGroup("Building")]
    [Export]
    public PackedScene BuildEffect;

#nullable enable
    /// <summary>
    /// Gets the Type of the turret the blueprint is of
    /// </summary>
    /// <returns>The Type of the turret</returns>
    public Type GetSubtype()
    {
        SceneState state = Prefab.GetState();
        int scriptIdx = -1;
        for (var i = 0; i < state.GetNodePropertyCount(0); i++)
        {
            if (state.GetNodePropertyName(0, i) == "script")
            {
                scriptIdx = i;
                break;
            }
        }
        
        var script = (Script?)state.GetNodePropertyValue(0, scriptIdx);
        if (script is not null && script.GetClass() == "CSharpScript")
        {
            // TODO - Better to map with full ResourcePath?
            return TypeSpriteLookup.GetTypeFromString(script.ResourcePath.GetFile().Replace("." + script.ResourcePath.GetExtension(), ""));
        }
        return TypeSpriteLookup.GetTypeFromString(null);
    }
}