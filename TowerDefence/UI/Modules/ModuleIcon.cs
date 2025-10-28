using BestagonDefence.Abstract.Data;
using BestagonDefence.Modules;
using Godot;

namespace BestagonDefence.UI.Modules;

/// <summary>
/// A UI element for a Icon for a Module
/// </summary>
public partial class ModuleIcon : Control
{
    /// <summary>
    /// The Sprite2D to set the icon of
    /// </summary>
    [Export]
    private TextureRect icon;
    /// <summary>
    /// The level text to set as the module's level
    /// </summary>
    [Export]
    private Label text;

    private Module _module;
        
    /// <summary>
    /// Sets the sprite for the module icon
    /// </summary>
    /// <param name="sprite"></param>
    private void SetSprite(Texture2D sprite)
    {
        icon.Texture = sprite;
    }

    /// <summary>
    /// Sets all values for the module icon
    /// </summary>
    /// <param name="handler">The module chain handler to display the icon for</param>
    public void SetData(ModuleChainHandler handler)
    {
        _module = handler.GetModule();
        SetSprite(handler.GetChain().Icon);
        text.Text = handler.GetTierDisplay();
    }
        
    /// <summary>
    /// Gets the module the icon is for
    /// </summary>
    /// <returns>The module the icon represents</returns>
    public Module GetModule()
    {
        return _module;
    }
}