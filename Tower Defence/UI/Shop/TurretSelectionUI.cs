using Abstract.Data;
using Gameplay;
using Godot;
using Turrets;
using UI.Inventory;
using UI.TurretStats;

namespace UI.Shop
{
    /// <summary>
    /// Displays the data for a turret shop card
    /// </summary>
    public partial class TurretSelectionUI : Control
    {
        private TurretBlueprint _turretBlueprint;
    
        /// <summary>
        /// The Text to display the turret's name
        /// </summary>
        // Content
        [Export]
        private Label displayName;
        /// <summary>
        /// The RTL to display the turret's tagline
        /// </summary>
        [Export]
        private Label tagline;
        
        /// <summary>
        /// The Sprite2D to place the turret's icon
        /// </summary>
        [Export]
        private Sprite2D icon;
        /// <summary>
        /// The glyph Sprite2D
        /// </summary>
        [Export]
        private Sprite2D glyph;
    
        /// <summary>
        /// The selection of modules to enable if the turret has any
        /// </summary>
        [ExportGroup("Modules")]
        [Export]
        private Control modulesSection;
        /// <summary>
        /// The parent of any module icons to display
        /// </summary>
        [Export]
        private Container modulesLayout;
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
        /// The TurretStat to display range
        /// </summary>
        [Export]
        private TurretStat range;

        /// <summary>
        /// The Hexagons shader background of the card
        // </summary>
        [ExportGroup("Colors")]
        // TODO - Hexagons shader
        // [Export]
        // private Hexagons bg;
        /// <summary>
        /// The background Sprite2D of the module's selection
        // </summary>
        [Export]
        private Sprite2D modulesBg;
        /// <summary>
        /// The title of the module section
        /// </summary>
        [Export]
        private Label modulesTitle;

        /// <summary>
        /// Creates and setups the Selection UI.
        /// </summary>
        /// <param name="turret">The turret the option selects</param>
        /// <param name="shop">The Shop (allows the game to select the turret when the player clicks the panel)</param>
        public void Init(TurretBlueprint turret, Shop shop)
        {
            _turretBlueprint = turret;
            
            // Turret text
            displayName.Text = turret.displayName;
            tagline.Text = turret.tagline;
            
            // Icon and Glyph
            icon.Texture = turret.shopIcon;
            glyph.Texture = turret.glyph.glyph;
            glyph.SelfModulate = turret.glyph.body;
            
            // Turret stats
            // TODO - Get stats
            var turretPrefab = turret.prefab; //.GetComponent<Turret>();
            // damage.SetData(turretPrefab.damage);
            // rate.SetData(turretPrefab.fireRate);
            // range.SetData(turretPrefab.range);
            
            // Turret's Modules
            if (turret.moduleHandlers.Count == 0)
            {
                modulesSection.Visible = false;
            }
            else
            {
                foreach (ModuleChainHandler handler in turret.moduleHandlers) {
                    var mod = moduleUI.Instantiate<TurretModulesIcon>();
                    // TODO - Set Parent
                    mod.GlobalPosition = modulesSection.GlobalPosition;
                    mod.Name = "_" + mod.Name;
                    mod.SetData(handler);
                }
            }

            // Colors
            tagline.SelfModulate = turret.accent;
            modulesTitle.SelfModulate = turret.accent;
            // bg.color = turret.accent;
            modulesBg.SelfModulate = turret.accent * new Color(1, 1, 1, .16f);

            damage.SetColor(turret.accent);
            rate.SetColor(turret.accent);
            range.SetColor(turret.accent);
            
            // Adds the click event to the card
            // bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        /// <summary>
        /// Called when a player clicks the card,
        /// selecting it and closing the shop
        /// </summary>
        /// <param name="shop"></param>
        private void MakeSelection(Shop shop)
        {
            shop.SpawnNewTurret(_turretBlueprint);
            TurretInfo.instance.DisplayTurretInventory();
            GameStats.Powercells -= 1;
        }
    }
}
