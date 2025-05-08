using System;
using Abstract.Data;
using Gameplay;
using Godot;
using Modules;
using UI.Glyphs;
using UI.Inventory;
using UI.Modules;

namespace UI.Shop
{
    /// <summary>
    /// Displays a shop card for a Module
    /// </summary>
    public partial class ModuleSelectionUI : Control
    {
        /// <summary>
        /// The module to display on the card
        /// </summary>
        [Export]
        private ModuleChainHandler handler;
        
        /// <summary>
        /// The hexagons background of the card
        // </summary>
        // TODO - Hexagons
        // [Export]
        // private Hexagons bg;
        
        /// <summary>
        /// The label to display name of the module
        /// </summary>
        [Export]
        private Label displayName;
        /// <summary>
        /// The label for the tagline
        /// </summary>
        [Export]
        private Label tagline;

        /// <summary>
        /// The ModuleIcon of the module
        /// </summary>
        [Export]
        private ModuleIcon icon;
        
        /// <summary>
        /// The label to contain the module description
        /// </summary>
        [Export]
        private Label effect;
        
        /// <summary>
        /// The generic glyph PackedScene to use to display applicable turrets
        /// </summary>
        [Export]
        private PackedScene glyphPrefab;
        /// <summary>
        /// The parent for the module's turret glyphs
        /// </summary>
        [Export]
        private Node applicableGlyphs;

        /// <summary>
        /// Creates the UI
        /// </summary>
        /// <param name="initHandler">The ModuleChainHandler the card is for</param>
        /// <param name="shop">The shop script</param>
        public void Init (ModuleChainHandler initHandler, Shop shop)
        {
            handler = initHandler;

            ModuleChain chain = initHandler.GetChain();
            Module module = initHandler.GetModule();

            // bg.color = chain.accentColor;

            displayName.Text = initHandler.GetDisplayName();
            tagline.Text = chain.Tagline;
            tagline.SelfModulate = chain.AccentColor;

            icon.SetData(initHandler);
        
            effect.Text = chain.Description;
            effect.SelfModulate = chain.AccentColor;
            
            foreach (Type turretType in module.GetValidTypes())
            {
                TurretGlyphSo glyphSo = shop.GlyphsLookup.GetForType(turretType);
                Node glyph = glyphPrefab.Instantiate();
                applicableGlyphs.AddChild(glyph);
                glyph.Name = "_" + glyph.Name;
                // glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                // glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                glyph.GetNode<Sprite2D>("Glyph").Texture = glyphSo.glyph;
            }
            
            // When the card is clicked, the game picks the module
            // bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        /// <summary>
        /// Called when the player clicks on the card.
        /// </summary>
        /// <param name="shop"></param>
        private void MakeSelection(Shop shop)
        {
            shop.SpawnNewModule(handler);
            GameStats.Powercells -= 1;
        }
    }
}
