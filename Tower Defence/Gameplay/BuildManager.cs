using Godot;
using Levels._Nodes;
using Turrets;
using UI.Inventory;


namespace Gameplay
{
    /// <summary>
    /// Handles all tasks related to building turrets and selecting nodes
    /// </summary>
    public partial class BuildManager : Node
    {
        /// <summary>
        /// The scene to use when displaying potential range when building
        /// </summary>
        // [Export]
        private PackedScene rangePreview;
        /// <summary>
        /// The current range preview
        /// </summary>
        [Export]
        public Node2D CurrentPreview;
        
        private static TurretInventoryItem _buildingButton;
        
        /// <summary>
        /// If the player is currently building or not
        /// </summary>
        public static bool HasTurretToBuild => _buildingButton != null && IsInstanceValid(_buildingButton);
        
        public static event SelectBlueprintEvent OnBlueprintSelected;
        public delegate void SelectBlueprintEvent(TurretInventoryItem inventoryItem);

        public static void SelectBlueprint(TurretInventoryItem inventoryItem)
        {
            OnBlueprintSelected?.Invoke(inventoryItem);
        }
        
        public static event TurretBuiltEvent OnTurretBuilt;
        public delegate void TurretBuiltEvent();

        public static void TurretBuilt()
        {
            OnTurretBuilt?.Invoke();
        }
        
        /// <summary>
        /// Check there is only one build manager when loading in
        /// </summary>
        public override void _Ready()
        {
            OnBlueprintSelected += SelectTurretToBuild;
            BuildableTile.OnTileSelected += SelectTile;
        }

        /// <summary>
        /// Sets the turret the player want's to build
        /// </summary>
        /// <param name="buttonToDelete">The inventory button to remove</param>
        public void SelectTurretToBuild(TurretInventoryItem buttonToDelete)
        {
            _buildingButton = buttonToDelete;
            CurrentPreview = (Node2D)rangePreview.Instantiate();
            // TODO - GetComponent
            // float range = turret.prefab.GetComponent<Turret>().range.GetStat();
            float range = 5;
            CurrentPreview.Scale = new Vector2(range * 2, range * 2);
            // currentPreview.GetComponent<SpriteRenderer>().color = turret.accent;
        }
        
        /// <summary>
        /// Let the build manager know the turret has been constructed
        /// </summary>
        public void BuiltTurret()
        {
            CurrentPreview.QueueFree();
            _buildingButton.QueueFree();
            GameManager.TurretInventory.Remove(_buildingButton.TurretBlueprint);
        }
    
        /// <summary>
        /// Gets the blueprint of the turret the player currently want to build
        /// </summary>
        /// <returns>The turret blueprint of the turret the player wants to build</returns>
        public static TurretBlueprint GetTurretToBuild()
        {
            return IsInstanceValid(_buildingButton) ? _buildingButton.TurretBlueprint : null;
        }
    
        /// <summary>
        /// Sets the selected node
        /// </summary>
        /// <param name="tile">The selected node</param>
        private static void SelectTile(BuildableTile tile)
        {
            if (tile != null)
            {
                _buildingButton = null;
            }
        }
    }
}
