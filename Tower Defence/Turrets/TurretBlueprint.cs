using System;
using System.Collections.Generic;
using Abstract;
using Abstract.Data;
using Godot;
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
    public Texture2D shopIcon;
    /// <summary>
    /// The turret name that appears on the selection card
    /// </summary>
    [Export]
    public string displayName;
    /// <summary>
    /// The tagline of the turret. It's not a description, just a witty little remark
    /// </summary>
    [Export]
    public string tagline;
    /// <summary>
    /// The glyph for the turret
    /// </summary>
    public TurretGlyphSo glyph;
        
    /// <summary>
    /// The main colour of the turret.
    /// </summary>
    [Export]
    public Color accent;
        
    /// <summary>
    /// The prefab to use when the turret is built
    /// </summary>
    [ExportGroup("Turret Info")]
    [Export]
    public PackedScene prefab;
    /// <summary>
    /// Any modules that come pre-applied when the turret is placed
    /// </summary>
    public List<ModuleChainHandler> moduleHandlers = new();

    public Type GetSubtype()
    {
        // TODO - Confirm this actually returns turret type, not just Node2D
        return prefab.GetType();
    }
}