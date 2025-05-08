using System;
using System.Collections.Generic;
using System.Linq;
using Abstract.Data;
using Gameplay;
using Godot;
using Levels.Maps;
using Turrets;

using Object = object;

namespace UI.Shop;

public enum HiddenMode
{
    Disabled,
    Count,
    Chance
}
    
public partial class GenerateShopSelection : Control
{
    /// <summary>
    /// The game object for a turret selection card
    /// </summary>
    [Export]
    private PackedScene _turretSelectionUI;
    /// <summary>
    /// The game object for a module selection card
    /// </summary>
    [Export]
    private PackedScene _moduleSelectionUI;
    /// <summary>
    /// The game object for a life selection card
    /// </summary>
    [Export]
    private PackedScene _lifeSelectionUI;
    /// <summary>
    /// The game object for a hidden selection card
    /// </summary>
    [Export]
    private PackedScene _hiddenSelectionUI;
    private ShopData _shopData;
    [Export]
    private Shop _shop;
        
    /// <summary>
    /// The turrets already purchased
    /// </summary>
    // [Export]
    // TODO - Export
    private readonly List<Type> _turretTypes = new() {typeof(Turret)};

    /// <summary>
    /// The button to show when unlocked
    /// </summary>
    [Export]
    private BaseButton _lockButton;
    /// <summary>
    /// The status to show when locked
    /// </summary>
    [Export]
    private BaseButton _lockedButton;
    private bool _isLocked;

    private List<Tuple<Object, int>> _hiddenChoices;
    private double _openTimeScale;
    
    /// <summary>
    /// Setups references, checks the player has enough gold and freezes the game when enabled
    /// </summary>
    public override void _Ready()
    {
        _shopData = _shop.ShopData;
    }
        
    /// <summary>
    /// Generates the selection of the shop
    /// </summary>
    public void GenerateSelection()
    {
        if (_isLocked) return;

        Shop.OldState = Shop.Random.GetState();
        if (_shopData.HiddenMode != HiddenMode.Disabled)
            _hiddenChoices = new List<Tuple<Object, int>>();

        // Destroy the previous selection
        for (int i = GetChildCount() - 1; i >= 0; i--)
        {
            GetChild(i).QueueFree();
        }
            
        int selectionCount = _shop.HasPlayerMadePurchase() ? _shopData.SelectionChoices : _shopData.InitialChoices;
        // Tracks what the game has given the player, so the game don't give duplicates
        var selectedTurrets = new List<TurretBlueprint>();
        var selectedModules = new List<ModuleChainHandler>();
        var hasLife = false;
            
        // We want to warn if there's a round where a selection couldn't be fully generated
        if (_shop.HasPlayerMadePurchase())
            CheckCategories(selectionCount);

        for (var i = 0; i < selectionCount; i++)
        {
            // If it's the first time opening the shop this level, the game should display a different selection
            if (!_shop.HasPlayerMadePurchase())
            {
                // Grants an Module option
                selectedTurrets.Add(GenerateInitialItem(i, selectedTurrets));
            }
            else
            {
                // Select if the game should get a module, turret or life
                // Can only have one life option
                // We clamp to make sure they don't affect each other if < 0
                float choice = Shop.Random.Range(0f,
                    Mathf.Clamp(_shopData.TurretOptionWeight.Sample(GameStats.Rounds), 0f, float.MaxValue)
                    + Mathf.Clamp(_shopData.ModuleOptionWeight.Sample(GameStats.Rounds), 0f, float.MaxValue)
                    + (!hasLife ? 1 : 0) * Mathf.Clamp(_shopData.LifeOptionWeight.Sample(GameStats.Rounds), 0f, float.MaxValue));
                if (choice <= _shopData.ModuleOptionWeight.Sample(GameStats.Rounds))
                {
                    // Grants an Module option
                    selectedModules.Add(GenerateModuleItem(i, selectedModules));

                }
                else if (_shopData.ModuleOptionWeight.Sample(GameStats.Rounds) < choice && choice <=
                         _shopData.ModuleOptionWeight.Sample(GameStats.Rounds) + _shopData.TurretOptionWeight.Sample(GameStats.Rounds))
                {
                    selectedTurrets.Add(GenerateTurretItem(i, selectedTurrets));
                }
                else
                {
                    if (ShouldHide(i))
                        GenerateHiddenUI(_shopData.LifeCount, i);
                    else
                        GenerateLifeItem();
                        
                    hasLife = true;
                }
            }
        }
    }

    private TurretBlueprint GenerateInitialItem(int selectionIndex, ICollection<TurretBlueprint> selectedTurrets)
    {
        // Grants a turret option
        var turrets = new WeightedList<TurretBlueprint>(_shopData.InitialTurretSelection);
        turrets.RemoveUnweighted();
        TurretBlueprint selected = turrets.GetRandomItem(duplicateType: _shopData.InitialDuplicateCheck,
            previousPicks: selectedTurrets.Take(selectionIndex).ToArray(), rng: Shop.Random);
            
        // Add the turret to the ui for the player to pick
        GenerateTurretUI(selected);
            
        return selected;
    }

    private TurretBlueprint GenerateTurretItem(int selectionIndex, ICollection<TurretBlueprint> selectedTurrets)
    {
        // Grants a turret option
        WeightedList<TurretBlueprint> turrets = _shopData.Turrets.ToWeightedList(GameStats.Rounds);
        TurretBlueprint selected = turrets.GetRandomItem(duplicateType: _shopData.TurretDuplicateCheck,
            previousPicks: selectedTurrets.Take(selectionIndex).ToArray(), rng: Shop.Random);

        if (ShouldHide(selectionIndex))
            GenerateHiddenUI(selected, selectionIndex);
        else
            GenerateTurretUI(selected);

        return selected;
    }
        
    private ModuleChainHandler GenerateModuleItem(int selectionIndex, ICollection<ModuleChainHandler> selectedModules)
    { 
        WeightedList<ModuleChainHandler> modules = _shopData.ModuleHandlers.ToWeightedList(GameStats.Rounds);

        // Only show modules that can be equipped on a turret the player has (or had)
        for (var i = 0; i < modules.Count; i++)
        {
            Type[] validTypes = modules[i].Item.GetModule().GetValidTypes();
            if (validTypes.Any(x => _turretTypes.Contains(x))) continue;
                
            modules.RemoveAt(i);
            i--;
        }
            
        ModuleChainHandler selected = modules.GetRandomItem(duplicateType: _shopData.ModuleDuplicateCheck,
            previousPicks: selectedModules.Take(selectionIndex).ToArray(), rng: Shop.Random);

        if (ShouldHide(selectionIndex))
            GenerateHiddenUI(selected, selectionIndex);
        else
            GenerateModuleUI(selected);

        return selected;
    }

    private GodotObject GenerateLifeItem()
    {
        // Create the ui as a child
        var lifeUI = _lifeSelectionUI.Instantiate<LifeSelectionUI>();
        AddChild(lifeUI);
        lifeUI.Name = "_" + lifeUI.Name;
        lifeUI.Init(_shopData.LifeCount, _shop);
        return lifeUI;
    }
    
    /// <summary>
    /// Adds a new Module UI option to the player's choice
    /// </summary>
    /// <param name="handler">The Module the player can pick</param>
    private GodotObject GenerateModuleUI(ModuleChainHandler handler)
    {
        // Create the ui as a child
        var moduleUI = _moduleSelectionUI.Instantiate<ModuleSelectionUI>();
        AddChild(moduleUI);
        moduleUI.Name = "_" + moduleUI.Name;
        moduleUI.Init(handler, _shop);
        return moduleUI;
    }
    
    /// <summary>
    /// Adds a new turret UI option to the player's choice
    /// </summary>
    /// <param name="turret">The turret the player can pick</param>
    private GodotObject GenerateTurretUI(TurretBlueprint turret)
    {
        // TODO - Does return correct type?
        turret.glyph = _shop.GlyphsLookup.GetForType(turret.prefab.GetType());
        var turretUI = _turretSelectionUI.Instantiate<TurretSelectionUI>();
        AddChild(turretUI);
        turretUI.Name = "_" + turretUI.Name;
        turretUI.Init(turret, _shop);
        return turretUI;
    }

    private void GenerateHiddenUI(Object choice, int selectionIndex)
    {
        _hiddenChoices.Add(new Tuple<Object, int>(choice, selectionIndex));
        Node hiddenUI = _hiddenSelectionUI.Instantiate();
        AddChild(hiddenUI);
        hiddenUI.Name = "_" + hiddenUI.Name;
    }

    private bool ShouldHide(int selectionIndex)
    {
        return _shopData.HiddenMode switch
        {
            HiddenMode.Disabled => false,
            HiddenMode.Count => _shopData.SelectionChoices - (selectionIndex + 1) < _shopData.HiddenChoices,
            HiddenMode.Chance => Shop.Random.Next() < _shopData.HiddenChance,
            _ => throw new Exception("Invalid hidden mode")
        };
    }

    private void CheckCategories(int selectionCount)
    {
        try
        {
            if (_shopData.TurretOptionWeight.Sample(GameStats.Rounds) < 0)
                _shopData.Turrets.ToWeightedList(GameStats.Rounds)
                    .GetRandomItems(selectionCount, _shopData.TurretDuplicateCheck);
        }
        catch (NullReferenceException)
        {
            GD.PushWarning("Shop may not have enough turrets to pick from at wave " + GameStats.Rounds);
        }
        try
        {
            if (_shopData.ModuleOptionWeight.Sample(GameStats.Rounds) < 0)
                _shopData.ModuleHandlers.ToWeightedList(GameStats.Rounds)
                    .GetRandomItems(selectionCount, _shopData.ModuleDuplicateCheck);
        }
        catch (NullReferenceException)
        {
            GD.PushWarning("Shop may not have enough modules to pick from at wave " + GameStats.Rounds);
        }
    }
        
    /// <summary>
    /// Adds a turret type to the selected type.
    /// Makes sure we have a full list of turret the player has purchased
    /// so we can display only ones they can use
    /// </summary>
    /// <param name="type">The type of the turret to add</param>
    public void AddTurretType(Type type)
    {
        if (!_turretTypes.Contains(type))
            _turretTypes.Add(type);
    }
        
    public void Open()
    {
        _openTimeScale = Engine.TimeScale;
        Engine.TimeScale = 0f;
        Visible = true;
    }

    public void Resume()
    {
        Engine.TimeScale = _openTimeScale;
        Visible = false;
    }

    public void Lock()
    {
        for (var i = 0; i < _hiddenChoices.Count; i++)
        {
            GetChild(_hiddenChoices[i].Item2 + i).QueueFree();
            GodotObject shownItem;
            if (_hiddenChoices[i].Item1.GetType() == typeof(TurretBlueprint))
            {
                shownItem = GenerateTurretUI((TurretBlueprint)_hiddenChoices[i].Item1);
            }
            else if (_hiddenChoices[i].Item1 is ModuleChainHandler)
            {
                shownItem = GenerateModuleUI((ModuleChainHandler)_hiddenChoices[i].Item1);
            }
            else
            {
                shownItem = GenerateLifeItem();
            }
            // TODO - SetSiblingIndex
            // shownItem.transform.SetSiblingIndex(_hiddenChoices[i].Item2);
        }
            
        _isLocked = true;
        _lockButton.Visible = false;
        _lockedButton.Visible = true;
    }

    public void Unlock()
    {
        _isLocked = false;
        _lockButton.Visible = true;
        _lockedButton.Visible = false;
    }
}