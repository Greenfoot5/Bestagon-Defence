using System;
using Abstract.Attributes;
using Abstract.Data;
using Godot;
using Levels._Nodes;
using Turrets;
using Turrets.Lancer;
using UI.Modules;
using UI.TurretStats;

namespace UI.Inventory;

public partial class TurretInfo : Control
{
    private BuildableTile _target;

    /// <summary>
    /// The Shop component of the scene
    /// </summary>
    [Export]
    private Shop.Shop shop;
        
    /// <summary>
    /// The inventory show/hide for the turrets
    /// </summary>
    [ExportGroup("Turret Inventory")]
    [Export]
    private Control turretInventoryPage;
    /// <summary>
    /// The button to open the turret inventory
    /// </summary>
    [Export]
    private Container turretInventoryContent;
        
    /// <summary>
    /// The inventory to show/hide for the modules
    /// </summary>
    [ExportGroup("Module Inventory")]
    [Export]
    private Control moduleInventoryPage;
    /// <summary>
    /// The text to display for the title
    /// </summary>
    [Export]
    private string moduleInventoryTitle;
    /// <summary>
    /// The button to open the turret inventory
    /// </summary>
    [Export]
    private Button turretInfoButton;
    /// <summary>
    /// The button list
    /// </summary>
    [Export]
    private BuildableTile moduleInventoryContent;
    /// <summary>
    /// The colour to set the bg when disabled")]
    /// </summary>
    [Export]
    private Color moduleDisabledColor;
        
    /// <summary>
    /// The page show/hide for the turret info
    /// </summary>
    [ExportGroup("Turret Info")]
    [Export]
    private Control turretInfoPage;
    /// <summary>
    /// The label for the turret name
    /// </summary>
    [Export]
    private Label inventoryTitle;
    /// <summary>
    /// The button to add more modules
    /// </summary>
    [Export]
    private PackedScene addModuleButton;
        
    /// <summary>
    /// The TurretStat used to display the damage
    /// </summary>
    [ExportSubgroup("TurretStat")]
    [Export]
    private TurretStat damage;
    /// <summary>
    /// The TurretStat used to display the fire rate
    /// </summary>
    [Export]
    private TurretStat rate;
    /// <summary>
    /// The TurretStat used to display the range
    /// </summary>
    [Export]
    private TurretStat range;
        
    /// <summary>
    /// The GodotObject to spawn the module icons as a child of
    /// </summary>
    [ExportSubgroup("Modules")]
    [Export]
    private Node modules;
    /// <summary>
    /// The prefab of a module icon to instantiate to display the turret's modules
    /// </summary>
    [Export]
    private PackedScene moduleIconPrefab;
        

    /// <summary>
    /// The button that changes the targeting method of the turret
    /// </summary>
    [ExportGroup("Buttons")]
    [Export]
    private BaseButton cycleTargetingButton;

    public override void _Ready()
    {
        BuildableTile.OnTileSelected += SetTarget;
        Shop.Shop.OnPickTurret += AddTurret;
    }
        
    /// <summary>
    /// Called when selecting a new node
    /// </summary>
    /// <param name="tile">The new node to display UI for</param>
    public void SetTarget(BuildableTile tile)
    {
        if (tile == null || _target == tile)
        {
            return;
        }
        
        _target = tile;
        
        // Display the radius of the turret
        _target.Turret.Selected();

        // Enable/Disable Targeting types cycle button if it's (not) a dynamic turret.
        if (_target.Turret is DynamicTurret dynamicTurret)
        {
            cycleTargetingButton.Visible = true;
            cycleTargetingButton.GetChild<Label>(0).Text = "Targeting:\n" + dynamicTurret.TargetPriorityMethod;
            // TODO - Clear all other listeners
            cycleTargetingButton.Pressed += CycleTargeting;
        }
        else if (_target.Turret is Lancer)
        {
            cycleTargetingButton.Visible = true;
            cycleTargetingButton.GetChild<Label>(0).Text = "Rotate";
            // cycleTargetingButton.Pressed += () => RemoveAllListeners();
            cycleTargetingButton.Pressed += RotateLancer;
        }
        else
        {
            cycleTargetingButton.Visible = false;
        }

        // // Rebuild the Modules and add the stats
        // if (moduleInventoryPage.activeSelf)
        //     OpenModuleInventory();
        // else
        OpenTurretInfo();
    }
        
    /// <summary>
    /// Turret's targeting method increments once through the cycle of targeting methods
    /// </summary>
    private void CycleTargeting()
    {
        Array types = Enum.GetValues(typeof(DynamicTurret.TargetingMethod));
        var dynamic = (DynamicTurret)_target.Turret;
        var currentMethod = (int)dynamic.TargetPriorityMethod;
        dynamic.TargetPriorityMethod = (DynamicTurret.TargetingMethod)( (currentMethod + 1) % types.Length);
            
        // Update our button text
        cycleTargetingButton.GetChild<Label>(0).Text = "Targeting:\n" + dynamic.TargetPriorityMethod;
    }
        
    /// <summary>
    /// Updates the stats display when the turret is selected or upgraded
    /// </summary>
    public void UpdateStats()
    {
        if (_target?.Turret is null) return;
        Turret turret = _target.Turret;
        // Stats
        damage.SetData(turret.Stats[AttributeType.Damage]);
        rate.SetData(turret.Stats[AttributeType.FireRate]);
        range.SetData(turret.Stats[AttributeType.Range]);
        // Display the radius of the turret
        turret.Selected();
        Color color = turret.RangeDisplay.Modulate;
        damage.SetColor(color);
        rate.SetColor(color);
        range.SetColor(color);
    }

    public void UpdateSelection()
    {
        UpdateStats();
        UpdateModules();
    }
        
    /// <summary>
    /// Sells the turret
    /// </summary>
    public void SellTurret()
    {
        _target.SellTurret(shop.GetSellAmount());
    }
        
    /// <summary>
    /// Rotates Lancer Turret
    /// </summary>
    private void RotateLancer()
    {
        ((Lancer)_target.Turret).partToRotate.Rotate(-60);
    }
        
    /// <summary>
    /// Updates the render of the modules for a turret
    /// </summary>
    private void UpdateModules()
    {
        // Removes module icons created from the previously selected turret
        for (var i = 0; i < modules.GetChildCount(); i++)
            modules.GetChild(i).QueueFree();
            
        // Add each Module as an icon
        foreach (ModuleChainHandler handle in _target.Turret.moduleHandlers)
        {
            var icon = moduleIconPrefab.Instantiate<ModuleIcon>();
            modules.AddChild(icon);
            icon.Name = "_" + icon.Name;
            icon.SetData(handle);
            // TODO - Is disabling raycast target needed?
            // foreach (Image image in icon.GetComponentsInChildren<Image>())
            // {
            //     image.raycastTarget = false;
            // }
        }
            
        // Button addModule = addModuleButton.Instantiate<Button>();
        // modules.AddChild(addModule);
        // addModule.Pressed += OpenModuleInventory;
            
        // modules.GetChild<TriangleLayout>(0).SetLayoutHorizontal();
        // modules.GetChild<TriangleLayout>(0).SetLayoutVertical();
    }
        
    public void DisplayTurretInventory()
    {
        BuildableTile.SelectedTile = null;
        // Shuw();
        turretInventoryPage.Visible = true;
        moduleInventoryPage.Visible = false;
        turretInfoPage.Visible = false;
    }
        
    public void ToggleTurretInventory()
    {
        if (turretInventoryPage.Visible)
        {
            // TODO - Let tile know to deselect
            return;
        }
        DisplayTurretInventory();
    }

    public void ToggleModuleInventory()
    {
        if (moduleInventoryPage.Visible)
        {
            BuildableTile.SelectedTile = null;
            return;
        }
        OpenModuleInventory();
    }

    public void OpenModuleInventory()
    {
        foreach (Node child in moduleInventoryContent.GetChildren())
        {
            var item = child as ModuleInventoryItem;
            if (_target != null && item != null && item.IsValid(_target.Turret))
            {
                // item.bg.color = item.accent;
                item.modulesBg.SelfModulate = item.accent * new Color(1, 1, 1, 0.16f);
                item.Disabled = false;
            }
            else if (item != null)
            {
                // item.bg.color = moduleDisabledColor;
                item.modulesBg.SelfModulate = moduleDisabledColor * new Color(1, 1, 1, 0.16f);
                item.Disabled = true;
            }
        }
            
        Shuw();
        inventoryTitle.Text = moduleInventoryTitle;
        moduleInventoryPage.Visible = true;
        turretInventoryPage.Visible = false;
        turretInfoPage.Visible = false;
    }

    public void OpenTurretInfo()
    {
        if (turretInfoPage.Visible)
        {
            BuildableTile.SelectedTile = null;
            return;
        }
            
        Shuw();
        inventoryTitle.Text = _target.TurretBlueprint.DisplayName;
        turretInfoPage.Visible = true;
        turretInventoryPage.Visible = false;
        moduleInventoryPage.Visible = false;
        // turretInventoryButton.Visible = false;

        // TODO - Was GettingComponent<Button>, does still work?
        // turretInfoButton.SelfModulate = _target.TurretBlueprint.Accent;
            
        UpdateStats();
        UpdateModules();
    }

    public void Close()
    {
        turretInfoPage.Visible = false;
        turretInventoryPage.Visible = false;
        moduleInventoryPage.Visible = false;
        // turretInventoryButton.Visible = true;
        turretInfoButton.Visible = false;
            
        _target = null;
            
        // TODO - Anchor
        // var rt = (RectTransform)transform;
        // rt.anchorMin = new Vector2(-0.25f, rt.anchorMin.Y);
        // rt.anchorMax = new Vector2(0f, rt.anchorMax.Y);
    }

    private void Shuw()
    {
        // var rt = (RectTransform)transform;
        // rt.anchorMin = new Vector2(0f, rt.anchorMin.Y);
        // rt.anchorMax = new Vector2(0.25f, rt.anchorMax.Y);
    }

    private void AddTurret(TurretInventoryItem blueprint)
    {
        turretInventoryContent.AddChild(blueprint);
    }
}