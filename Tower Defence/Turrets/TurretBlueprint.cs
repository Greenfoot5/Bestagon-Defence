using System;
using Abstract;
using Abstract.Data;
using Godot;
using Godot.Collections;
using UI.Glyphs;

namespace Turrets;

/// <summary>
/// Allows us to save turret data in an so without creating a prefab per turret
/// </summary>
[GlobalClass]
[Tool]
public partial class TurretBlueprint : Resource, ISubtypeable
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

    public Type GetSubtype()
    {
        // TODO - Confirm this actually returns turret type, not just Node2D
        return Prefab.GetType();
    }
}