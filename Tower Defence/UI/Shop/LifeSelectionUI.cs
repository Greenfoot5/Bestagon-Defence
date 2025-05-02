using Gameplay;
using Godot;

namespace UI.Shop
{
    /// <summary>
    /// Displays a shop card for a Module
    /// </summary>
    public partial class LifeSelectionUI : Control
    {
        /// <summary>
        /// The amount of lives to grant
        /// </summary>
        [Export]
        private int _count;
        
        // TODO - Hexagons
        // [Export]
        /// <summary>
        /// The hexagons background of the card (the card's background shader)
        // </summary>
        // [Export]
        // private Hexagons bg;
        
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
        }

        /// <summary>
        /// Called when the player clicks on the card.
        /// </summary>
        private void MakeSelection ()
        {
            GameStats.Lives += _count;
            GameStats.Powercells -= 1;
        }
    }
}
