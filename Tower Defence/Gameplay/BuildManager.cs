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
        /// The instance of the BuildManager
        /// </summary>
        public static BuildManager instance;
        
        /// <summary>
        /// The effect spawned when a turret is built
        /// </summary>
        // [Export]
        public PackedScene buildEffect;
        /// <summary>
        /// The effect spawned when a turret is sold
        /// </summary>
        // [Export]
        public PackedScene sellEffect;
        
                
        /// <summary>
        /// The scene to use when displaying potential range when building
        /// </summary>
        // [Export]
        private PackedScene rangePreview;
        /// <summary>
        /// The current range preview
        /// </summary>
        public Node2D currentPreview;

        private TurretBlueprint _turretToBuild;
        private TurretInventoryItem _buildingButton;
        private BuildableTile _selectedTile;
        
        /// <summary>
        /// If the player is currently building or not
        /// </summary>
        public bool HasTurretToBuild => _turretToBuild != null;
        
        /// <summary>
        /// Check there is only one build manager when loading in
        /// </summary>
        public override void _Ready()
        {
            // Make sure there is only ever have one BuildManager
            if (instance != null)
            {
                GD.PushError("More than one build manager in scene!");
                return;
            }
            instance = this;
        }

        /// <summary>
        /// Sets the turret the player want's to build
        /// </summary>
        /// <param name="turret">The blueprint of the turret to build</param>
        /// <param name="buttonToDelete">The inventory button to remove</param>
        public void SelectTurretToBuild(TurretBlueprint turret, TurretInventoryItem buttonToDelete)
        {
            _turretToBuild = turret;
            _buildingButton = buttonToDelete;
            currentPreview = (Node2D)rangePreview.Instantiate();
            // TODO - GetComponent
            // float range = turret.prefab.GetComponent<Turret>().range.GetStat();
            float range = 5;
            currentPreview.Scale = new Vector2(range * 2, range * 2);
            // currentPreview.GetComponent<SpriteRenderer>().color = turret.accent;
        }
        
        /// <summary>
        /// Let the build manager know the turret has been constructed
        /// </summary>
        public void BuiltTurret()
        {
            currentPreview.QueueFree();
            _buildingButton.QueueFree();
            GameManager.TurretInventory.Remove(_turretToBuild);
            _turretToBuild = null;
        }
    
        /// <summary>
        /// Gets the blueprint of the turret the player currently want to build
        /// </summary>
        /// <returns>The turret blueprint of the turret the player wants to build</returns>
        public TurretBlueprint GetTurretToBuild()
        {
            return _turretToBuild;
        }
    
        /// <summary>
        /// Sets the selected node
        /// </summary>
        /// <param name="tile">The selected node</param>
        public void SelectNode(BuildableTile tile)
        {
            if (_selectedTile == tile)
            {
                Deselect();
                // TurretInfo.instance.Close();
                return;
            }

            if (_selectedTile != null)
            {
                // Clear any previous selection
                Deselect();
            }

            _selectedTile = tile;
            // TurretInfo.instance.SetTarget(tile);
        }

        public void Deselect()
        {
            _turretToBuild = null;

            if (_selectedTile != null && _selectedTile.Turret != null)
            {
                // TODO - GetComponent
                // _selectedNode.Turret.GetComponent<Turret>().Deselected();
            }
            _selectedTile = null;
            // TurretInfo.instance.Close();
        }
    }
}
