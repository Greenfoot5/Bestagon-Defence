using Godot;

namespace UI.Glyphs;

/// <summary>
/// Stores the settings for a glyph
/// </summary>
[GlobalClass]
[Tool]
public partial class TurretGlyph : Resource
{
    /// <summary>
    /// The sprite of the glyph
    /// </summary>
    [Export]
    public Texture2D Glyph;
    /// <summary>
    /// THe main colour of the sprite
    /// </summary>
    [Export]
    public Color Body = new(0.6f, 0.6f, 0.6f, 100);
    /// <summary>
    /// The shade to apply to the colour to obtain a secondary colour
    /// </summary>
    [Export]
    public Color Shade = new(0, 0, 0, 0.25f);
}