

using Godot;

namespace UI.Glyphs
{
    /// <summary>
    /// Stores the settings for a glyph
    /// </summary>
    public partial class TurretGlyphSo : Resource
    {
        /// <summary>
        /// The sprite of the glyph
        /// </summary>
        [Export]
        public Texture2D glyph;
        /// <summary>
        /// THe main colour of the sprite
        /// </summary>
        [Export]
        public Color body = new(255, 255, 255, 100);
        /// <summary>
        /// The shade to apply to the colour to obtain a secondary colour
        /// </summary>
        [Export]
        public Color shade = new(0, 0, 0, 0.25f);
    }
}
