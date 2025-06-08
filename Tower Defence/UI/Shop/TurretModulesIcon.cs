using Abstract.Data;
using Godot;
using UI.Modules;

namespace UI.Shop;

/// <summary>
/// Sets the module icon for each module in a turret's upgrades on selection
/// </summary>
public partial class TurretModulesIcon : Control
{
    /// <summary>
    /// The ModuleIcon of the module
    /// </summary>
    [Export]
    private ModuleIcon icon;
    /// <summary>
    /// The label displaying the display name
    /// </summary>
    [Export]
    private Label text;
        
    /// <summary>
    /// Sets the data
    /// </summary>
    /// <param name="handler">The module chain handler the icon is for</param>
    public void SetData(ModuleChainHandler handler)
    {
        icon.SetData(handler);
        text.Text = handler.GetDisplayName();
    }
}