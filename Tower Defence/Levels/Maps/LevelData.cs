using System.ComponentModel.DataAnnotations;
using Abstract.Data;
using Godot;
using Turrets;
using UI.Shop;

namespace Levels.Maps;

/// <summary>
/// Allows us to save data to allow levels to be different
/// </summary>
[GlobalClass]
[Tool]
public partial class LevelData : Resource
{
    /// <summary>
    /// The chances for turrets in the initial selection(s)
    /// </summary>
    [ExportGroup("InitialSelection")]
    [Export]
    public WeightedList<TurretBlueprint> InitialTurretSelection = new();
        
    /// <summary>
    /// The duplicate check to perform when generating the initial selection
    /// </summary>
    [Export]
    public DuplicateTypes InitialDuplicateCheck = DuplicateTypes.None;

    /// <summary>
    /// The weighted chance of a turret card to appear in the selection
    /// </summary>
    [ExportGroup("Selection")]
    [Export]
    public Curve TurretOptionWeight;
    /// <summary>
    /// The weighted chance of a module card to appear in the selection
    /// </summary>
    [Export]
    public Curve ModuleOptionWeight;
    /// <summary>
    /// The weighted chance of additional lives to appear in the selection
    /// </summary>
    [Export]
    public Curve LifeOptionWeight;
    /// <summary>
    /// How many lives a life card will grant
    /// </summary>
    [Export]
    public int LifeCount;

    /// <summary>
    /// What turrets can appear and their individual chances
    /// </summary>
    [Export]
    public WeightedCurveList<TurretBlueprint> Turrets = new();
    /// <summary>
    /// The duplicate check to perform when generating a turret card
    /// </summary>
    [Export]
    public DuplicateTypes TurretDuplicateCheck = DuplicateTypes.ByName;
    /// <summary>
    /// What modules can appear and their individual chances
    /// </summary>
    [Export]
    public WeightedCurveList<ModuleChainHandler> ModuleHandlers = new();
    /// <summary>
    /// The duplicate check to perform when generating a module card
    /// </summary>
    [Export]
    public DuplicateTypes ModuleDuplicateCheck = DuplicateTypes.ByType;

    /// <summary>
    /// How initial selections should be granted
    /// </summary>
    [ExportGroup("Selection Counts")]
    [Export]
    public int InitialSelectionCount = 1;
    /// <summary>
    /// How many options to show in the initial selection(s)
    /// </summary>
    [Export]
    public int InitialChoices = 3;
    /// <summary>
    /// How many options to display in non-initial selections
    /// </summary>
    [Export]
    public int SelectionChoices = 3;
    /// <summary>
    /// How cards are selected to be hidden
    /// </summary>
    [Export]
    public HiddenMode HiddenMode = HiddenMode.Disabled;
    /// <summary>
    /// How many cards to hide
    /// </summary>
    [Export]
    public int HiddenChoices = 0;
    /// <summary>
    /// The for a card to be hidden
    /// </summary>
    [Export]
    [Range(0f, 1f)]
    public float HiddenChance = 0;
        
    /// <summary>
    /// How costs scale per wave.
    ///
    /// Must be a valid Godot Expression where "x" is the wave number
    /// </summary>
    [ExportGroup("Costs")]
    [Export]
    public string SelectionCostFormula;
        
    /// <summary>
    /// Lives to reroll ration for the selection
    ///
    /// Use an integer to take 1+ hearts per reroll
    /// Use a decimal to grant extra free rerolls per purchase
    /// </summary>
    [Export]
    public float RerollCost;
        
    /// <summary>
    /// What percentage of the shop cost to refund when selling
    /// </summary>
    [Export]
    public float SellPercentage;
}