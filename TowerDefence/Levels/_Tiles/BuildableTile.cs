using BestagonDefence.Abstract.Data;
using BestagonDefence.Gameplay;
using BestagonDefence.Turrets;
using BestagonDefence.UI;
using BestagonDefence.UI.Modules;
using Godot;

namespace BestagonDefence.Levels._Tiles;

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
    private TurretBlueprint _initialTurret;

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
    [Export]
    private AnimationPlayer _animator;

    // Pointer handling
    private bool _isHolding;
        
    private static BuildableTile _selectedTile;
    public static BuildableTile SelectedTile
    {
        get => _selectedTile;
        set
        {
            _selectedTile = value;
            OnTileSelected?.Invoke(value);
        }
    }

    public static event SelectTile OnTileSelected;
    public delegate void SelectTile(BuildableTile tile);
    
    /// <summary>
    /// Initialises the BuildableTile
    /// </summary>
    public override void _EnterTree()
    {
        if (_initialTurret != null)
            LoadTurret(_initialTurret);
            
        InputEvent += OnMouseDown;
        MouseEntered += OnMouseEnter;
        MouseExited += OnMouseExit;
    }
    
    /// <summary>
    /// Removes listeners when it leaves the tree
    /// </summary>
    public override void _ExitTree()
    {
        InputEvent -= OnMouseDown;
        MouseEntered -= OnMouseEnter;
        MouseExited -= OnMouseExit;
    }
        
    /// <summary>
    /// We load a turret into the node without any fancy build effects or adding modules (those are added separately).
    /// </summary>
    /// <param name="blueprint">The blueprint of the turret to build</param>
    public void LoadTurret(TurretBlueprint blueprint)
    {
        var newTurret = (Turret)blueprint.Prefab.Instantiate();
        AddChild(newTurret);
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
    /// Instantiate and place the turret on the tile
    /// </summary>
    private void BuildBlueprint()
    {
        // Spawn the turret and set the turret and blueprint
        var newTurret = TurretBlueprint.Prefab.Instantiate<Turret>();
        newTurret.Name = "_" + newTurret.Name;
        Turret = newTurret;
        newTurret.DisplayName = TurretBlueprint.DisplayName;
        
        foreach (ModuleChainHandler handler in TurretBlueprint.ModuleHandlers)
        {
            newTurret.AddModule(handler);
        }
            
        _rend.AddChild(newTurret);
        BuildManager.TurretBuilt();
        OnTileSelected?.Invoke(this);
    }

    /// <summary>
    /// Places the turret on the node
    /// </summary>
    /// <param name="blueprint">The blueprint of the turret to build</param>
    private void BuildTurret(TurretBlueprint blueprint)
    {
        TurretBlueprint = blueprint;
        _animator.Play("BuildableTile/Build");
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
        // TODO - Module upgrade effect
            
        // Node2D effect = (Node2D)_buildManager.buildEffect.Instantiate();
        // effect.Position = Position;
        // effect.Name = "_" + effect.Name;
            
        // TODO - free after correct time
            
        // GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };
            
        // Update the TurretInfo
        SelectedTile = this;
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
        // TODO - Sell Effect?
        //// var effect = (Node2D)_buildManager.sellEffect.Instantiate();
        // effect.Position = Position;
        // effect.Name = "_" + effect.Name;
        // TODO - free after correct time
        //// GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };
        
        // Destroy the turret and reset any of the node's selection variables
        Turret.QueueFree();
        TurretBlueprint = null;

        SelectedTile = null;
    }

    /// <summary>
    /// Handles clicking on the turret
    /// </summary>
    /// <param name="viewport">The viewport the mouse is in</param>
    /// <param name="event">The input event that triggered</param>
    /// <param name="shapeIndex">Child index of the clicked Shape2D</param>
    private void OnMouseDown(Node viewport, InputEvent @event, long shapeIndex)
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
            SelectedTile = this;
            return;
        }
            
        // If the player is clicking on the same node
        if (SelectedTile == this)
        {
            SelectedTile = null;
            return;
        }
            
        // Player doesn't have a build button selected
        if (!BuildManager.HasTurretToBuild)
        {
            SelectedTile = null;
            return;
        }

        // Construct a turret
        BuildTurret(BuildManager.GetTurretToBuild());
    }
        
    /// <summary>
    /// Called when the mouse hovers over the node
    /// </summary>
    private void OnMouseEnter()
    {
        _animator.Queue(new StringName("BuildableTile/OnMouseEnter"));
        if (Turret != null)
        {
            // UpdateModules();
            // _modulesDisplay.Visible = true;
        }
            
        // Make sure the player is trying to build
        if (!BuildManager.HasTurretToBuild)
        {
            return;
        }
        SelfModulate = HoverColour;
        // TODO - Move Module preview
        // BuildManager.instance.currentPreview.Position = Position;
        // BuildManager.instance.currentPreview.Visible = true;
    }
    
    /// <summary>
    /// Called when the mouse is no longer over the node
    /// </summary>
    private void OnMouseExit()
    {
        _animator.Queue(new StringName("BuildableTile/OnMouseExit"));
        if (Turret != null)
        {
            // _modulesDisplay.Visible = false;
        }
        SelfModulate = _defaultColour;
        // TODO - Disable Module Preview
        // if (BuildManager.instance.currentPreview != null)
        //     BuildManager.instance.currentPreview.Visible = false;
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
        foreach (ModuleChainHandler handle in Turret.ModuleHandlers)
        {
            var moduleIcon = _moduleIconPrefab.Instantiate<ModuleIcon>();
            _modulesDisplay.AddChild(moduleIcon);
            moduleIcon.Position = _modulesDisplay.Position;
            moduleIcon.Name = "_" + moduleIcon.Name;
            moduleIcon.SetData(handle);
        }

        _modulesDisplay.SetLayoutHorizontal();
        _modulesDisplay.SetLayoutVertical();
    }
}