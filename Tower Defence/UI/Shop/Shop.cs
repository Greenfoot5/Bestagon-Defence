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
    public partial class Shop : Control
    {
        private BuildManager _buildManager;
        private LevelData _levelData;
        private ModuleChainHandler _selectedHandler;

        /// <summary>
        /// The inventory to place the turret buttons
        /// </summary>
        [Export]
        private Control turretInventory;

        /// <summary>
        /// The inventory to place the module buttons
        /// </summary>
        [Export]
        private Control moduleInventory;

        /// <summary>
        /// The generic turret button scene
        /// </summary>
        [Export]
        private PackedScene defaultTurretButton;

        /// <summary>
        /// The generic module button scene
        /// </summary>
        [Export]
        private PackedScene defaultModuleButton;

        /// <summary>
        /// The UI to display when the player opens the shop
        /// </summary>
        [Export]
        public GenerateShopSelection selectionGenerator;
        
        public int nextCost;
        
        public int totalCellsCollected;

        /// <summary>
        /// Current count of powercells
        /// </summary>
        [Export]
        private RichTextLabel powercellCount;

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
        private Sprite2D buyButton;
        private Button _buyButtonButton;

        /// <summary>
        /// Shop button image when can afford
        /// </summary>
        [Export]
        private Texture2D affordButtonImage;

        /// <summary>
        /// Shop buttons image when can't afford
        /// </summary>
        [Export]
        private Texture2D expensiveButtonImage;

        /// <summary>
        /// Shop button colours button when can afford
        /// </summary>
        [Export]
        private Control expensiveButtonOverlay;

        /// <summary>
        /// The GlyphsLookup index in the scene
        /// </summary>
        [Export]
        public TypeSpriteLookup glyphsLookup;

        public static Squirrel3 random;
        /// <summary>
        /// The previous state of the random before the current selection
        /// </summary>
        public static Tuple<int, int> oldState;

        /// <summary>
        /// Initialises values and set's starting prices
        /// </summary>
        public override void _Ready()
        {
            _buildManager = BuildManager.instance;
            // TODO - GetComponent
            // _levelData = _buildManager.GetComponent<GameManager>().levelData;
            // _buyButtonButton = buyButton.gameObject.GetComponent<Button>();
            // selectionGenerator = GetComponent<GenerateShopSelection>();

            // It should only be greater than 0 if we've loaded a save
            nextCost = GetEnergyCost();

            GameStats.OnGainEnergy += CalculateCells;
            GameStats.OnGainPowercell += UpdateBuyButton;
            GameStats.OnRoundProgress += selectionGenerator.GenerateSelection;
            CalculateCells();
            UpdateBuyButton();
        }

        private void OnDestroy()
        {
            GameStats.OnGainEnergy -= CalculateCells;
            GameStats.OnGainPowercell -= UpdateBuyButton;
            GameStats.OnRoundProgress -= selectionGenerator.GenerateSelection;
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
            selectionGenerator.GenerateSelection();
            selectionGenerator.Resume();
            selectionGenerator.Unlock();

            // Add and display the new item
            var turretButton = (TurretInventoryItem)defaultTurretButton.Instantiate();
            turretButton.Position = turretInventory.Position;
            turretButton.Name = "_" + turretButton.Name;
            turretButton.Init(turret);
            
            // TODO - Check GetType()
            selectionGenerator.AddTurretType(turret.prefab.GetType());
            GameManager.TurretInventory.Add(turret);
        }

        /// <summary>
        /// Adds a new module button to the module inventory
        /// </summary>
        /// <param name="module">The module to add</param>
        public void SpawnNewModule(ModuleChainHandler module)
        {
            selectionGenerator.GenerateSelection();
            selectionGenerator.Resume();
            selectionGenerator.Unlock();

            var moduleButton = (ModuleInventoryItem)defaultModuleButton.Instantiate();
            moduleButton.Position = moduleInventory.Position;
            moduleButton.Name = "_" + moduleButton.Name;
            moduleButton.Init(module, glyphsLookup);
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
            return totalCellsCollected - GameStats.Powercells >= _levelData.initialSelectionCount;
        }

        public int GetSellPercentage()
        {
            return (int)(_levelData.sellPercentage * 100);
        }

        public int GetSellAmount()
        {
            return (int)(_levelData.sellPercentage * nextCost);
        }

        private void CalculateCells()
        {
            var energyToSubtract = 0;
            while (GameStats.Energy - energyToSubtract > nextCost && nextCost != 0)
            {
                totalCellsCollected += 1;
                nextCost = GetEnergyCost();
                energyToSubtract += nextCost;
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
                buyButton.Texture = affordButtonImage;
                expensiveButtonOverlay.Visible = false;
                _buyButtonButton.Disabled = false;
            }
            else
            {
                buyButton.Texture = expensiveButtonImage;
                expensiveButtonOverlay.Visible = true;
                _buyButtonButton.Disabled = true;
            }

            UpdateEnergyCount();
        }

        private void UpdateEnergyCount()
        {
            powercellCount.Text = GameStats.Powercells.ToString();
            // powercellProgress.percentage = GameStats.Energy / (float)nextCost;
        }

        /// <summary>
        /// To make sure the expression evaluator is doing the right thing each time, there's a function.
        /// </summary>
        public int GetEnergyCost()
        {
            var expression = new Expression();
            expression.Parse(_levelData.selectionCostFormula.Replace("x", $"({totalCellsCollected.ToString()})"));
            int output =  expression.Execute().AsInt32();
            if (output == 0) 
                GD.PushError("Energy Cost was 0, likely an issue with formula");
            return output;
        }
    }
}
