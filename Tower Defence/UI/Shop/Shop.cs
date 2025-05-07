using System;
using Abstract;
using Abstract.Data;
using Gameplay;
using Godot;
using Levels.Maps;
using Turrets;
using UI.Inventory;

namespace UI.Shop
{
    /// <summary>
    /// Handles the shop and inventory of the player
    /// </summary>
    // [RequireComponent(typeof(GenerateShopSelection))]
    public partial class Shop : BaseButton
    {
        private BuildManager _buildManager;
        private LevelData _levelData;
        private ModuleChainHandler _selectedHandler;

        /// <summary>
        /// The inventory to place the turret buttons
        /// </summary>
        [Export]
        private Control _turretInventory;

        /// <summary>
        /// The inventory to place the module buttons
        /// </summary>
        [Export]
        private Control _moduleInventory;

        /// <summary>
        /// The generic turret button scene
        /// </summary>
        [Export]
        private PackedScene _defaultTurretButton;

        /// <summary>
        /// The generic module button scene
        /// </summary>
        [Export]
        private PackedScene _defaultModuleButton;

        /// <summary>
        /// The UI to display when the player opens the shop
        /// </summary>
        [Export]
        public GenerateShopSelection SelectionGenerator;

        private int _nextCost;
        
        public int TotalCellsCollected;

        /// <summary>
        /// Current count of powercells
        /// </summary>
        [Export]
        private Label _powercellCount;

        /// <summary>
        /// Progress to next powercell
        // </summary>
        // [Export]
        // TODO - Progress
        // private Progress powercellProgress;

        /// <summary>
        /// Shop button colours top when can afford
        /// </summary>
        [ExportGroup("Shop Button")] 
        [Export]
        private Sprite2D _buyButton;

        /// <summary>
        /// Shop button image when can afford
        /// </summary>
        [Export]
        private Texture2D _affordButtonImage;

        /// <summary>
        /// Shop buttons image when can't afford
        /// </summary>
        [Export]
        private Texture2D _expensiveButtonImage;

        /// <summary>
        /// Shop button colours button when can afford
        /// </summary>
        [Export]
        private Control _expensiveButtonOverlay;

        /// <summary>
        /// The GlyphsLookup index in the scene
        /// </summary>
        [Export]
        public TypeSpriteLookup GlyphsLookup;

        public static Squirrel3 Random;
        /// <summary>
        /// The previous state of the random before the current selection
        /// </summary>
        public static Tuple<int, int> OldState;

        /// <summary>
        /// Initialises values and set's starting prices
        /// </summary>
        public override void _Ready()
        {
            // TODO - GetComponent
            // _levelData = _buildManager.GetComponent<GameManager>().levelData;

            // It should only be greater than 0 if we've loaded a save
            _nextCost = GetEnergyCost();

            GameStats.OnGainEnergy += CalculateCells;
            GameStats.OnGainPowercell += UpdateBuyButton;
            GameStats.OnRoundProgress += SelectionGenerator.GenerateSelection;
            CalculateCells();
            UpdateBuyButton();
        }

        public override void _ExitTree()
        {
            GameStats.OnGainEnergy -= CalculateCells;
            GameStats.OnGainPowercell -= UpdateBuyButton;
            GameStats.OnRoundProgress -= SelectionGenerator.GenerateSelection;
        }

        /// <summary>
        /// Removes a module from the inventory
        /// </summary>
        public void RemoveModule(Button button)
        {
            button.QueueFree();
        }

        /// <summary>
        /// Adds a new turret button to the turret inventory
        /// </summary>
        /// <param name="turret">The blueprint of the turret to add</param>
        public void SpawnNewTurret(TurretBlueprint turret)
        {
            SelectionGenerator.GenerateSelection();
            SelectionGenerator.Resume();
            SelectionGenerator.Unlock();

            // Add and display the new item
            var turretButton = (TurretInventoryItem)_defaultTurretButton.Instantiate();
            turretButton.Position = _turretInventory.Position;
            turretButton.Name = "_" + turretButton.Name;
            turretButton.Init(turret);
            
            // TODO - Check GetType()
            SelectionGenerator.AddTurretType(turret.prefab.GetType());
            GameManager.TurretInventory.Add(turret);
        }

        /// <summary>
        /// Adds a new module button to the module inventory
        /// </summary>
        /// <param name="module">The module to add</param>
        public void SpawnNewModule(ModuleChainHandler module)
        {
            SelectionGenerator.GenerateSelection();
            SelectionGenerator.Resume();
            SelectionGenerator.Unlock();

            var moduleButton = (ModuleInventoryItem)_defaultModuleButton.Instantiate();
            moduleButton.Position = _moduleInventory.Position;
            moduleButton.Name = "_" + moduleButton.Name;
            moduleButton.Init(module, GlyphsLookup);
            moduleButton.Pressed += () =>
            {
                TurretInfo.instance.ApplyModule(module, moduleButton);
            };

            GameManager.ModuleInventory.Add(module);
        }

        /// <summary>
        /// Gets if the player has made a purchase yet
        /// </summary>
        /// <returns>If the player has made a purchase</returns>
        public bool HasPlayerMadePurchase()
        {
            return TotalCellsCollected - GameStats.Powercells >= _levelData.InitialSelectionCount;
        }

        public int GetSellPercentage()
        {
            return (int)(_levelData.SellPercentage * 100);
        }

        public int GetSellAmount()
        {
            return (int)(_levelData.SellPercentage * _nextCost);
        }

        private void CalculateCells()
        {
            var energyToSubtract = 0;
            while (GameStats.Energy - energyToSubtract > _nextCost && _nextCost != 0)
            {
                TotalCellsCollected += 1;
                _nextCost = GetEnergyCost();
                energyToSubtract += _nextCost;
                GameStats.Powercells++;
            }

            if (energyToSubtract > 0)
                GameStats.Energy -= energyToSubtract;
            UpdateBuyButton();
        }

        private void UpdateBuyButton()
        {
            if (GameStats.Powercells > 0)
            {
                _buyButton.Texture = _affordButtonImage;
                _expensiveButtonOverlay.Visible = false;
                Disabled = false;
            }
            else
            {
                _buyButton.Texture = _expensiveButtonImage;
                _expensiveButtonOverlay.Visible = true;
                Disabled = true;
            }

            UpdateEnergyCount();
        }

        private void UpdateEnergyCount()
        {
            _powercellCount.Text = GameStats.Powercells.ToString();
            // powercellProgress.percentage = GameStats.Energy / (float)nextCost;
        }

        /// <summary>
        /// To make sure the expression evaluator is doing the right thing each time, there's a function.
        /// </summary>
        public int GetEnergyCost()
        {
            var expression = new Expression();
            expression.Parse(_levelData.SelectionCostFormula.Replace("x", $"({TotalCellsCollected.ToString()})"));
            int output =  expression.Execute().AsInt32();
            if (output == 0) 
                GD.PushError("Energy Cost was 0, likely an issue with formula");
            return output;
        }
    }
}
