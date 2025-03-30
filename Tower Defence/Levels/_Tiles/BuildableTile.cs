using Abstract.Data;
using Gameplay;
using Godot;
using Turrets;
using UI;
using UI.Inventory;
using UI.Modules;

namespace Levels._Nodes
{
    /// <summary>
    /// Manages all data and actions for a single node on a level map
    /// </summary>
    public partial class BuildableTile : Area2D
    {
        /// <summary>
        /// The colour to set the node when it's being hovered over and the player is trying to build something
        /// </summary>
        [Export]
        public Color HoverColour;
        private Color _defaultColour = Color.FromHsv(0, 0, 1);
        
        /// <summary>
        /// A turret that starts on the node
        /// </summary>
        [Export]
        private TurretBlueprint initialTurret;

        /// <summary>
        /// The node to spawn the module icons as a child of
        /// </summary>
        [Export]
        private TriangleLayout _modulesDisplay;
        /// <summary>
        /// The prefab of a module icon to instantiate to display the turret's modules
        /// </summary>
        [Export]
        private PackedScene _moduleIconPrefab;
        
        // Turret info
        public Turret Turret;
        public TurretBlueprint TurretBlueprint;
        
        [Export]
        private Sprite2D _rend;
        private BuildManager _buildManager;

        // Pointer handling
        private bool _isHolding;

        public override void _EnterTree()
        {
            GD.Print(initialTurret.displayName);
            if (initialTurret != null)
                LoadTurret(initialTurret);
        }

        public override void _Ready()
        {
            _buildManager = BuildManager.instance;
        }
        
        /// <summary>
        /// We load a turret into the node without any fancy build effects or adding modules (those are added separately).
        /// </summary>
        /// <param name="blueprint">The blueprint of the turret to build</param>
        public void LoadTurret(TurretBlueprint blueprint)
        {
            GD.Print("Loading Turret");
            var newTurret = (Turret)blueprint.prefab.Instantiate();
            AddChild(newTurret);
            GD.Print(newTurret.Name);
            newTurret.Name = "_" + newTurret.Name;
            Turret = newTurret;
            TurretBlueprint = blueprint;
        }
        
        /// <summary>
        /// Loads a module without any fancy effects
        /// </summary>
        /// <param name="handler">The module handler to load</param>
        public bool LoadModule(ModuleChainHandler handler)
        {
            // Check handler has a module and tier
            if (handler.GetModule() == null)
            {
                return false;
            }
            
            // Apply the Module
            bool hasAppliedModule = Turret.AddModule(handler);
            return hasAppliedModule;
        }

        /// <summary>
        /// Places the turret on the node
        /// </summary>
        /// <param name="blueprint">The blueprint of the turret to build</param>
        private void BuildTurret(TurretBlueprint blueprint)
        {
            // Spawn the turret and set the turret and blueprint
            Vector2 nodePosition = Position;
            var newTurret = blueprint.prefab.Instantiate<Turret>();
            newTurret.Position = nodePosition;
            newTurret.Name = "_" + newTurret.Name;
            Turret = newTurret;
            TurretBlueprint = blueprint;
            newTurret.displayName = blueprint.displayName;
        
            foreach (ModuleChainHandler handler in blueprint.moduleHandlers)
            {
                newTurret.AddModule(handler);
            }
        
            // Spawn the build effect and destroy after
            Node2D effect = (Node2D)_buildManager.buildEffect.Instantiate();
            effect.Position = Position;
            effect.Name = "_" + effect.Name;
            // TODO - free after correct time
            GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };
        }
    
        /// <summary>
        /// Called when upgrading a turret
        /// </summary>
        /// <param name="handler">The Module to add to the turret</param>
        /// <returns>If the Module was applied</returns>
        public bool ApplyModuleToTurret(ModuleChainHandler handler)
        {
            // Check handler has a module and tier
            if (handler.GetModule() == null)
            {
                return false;
            }
            
            // Apply the Module
            bool hasAppliedModule = Turret.AddModule(handler);
            if (!hasAppliedModule) return false;

            // Spawn the build effect
            Node2D effect = (Node2D)_buildManager.buildEffect.Instantiate();
            effect.Position = Position;
            effect.Name = "_" + effect.Name;
            // TODO - free after correct time
            GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };
        
            // Update the TurretInfo
            TurretInfo.instance.UpdateSelection();
            return true;
        }
    
        /// <summary>
        /// Called when the turret is sold
        /// </summary>
        public void SellTurret(int sellAmount)
        {
            // Grant the money
            GameStats.Energy += sellAmount;

            // Spawn the sell effect
            var effect = (Node2D)_buildManager.sellEffect.Instantiate();
            effect.Position = Position;
            effect.Name = "_" + effect.Name;
            // TODO - free after correct time
            GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };
        
            // Destroy the turret and reset any of the node's selection variables
            Turret.QueueFree();
            TurretBlueprint = null;

            BuildManager.instance.Deselect();
        }

        private void OnMouseDown(Viewport viewport, InputEvent @event, int shapeIndex)
        {
            switch (@event)
            {
                case InputEventMouseButton mouseEvent:
                {
                    if ((mouseEvent.ButtonMask & MouseButtonMask.Left) != 0)
                        HandlePointerInteract();
                    break;
                }
                case InputEventScreenTouch { Canceled: false, Pressed: false } eventTouch:
                {
                    if (eventTouch.Index == 0 && !eventTouch.DoubleTap)
                    {
                        if (!_isHolding)
                            HandlePointerInteract();
                    }

                    break;
                }
                case InputEventScreenTouch { Canceled: false, Pressed: true }:
                    _isHolding = true;
                    break;
                case InputEventScreenDrag:
                    _isHolding = false;
                    break;
            }
        }

        /// <summary>
        /// Handles interaction.
        /// Either Selects the turret or builds
        /// </summary>
        private void HandlePointerInteract()
        {
            // Select the node/turret
            if (Turret != null)
            {
                _buildManager.SelectNode(this);
                return;
            }
            // If the player is clicking an empty node

            // Player doesn't have a build button selected
            if (!_buildManager.HasTurretToBuild)
            {
                _buildManager.Deselect();
                return;
            }

            // Construct a turret
            BuildTurret(_buildManager.GetTurretToBuild());
            _buildManager.BuiltTurret();
            _buildManager.SelectNode(this);
        }
        
        /// <summary>
        /// Called when the mouse hovers over the node
        /// </summary>
        private void OnMouseEnter()
        {
            if (Turret != null)
            {
                UpdateModules();
                _modulesDisplay.Visible = true;
            }
            
            // Make sure the player is trying to build
            if (!_buildManager.HasTurretToBuild)
            {
                return;
            }
            Modulate = HoverColour;
            BuildManager.instance.currentPreview.Position = Position;
            BuildManager.instance.currentPreview.Visible = true;
        }
    
        /// <summary>
        /// Called when the mouse is no longer over the node
        /// </summary>
        private void OnMouseExit()
        {
            if (Turret != null)
            {
                _modulesDisplay.Visible = false;
            }
            
            Modulate = _defaultColour;
            if (BuildManager.instance.currentPreview != null)
                BuildManager.instance.currentPreview.Visible = false;
        }
        
        /// <summary>
        /// Updates the render of the modules for a turret
        /// </summary>
        private void UpdateModules()
        {
            // Removes module icons created from the previously selected turret
            for (var i = 0; i < _modulesDisplay.GetChildCount(); i++)
                _modulesDisplay.GetChild(i).QueueFree();
            
            // Add each Module as an icon
            foreach (ModuleChainHandler handle in Turret.moduleHandlers)
            {
                var moduleIcon = _moduleIconPrefab.Instantiate<ModuleIcon>();
                _modulesDisplay.AddChild(moduleIcon);
                moduleIcon.Position = _modulesDisplay.Position;
                moduleIcon.Name = "_" + moduleIcon.Name;
                moduleIcon.SetData(handle);
                foreach (Node node in moduleIcon.GetChildren())
                {
                    // TODO - Is disabling raycastTarget needed?
                    // if (node is Sprite2D image)
                    //     image.raycastTarget = false;
                }
            }

            _modulesDisplay.SetLayoutHorizontal();
            _modulesDisplay.SetLayoutVertical();
        }
    }
}
