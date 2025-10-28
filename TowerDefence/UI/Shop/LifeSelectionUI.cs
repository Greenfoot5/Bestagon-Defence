using BestagonDefence.Gameplay;
using Godot;

namespace BestagonDefence.UI.Shop;

/// <summary>
/// Displays a shop card for a Life
/// </summary>
public partial class LifeSelectionUI : Control
{
    /// <summary>
    /// The amount of lives to grant
    /// </summary>
    private int _count;
        
    /// <summary>
    /// The button that "picks" this option
    /// </summary>
    [Export]
    private BaseButton button;
        
    /// <summary>
    /// The label contains the card description
    /// </summary>
    [Export]
    private Label effect;
        
    /// <summary>
    /// The generic glyph to use to display the applicable turrets
    /// </summary>
    [Export]
    private string lifeCount;

    /// <summary>
    /// Creates the UI
    /// </summary>
    /// <param name="count">The amount of lives to grant</param>
    /// <param name="shop">The shop script</param>
    public void Init (int count, Shop shop)
    {
        _count = count;

        effect.Text = lifeCount;
            
        // When the card is clicked, the game picks the module
        // bg.GetComponent<Button>().onClick.AddListener(MakeSelection);
        button.Pressed += () => { MakeSelection(shop); };
    }

    /// <summary>
    /// Called when the player clicks on the card.
    /// </summary>
    private void MakeSelection(Shop shop)
    {
        GameStats.Lives += _count;
        shop.SelectionGenerator.GenerateSelection();
        shop.SelectionGenerator.Resume();
        shop.SelectionGenerator.Unlock();
        GameStats.Powercells -= 1;
    }
}