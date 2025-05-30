using System.Collections.Generic;
using Abstract.Data;
using Abstract.Saving;
using Godot;
using Levels._Nodes;
using Turrets;
using UI.Shop;

namespace Gameplay
{
    /// <summary>
    /// Manages the current game's state
    /// </summary>
    public partial class GameManager : Node, ISaveableLevel
    {
        // If the game has actually finished yet
        public static bool isGameOver;
        
        /// <summary>
        /// The UI to display when the player loses
        /// </summary>
        [Export]
        public Control GameOverUI;

        /// <summary>
        /// The UI that displays the shop
        /// </summary>
        [Export]
        private Shop shop;

        /// <summary>
        /// The label for the lives amount
        /// </summary>
        [Export]
        private Label livesText;
        /// <summary>
        /// The Progress Graphic for the lives bar
        // </summary>
        // TODO - Progress type
        // [Export]
        // private Progress livesBar;
        private int _startLives;

        /// <summary>
        /// The paret of all the nodes
        /// </summary>
        [Export]
        public Node2D NodeParent;

        public static readonly List<TurretBlueprint> TurretInventory = [];
        public static readonly List<ModuleChainHandler> ModuleInventory = [];

        private void Awake()
        {
            _startLives = GameStats.Lives;
            // TODO - Load PlayerPrefs
            // if (PlayerPrefs.GetInt("LoadingLevel", 0) == 0) return;
            // LoadJsonData(this);
        }

        /// <summary>
        /// Makes sure the level has some data to run with
        /// Makes sure that the game isn't over.
        /// </summary>
        public override void _Ready()
        {
            isGameOver = false;
            
            GameStats.OnLoseLife += UpdateLives;
            GameStats.OnGameOver += EndGame;
            UpdateLives();
        }
    
        /// <summary>
        /// Checks if the game is over yet
        /// </summary>
        public override void _Process(double delta)
        {
            // TODO - Move somewhere better
            RenderingServer.GlobalShaderParameterSet("UNSCALED_TIME", Time.GetTicksMsec() / 1000.0);
        }

        private void OnDestroy()
        {
            GameStats.OnLoseLife -= UpdateLives;
        }
    
        /// <summary>
        /// Ends the game.
        /// Displays the game over screen and saves the player's score.
        /// </summary>
        private void EndGame()
        {
            isGameOver = true;

            GameOverUI.ProcessMode = ProcessModeEnum.Always;
            GameOverUI.Visible = true;
            shop.ProcessMode = ProcessModeEnum.Disabled;
            shop.Visible = false;

            Engine.TimeScale = 0;
            
            // TODO - ClearSave
            // SaveManager.ClearSave(SceneManager.GetActiveScene().Name);
        }

        public void PopulateSaveData(SaveLevel saveData)
        {
            var droppedEnergy = 0;
            foreach (DeathEnergy particle in DeathBitManager.Particles)
            {
                // if (!particle.IsAnimating)
                droppedEnergy += particle.Value;
            }
            
            saveData.Energy = GameStats.Energy + droppedEnergy;
            saveData.Powercells = GameStats.Powercells;
            saveData.Lives = GameStats.Lives;
            saveData.WaveIndex = GameStats.Rounds - 1;
            // TODO - GetComponent
            // saveData.TotalCellsCollected = shop.GetComponent<Shop>().totalCellsCollected;
            saveData.Nodes = [];
            saveData.TurretInventory = [];
            saveData.ModuleInventory = [];
            
            // Random
            // saveData.RandomState = Random.state;
            saveData.RandomSeed = Shop.OldState.Item1;
            saveData.ShopRandomN = Shop.OldState.Item2;

            // Node Data
            foreach (Node node in NodeParent.GetChildren())
            {
                var tile = (BuildableTile)node;
                if (tile.Turret == null)
                {
                    continue;
                }

                Turret turret = tile.Turret;
                List<string> names = [];
                List<int> tiers = [];
                foreach (ModuleChainHandler handler in turret.ModuleHandlers)
                {
                    names.Add(handler.GetChain().GetName());
                    tiers.Add(handler.GetTier());
                }
                
                var nodeData = new SaveLevel.NodeData
                {
                    uuid = tile.Name,
                    blueprintName = tile.TurretBlueprint.GetName(),
                    moduleNames = names,
                    moduleTiers = tiers
                };

                if (turret.GetType().IsSubclassOf(typeof(DynamicTurret)))
                {
                    nodeData.turretRotation = ((DynamicTurret)turret).PartToRotate.Rotation;
                    nodeData.targetingMethod = ((DynamicTurret)turret).TargetPriorityMethod;
                }

                saveData.Nodes.Add(nodeData);
            }
            
            // Inventory
            // Turrets
            foreach (TurretBlueprint turret in TurretInventory)
            {
                saveData.TurretInventory.Add(turret);
            }
            // Modules
            foreach (ModuleChainHandler module in ModuleInventory)
            {
                saveData.ModuleInventory.Add(module);
            }
        }
        
        /// <summary>
        /// Loads the data from json
        /// </summary>
        private static void LoadJsonData(ISaveableLevel level)
        {
            // TODO - LoadLevel
            // SaveManager.LoadLevel(level, SceneManager.GetActiveScene().Name);
        }

        public void LoadFromSaveData(SaveLevel saveData)
        {
            _startLives = GameStats.Lives;
            GameStats.Lives = saveData.Lives;
            GameStats.PopulateRounds(saveData.WaveIndex);
            Shop shopComponent = shop;
            shopComponent.TotalCellsCollected = saveData.TotalCellsCollected;
            GameStats.Powercells = saveData.Powercells;
            GameStats.Energy = saveData.Energy;
            
            // Random
            // Random.state = saveData.RandomState;
            Shop.Random = new Squirrel3(saveData.RandomSeed, saveData.ShopRandomN);

            foreach (SaveLevel.NodeData nodeData in saveData.Nodes)
            {
                foreach (Node node in NodeParent.GetChildren())
                {
                    var tile = (BuildableTile)node;
                    if (tile.Name != nodeData.uuid) continue;
                    
                    tile.LoadTurret(SaveLevel.Blueprints[nodeData.blueprintName]);
                    for (var i = 0; i < nodeData.moduleNames.Count; i++)
                    {
                        tile.LoadModule(new ModuleChainHandler(SaveLevel.Chains[nodeData.moduleNames[i]], nodeData.moduleTiers[i]));
                    }
                    
                    Turret turret = tile.Turret;
                    shopComponent.SelectionGenerator.AddTurretType(turret.GetType());
                    if (turret.GetType().IsSubclassOf(typeof(DynamicTurret)))
                    {
                        ((DynamicTurret)turret).PartToRotate.Rotation = nodeData.turretRotation;
                        ((DynamicTurret)turret).TargetPriorityMethod = nodeData.targetingMethod;
                    }
                }
            }

            foreach (TurretBlueprint turret in saveData.TurretInventory)
            {
                shopComponent.SpawnNewTurret(turret);
            }
            foreach (ModuleChainHandler module in saveData.ModuleInventory)
            {
                shopComponent.SpawnNewModule(module);
            }
        }

        private void UpdateLives()
        {
            // livesBar.percentage = GameStats.Lives / (float)_startLives;
            livesText.Text = $"{GameStats.Lives}";
        }
    }
}
