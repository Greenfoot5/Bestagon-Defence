using Gameplay;
using Godot;
using Levels.Maps;

namespace UI.Shop
{
    public partial class SelectionRefunds : Control
    {
        [Export]
        /// <summary>
        /// The shop of the scene")]
        private Shop shop;
        [Export]
        /// <summary>
        /// The component that spawns the cards")]
        private GenerateShopSelection selectionCardsParent;

        [ExportGroup("Buttons")]
        [Export]
        /// <summary>
        /// Revitalise tagline")]
        private Label revitaliseText;
        [Export]
        /// <summary>
        /// Refund tagline")]
        private Label refundText;
        [Export]
        /// <summary>
        /// Reroll tagline")]
        private Label rerollText;
        [Export]
        /// <summary>
        /// The reroll game object with the hexagons and button.")]
        private GodotObject reroll;
        [Export]
        /// <summary>
        /// The color of the reroll hexagons when the player cannot afford it")]
        private Color rerollDisabledColor = new(0.3f, 0.3f, 0.3f, 1f);

        private Color _rerollDefaultColor;
        private int _rerollsLeft;

        private LevelData _levelData;
        
        /// <summary>
        /// Sets up private references
        /// </summary>
        private void Awake()
        {
            // TODO - Get levelData
            // _levelData = BuildManager.instance.GetComponent<GameManager>().levelData;
            // _rerollDefaultColor = reroll.GetComponent<Hexagons>().color;
        }
        
        /// <summary>
        /// Displays the correct values in text, and checks the reroll
        /// </summary>
        private void OnEnable()
        {
            CheckReroll();
        }
        
        /// <summary>
        /// Grants a user heart(s) and closes the selection menu
        /// </summary>
        public void Revitalise()
        {
            var parent = GetNode<Control>("../");
            parent.Visible = false;
            Engine.TimeScale = 1d;
        }
        
        /// <summary>
        /// Refunds (part of) the cost of the selection opening and closes it.
        /// </summary>
        public void Refund()
        {
            int refundAmount = shop.GetEnergyCost();
            // GameStats.Energy += (int) (refundAmount * _levelData.refundPercentage);
            var parent = GetNode<Control>("../");
            parent.Visible = false;
            Engine.TimeScale = 1d;
        }
        
        /// <summary>
        /// Rerolls the current selection at the cost of lives
        /// </summary>
        public void Reroll()
        {
            // Calculate cost
            if (_rerollsLeft <= 0)
            {
                if (_levelData.rerollCost < 1)
                {
                    GameStats.Lives -= Mathf.CeilToInt(_levelData.rerollCost);
                    _rerollsLeft = (int) (1f / _levelData.rerollCost);
                }
                else
                {
                    GameStats.Lives -= (int) _levelData.rerollCost;
                }
            }
            _rerollsLeft--;
            
            // Regenerate and refresh
            selectionCardsParent.GenerateSelection();
            //shop.GenerateSelection();
            CheckReroll();
        }
        
        /// <summary>
        /// Check if the player can reroll the selection, and at what cost (if any)
        /// </summary>
        private void CheckReroll()
        {
            // Check if the player can afford a reroll
            if (GameStats.Lives > _levelData.rerollCost)
            {
                // reroll.GetComponent<Button>().interactable = true;
                // reroll.GetComponent<Hexagons>().color = _rerollDefaultColor;
                // rerollText.color = _rerollDefaultColor;
            }
            else
            {
                // reroll.GetComponent<Button>().interactable = false;
                // reroll.GetComponent<Hexagons>().color = rerollDisabledColor;
                // rerollText.color = rerollDisabledColor;
            }
            
            // Work out if it's a reroll
            if (_rerollsLeft > 0)
            {
                rerollText.Text = "Free for " + _rerollsLeft + " rolls";
            }
            else if (_levelData.rerollCost < 1)
            {
                rerollText.Text = "-" + Mathf.CeilToInt(_levelData.rerollCost) + " <sprite=\"UI-Life\" name=\"life\"> " +
                                  "for " + (int) (1 / _levelData.rerollCost) + " rerolls";
            }
        }
    }
}
