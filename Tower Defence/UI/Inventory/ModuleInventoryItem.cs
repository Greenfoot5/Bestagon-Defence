using System;
using Abstract;
using Abstract.Data;
using Godot;
using Turrets;
using UI.Glyphs;
using UI.Modules;

namespace UI.Inventory
{
    public partial class ModuleInventoryItem : Button
    {
        private ModuleChainHandler _module;
        
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
        
        [ExportGroup("Colors")]
        // <summary>
        // The Hexagons shader background of the card")]
        // [Export]
        // TODO - GlowBox
        // public GlowBox bg;
        /// <summary>
        /// The background Image of the modules section
        /// </summary>
        [Export]
        public Sprite2D modulesBg;
        
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
        // TODO - Export
        // [Export]
        public Type[] TurretTypes;
        /// <summary>
        /// The original accent colour
        /// </summary>
        public Color accent;

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
            effectText.Text = module.GetChain().description;
            
            // Icon
            icon.SetData(module);

            // Colors
            // bg.color = module.GetChain().accentColor;
            accent = module.GetChain().accentColor;
            modulesBg.SelfModulate = module.GetChain().accentColor * new Color(1, 1, 1, .16f);
            
            foreach (Type turretType in module.GetModule().GetValidTypes())
            {
                TurretGlyphSo glyphSo = lookup.GetForType(turretType);
                Node glyph = glyphPrefab.Instantiate();
                applicableGlyphs.AddChild(glyph);
                glyph.Name = "_" + glyph.Name;
                // glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                // glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                glyph.GetNode<Sprite2D>("Glyph").Texture = glyphSo.glyph;
            }

            TurretTypes = module.GetModule().GetValidTypes();
        }

        public bool IsValid(Damager damager)
        {
            return _module.ValidModule(damager);
        }
    }
}
