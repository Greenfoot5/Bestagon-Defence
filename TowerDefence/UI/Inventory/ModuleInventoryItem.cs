using System;
using BestagonDefence.Abstract;
using BestagonDefence.Abstract.Data;
using BestagonDefence.Turrets;
using BestagonDefence.UI.Glyphs;
using BestagonDefence.UI.Modules;
using Godot;

namespace BestagonDefence.UI.Inventory;

/// <summary>
/// The UI for a module in the inventory
/// </summary>
public partial class ModuleInventoryItem : Button
{
    /// <summary>
    /// The TMP text to display the module's display name
    /// </summary>
    [Export]
    private Label displayName;
    /// <summary>
    /// The text to set for the module's effect
    /// </summary>
    [Export]
    private Label effectText;
    
    /// <summary>
    /// The ModuleIcon for the module card
    /// </summary>
    [Export]
    private ModuleIcon icon;
    
    /// <summary>
    /// The background Image of the modules section
    /// </summary>
    [ExportGroup("Colors")]
    [Export]
    public Button modulesBg;
    // <summary>
    // The Hexagons shader background of the card")]
    // [Export]
    // TODO - GlowBox
    // public GlowBox bg;
    
    /// <summary>
    /// The generic glyph prefab to use to display the applicable turrets
    /// </summary>
    [Export]
    private PackedScene glyphPrefab;
    /// <summary>
    /// The Transform to set as the parent for the module's turret glyphs
    /// </summary>
    [Export]
    private Node applicableGlyphs;
    
    /// <summary>
    /// The list of types the turret has
    /// </summary>
    public Type[] TurretTypes;
    /// <summary>
    /// The original accent colour
    /// </summary>
    public Color Accent;
    
    private ModuleChainHandler _module;

    /// <summary>
    /// Creates and setups the Selection UI.
    /// </summary>
    /// <param name="module">The module the option selects</param>
    /// <param name="lookup">The TypeSpriteLookup</param>
    public void Init (ModuleChainHandler module, TypeSpriteLookup lookup)
    {
        _module = module;
            
        // Module text
        displayName.Text = module.GetDisplayName();
        effectText.Text = module.GetChain().Description;
            
        // Icon
        icon.SetData(module);

        // Colors
        // bg.color = module.GetChain().accentColor;
        Accent = module.GetChain().AccentColor;
        modulesBg.SelfModulate = module.GetChain().AccentColor * new Color(1, 1, 1, .16f);
            
        foreach (Type turretType in module.GetModule().GetValidTypes())
        {
            TurretGlyph glyphRes = lookup.GetForType(turretType);
            Node glyph = glyphPrefab.Instantiate();
            applicableGlyphs.AddChild(glyph);
            glyph.Name = "_" + glyph.Name;
            // TODO - Glyph
            // glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
            // glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
            glyph.GetNode<TextureRect>("Glyph").Texture = glyphRes.Glyph;
        }

        TurretTypes = module.GetModule().GetValidTypes();
    }

    /// <summary>
    /// Checks if the damager is valid for the module
    /// </summary>
    /// <param name="damager">The damager to check for</param>
    /// <returns>true if the module can be applied to the damager</returns>
    public bool IsValid(Damager damager)
    {
        return _module.ValidModule(damager);
    }
}