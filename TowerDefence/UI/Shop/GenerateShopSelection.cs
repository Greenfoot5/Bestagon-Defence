using System;
using System.Collections.Generic;
using System.Linq;
using BestagonDefence.Abstract.Data;
using BestagonDefence.Gameplay;
using BestagonDefence.Levels.Maps;
using BestagonDefence.Turrets;
using Godot;

namespace BestagonDefence.UI.Shop;

public partial class GenerateShopSelection : Control
{
    /// <summary>
    /// The PackedScene for a turret selection card
    /// </summary>
    [Export]
    private PackedScene _turretSelectionUI;
    /// <summary>
    /// The PackedScene for a module selection card
    /// </summary>
    [Export]
    private PackedScene _moduleSelectionUI;
    /// <summary>
    /// The PackedScene for a life selection card
    /// </summary>
    [Export]
    private PackedScene _lifeSelectionUI;
    /// <summary>
    /// The PackedScene for a hidden selection card
    /// </summary>
    [Export]
    private PackedScene _hiddenSelectionUI;
    private ShopData _shopData;
    /// <summary>
    /// The Shop in the level
    /// </summary>
    [Export]
    private Shop _shop;

    /// <summary>
    /// The parent node to display the selection under
    /// </summary>
    [Export]
    private Node _selectionParent;

    /// <summary>
    /// The turrets already purchased
    /// </summary>
    private readonly List<Type> _turretTypes = [typeof(Turret)];

    /// <summary>
    /// The button to show when options aren't locked
    /// </summary>
    [Export]
    private BaseButton _lockButton;
    /// <summary>
    /// The button to show when options are locked
    /// </summary>
    [Export]
    private BaseButton _lockedButton;
    private bool _isLocked;

    private List<Tuple<object, int>> _hiddenChoices;
    private double _openTimeScale;
    
    /// <summary>
    /// Setups references, checks the player has enough gold and freezes the game when enabled
    /// </summary>
    public override void _Ready()
    {
        _shopData = _shop.ShopData;
        GenerateSelection();
    }
        
    /// <summary>
    /// Generates the selection of the shop
    /// </summary>
    public void GenerateSelection()
    {
        if (_isLocked) return;

        Shop.OldState = Shop.Random.GetState();
        if (_shopData.HiddenMode != HiddenMode.Disabled)
            _hiddenChoices = [];

        // Destroy the previous selection
        for (int i = _selectionParent.GetChildCount() - 1; i >= 0; i--)
        {
            _selectionParent.GetChild(i).QueueFree();
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

    /// <summary>
    /// Creates an initial turret option card
    /// </summary>
    /// <param name="selectionIndex">The item index in the options</param>
    /// <param name="selectedTurrets">Which turret options have already been created</param>
    /// <returns>The Blueprint of the created card</returns>
    private TurretBlueprint GenerateInitialItem(int selectionIndex, ICollection<TurretBlueprint> selectedTurrets)
    {
        // Grants a turret option
        var turrets = new WeightedList(_shopData.InitialTurretSelection);
        turrets.RemoveUnweighted();
        TurretBlueprint selected = turrets.GetRandomItem(duplicateType: _shopData.InitialDuplicateCheck,
            previousPicks: selectedTurrets.Take(selectionIndex).ToArray(), rng: Shop.Random);
            
        // Add the turret to the ui for the player to pick
        GenerateTurretUI(selected);
            
        return selected;
    }

    /// <summary>
    /// Creates a turret option card
    /// </summary>
    /// <param name="selectionIndex">The item index in the options</param>
    /// <param name="selectedTurrets">Which turret options have already been created</param>
    /// <returns>The Blueprint of the created card</returns>
    private TurretBlueprint GenerateTurretItem(int selectionIndex, ICollection<TurretBlueprint> selectedTurrets)
    {
        // Grants a turret option
        var turrets = _shopData.Turrets.ToWeightedList(GameStats.Rounds);
        TurretBlueprint selected = turrets.GetRandomItem(duplicateType: _shopData.TurretDuplicateCheck,
            previousPicks: selectedTurrets.Take(selectionIndex).ToArray(), rng: Shop.Random);

        if (ShouldHide(selectionIndex))
            GenerateHiddenUI(selected, selectionIndex);
        else
            GenerateTurretUI(selected);

        return selected;
    }
    
    /// <summary>
    /// Creates a module option card
    /// </summary>
    /// <param name="selectionIndex">The item index in the options</param>
    /// <param name="selectedModules">Which handler options have already been created</param>
    /// <returns>The ModuleChainHandler of the created card</returns>
    private ModuleChainHandler GenerateModuleItem(int selectionIndex, ICollection<ModuleChainHandler> selectedModules)
    { 
        var modules = _shopData.ModuleHandlers.ToWeightedList(GameStats.Rounds);

        // Only show modules that can be equipped on a turret the player has (or had)
        for (var i = 0; i < modules.Count; i++)
        {
            Type[] validTypes = modules.GetHandler(i).GetModule().GetValidTypes();
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

    /// <summary>
    /// Creates a life option card
    /// </summary>
    /// <returns>The LifeSelectionUI created</returns>
    private LifeSelectionUI GenerateLifeItem()
    {
        // Create the ui as a child
        var lifeUI = _lifeSelectionUI.Instantiate<LifeSelectionUI>();
        _selectionParent.AddChild(lifeUI);
        lifeUI.Name = "_" + lifeUI.Name;
        lifeUI.Init(_shopData.LifeCount, _shop);
        return lifeUI;
    }
    
    /// <summary>
    /// Adds a new Module UI option to the player's choice
    /// </summary>
    /// <param name="handler">The Module the player can pick</param>
    private ModuleSelectionUI GenerateModuleUI(ModuleChainHandler handler)
    {
        // Create the ui as a child
        var moduleUI = _moduleSelectionUI.Instantiate<ModuleSelectionUI>();
        _selectionParent.AddChild(moduleUI);
        moduleUI.Name = "_" + moduleUI.Name;
        moduleUI.Init(handler, _shop);
        return moduleUI;
    }
    
    /// <summary>
    /// Adds a new turret UI option to the player's choice
    /// </summary>
    /// <param name="turret">The turret the player can pick</param>
    private TurretSelectionUI GenerateTurretUI(TurretBlueprint turret)
    {
        turret.Glyph = _shop.GlyphsLookup.GetForType(turret.GetSubtype());
        var turretUI = _turretSelectionUI.Instantiate<TurretSelectionUI>();
        _selectionParent.AddChild(turretUI);
        turretUI.Name = "_" + turretUI.Name;
        turretUI.Init(turret, _shop);
        return turretUI;
    }

    /// <summary>
    /// Creates a hidden option card
    /// </summary>
    /// <param name="choice">The option being hidden</param>
    /// <param name="selectionIndex">The index of the option in the selection</param>
    private void GenerateHiddenUI(object choice, int selectionIndex)
    {
        _hiddenChoices.Add(new Tuple<object, int>(choice, selectionIndex));
        Node hiddenUI = _hiddenSelectionUI.Instantiate();
        _selectionParent.AddChild(hiddenUI);
        hiddenUI.Name = "_" + hiddenUI.Name;
    }

    /// <summary>
    /// Calculates if the current index should be hidden
    /// </summary>
    /// <param name="selectionIndex">The index of the option in the selection</param>
    /// <returns>true if the option should be hidden</returns>
    /// <exception cref="Exception">If the HiddenMode is invalid</exception>
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

    /// <summary>
    /// Checks there are still options available to display in the selection
    /// </summary>
    /// <param name="selectionCount">How many options have already been created</param>
    private void CheckCategories(int selectionCount)
    {
        try
        {
            if (_shopData.TurretOptionWeight.Sample(GameStats.Rounds) < 0)
                _shopData.Turrets.ToWeightedList(GameStats.Rounds)
                    .GetRandomItems<TurretBlueprint>(selectionCount, _shopData.TurretDuplicateCheck);
        }
        catch (NullReferenceException)
        {
            GD.PushWarning("Shop may not have enough turrets to pick from at wave " + GameStats.Rounds);
        }
        try
        {
            if (_shopData.ModuleOptionWeight.Sample(GameStats.Rounds) < 0)
                _shopData.ModuleHandlers.ToWeightedList(GameStats.Rounds)
                    .GetRandomItems<ModuleChainHandler>(selectionCount, _shopData.ModuleDuplicateCheck);
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
    
    /// <summary>
    /// Opens (displays) the shop
    /// </summary>
    public void Open()
    {
        _openTimeScale = Engine.TimeScale;
        Engine.TimeScale = 0f;
        Visible = true;
    }

    /// <summary>
    /// Closes (hides) the shop
    /// </summary>
    public void Resume()
    {
        Engine.TimeScale = _openTimeScale;
        Visible = false;
    }

    /// <summary>
    /// Prevents the selection from being regenerated until one is picked
    /// Reveals hidden options
    /// </summary>
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

    /// <summary>
    /// Allows the selection to be regenerated (and locked) again
    /// </summary>
    public void Unlock()
    {
        _isLocked = false;
        _lockButton.Visible = true;
        _lockedButton.Visible = false;
    }
}