using BestagonDefence.Abstract.Attributes;
using BestagonDefence.Abstract.Data;
using BestagonDefence.Gameplay;
using BestagonDefence.Turrets;
using BestagonDefence.UI.Glyphs;
using BestagonDefence.UI.Modules;
using BestagonDefence.UI.TurretStats;
using Godot;

namespace BestagonDefence.UI.Inventory;

/// <summary>
/// A turret blueprint UI in the turret inventory
/// </summary>
public partial class TurretInventoryItem : Control
{
    /// <summary>
    /// The blueprint the UI displays for
    /// </summary>
    public TurretBlueprint TurretBlueprint { get; private set; }
    
    /// <summary>
    /// The TMP text to display the turret's display name
    /// </summary>
    [Export]
    private Label displayName;
    
    /// <summary>
    /// The Sprite2D to place the turret's icon
    /// </summary>
    [Export]
    private TextureRect icon;
    /// <summary>
    /// The Sprite2D to place the turret's glyph
    /// </summary>
    [Export]
    private GlyphUI glyph;
    // <summary>
    // The body colour of the turret's glyph
    // </summary>
    // [Export]
    // TODO - HexagonSprite
    // private HexagonSprite glyphBody;
    
    /// <summary>
    /// The none text of modules to disable if the turret has any modules
    /// </summary>
    [ExportGroup("Modules")]
    [Export]
    private Label noneText;
    /// <summary>
    /// The parent of any module icons to display
    /// </summary>
    [Export]
    private Control modulesLayout;
    /// <summary>
    /// The prefab of a generic module icon to instantiate under the modulesLayout
    /// </summary>
    [Export]
    private PackedScene moduleUI;
    
    /// <summary>
    /// The TurretStat used to display the damage
    /// </summary>
    [ExportGroup("Stats")]
    [Export]
    private TurretStat damage;
    /// <summary>
    /// The TurretStat used to display the fire rate
    /// </summary>
    [Export]
    private TurretStat rate;
    /// <summary>
    /// The TurretStat used to display the range
    /// </summary>
    [Export]
    private TurretStat range;
    
    /// <summary>
    /// The background Image of the modules section
    /// </summary>
    [ExportGroup("Colors")]
    [Export]
    private Panel modulesBg;
    // <summary>
    // The Hexagons shader background of the card
    // </summary>
    // [Export]
    // TODO - GlowBox
    // private GlowBox bg;

    /// <summary>
    /// Creates and setups the Selection UI.
    /// </summary>
    /// <param name="turret">The turret the option selects</param>
    public void Init(TurretBlueprint turret)
    {
        TurretBlueprint = turret;
            
        // Turret text
        displayName.Text = turret.DisplayName;
            
        // Icon and Glyph
        icon.Texture = turret.ShopIcon;
        glyph.FromResource(turret.Glyph);
            
        // Turret stats
        var turretPrefab = turret.Prefab.Instantiate<Turret>();
        damage.SetData(turretPrefab.Stats[AttributeType.Damage]);
        rate.SetData(turretPrefab.Stats[AttributeType.FireRate]);
        range.SetData(turretPrefab.Stats[AttributeType.Range]);
        turretPrefab.QueueFree();
            
        // Colors
        // bg.color = turret.accent;
        modulesBg.SelfModulate = turret.Accent * new Color(1, 1, 1, .16f);

        damage.SetColor(turret.Accent);
        rate.SetColor(turret.Accent);
        range.SetColor(turret.Accent);
            
        // Turret's Modules
        if (turret.ModuleHandlers.Count == 0) return;
        
        noneText.Visible = false;
        foreach (ModuleChainHandler handler in turret.ModuleHandlers) {
            var mod = moduleUI.Instantiate<ModuleIcon>();
            modulesLayout.AddChild(mod);
            mod.Name = "_" + mod.Name;
            mod.SetData(handler);
        }
    }

    /// <summary>
    /// Called when a player clicks the card,
    /// selecting it and closing the shop
    /// </summary>
    private void Select()
    {
        BuildManager.SelectBlueprint(this);
    }
}