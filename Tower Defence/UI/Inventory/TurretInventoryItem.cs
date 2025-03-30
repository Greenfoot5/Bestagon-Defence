using Abstract.Data;
using Gameplay;
using Godot;
using Turrets;
using UI.Modules;
using UI.TurretStats;

namespace UI.Inventory
{
    public partial class TurretInventoryItem : Control
    {
        private TurretBlueprint _turretBlueprint;
        
        /// <summary>
        /// The TMP text to display the turret's display name
        /// </summary>
        [Export]
        private RichTextLabel displayName;
        
        /// <summary>
        /// The Sprite2D to place the turret's icon
        /// </summary>
        [Export]
        private Sprite2D icon;
        /// <summary>
        /// The Sprite2D to place the turret's glyph
        /// </summary>
        [Export]
        private Sprite2D glyph;
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
        private RichTextLabel noneText;
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
        
        [ExportGroup("Colors")]
        // <summary>
        // The Hexagons shader background of the card
        // </summary>
        // [Export]
        // TODO - GlowBox
        // private GlowBox bg;
        
        /// <summary>
        /// The background Image of the modules section
        /// </summary>
        [Export]
        private Image modulesBg;

        /// <summary>
        /// Creates and setups the Selection UI.
        /// </summary>
        /// <param name="turret">The turret the option selects</param>
        public void Init(TurretBlueprint turret)
        {
            _turretBlueprint = turret;
            
            // Turret text
            displayName.Text = turret.displayName;
            
            // Icon and Glyph
            icon.Texture = turret.shopIcon;
            glyph.Texture = turret.glyph.glyph;
            // glyphBody.color = turret.glyph.body;
            
            // Turret stats
            // var turretPrefab = turret.prefab;
            // damage.SetData(turretPrefab.damage);
            // rate.SetData(turretPrefab.fireRate);
            // range.SetData(turretPrefab.range);
            
            // Colors
            // bg.color = turret.accent;
            // modulesBg.color = turret.accent * new Color(1, 1, 1, .16f);

            damage.SetColor(turret.accent);
            rate.SetColor(turret.accent);
            range.SetColor(turret.accent);
            
            // Turret's Modules
            if (turret.moduleHandlers.Count != 0)
            {
                noneText.Visible = false;
                foreach (ModuleChainHandler handler in turret.moduleHandlers) {
                    var mod = moduleUI.Instantiate<ModuleIcon>();
                    modulesLayout.AddChild(mod);
                    mod.Name = "_" + mod.Name;
                    mod.SetData(handler);
                }
            }
        }

        /// <summary>
        /// Called when a player clicks the card,
        /// selecting it and closing the shop
        /// </summary>
        public void Select()
        {
            BuildManager.instance.SelectTurretToBuild(_turretBlueprint, this);
        }
    }
}
