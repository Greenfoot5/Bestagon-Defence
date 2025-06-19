using Godot;

namespace BestagonDefense.UI.Glyphs;

/// <summary>
/// Node of a Glyph
/// </summary>
public partial class GlyphUI : TextureRect
{
    /// <summary>
    /// The sprite of the glyph
    /// </summary>
    [Export]
    public TextureRect Glyph;
    /// <summary>
    /// THe main colour of the sprite
    /// </summary>
    [Export]
    public TextureRect Body;
    /// <summary>
    /// The shade to apply to the colour to obtain a secondary colour
    /// </summary>
    [Export]
    public TextureRect Shade;

    /// <summary>
    /// Populates the UI values from a TurretGlyph resource
    /// </summary>
    /// <param name="res">The TurretGlyph data to use</param>
    public void FromResource(TurretGlyph res)
    {
        Glyph.Texture = res.Glyph;
        Body.SelfModulate = res.Body;
        Shade.SelfModulate = res.Shade;
    }
}