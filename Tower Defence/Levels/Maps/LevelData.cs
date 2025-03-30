using System.ComponentModel.DataAnnotations;
using Abstract.Data;
using Godot;
using Turrets;
using UI.Shop;

namespace Levels.Maps
{
    /// <summary>
    /// Allows us to save data to allow levels to be different
    /// </summary>
    [GlobalClass]
    public partial class LevelData : Resource
    {
        /// <summary>
        /// The chances for turrets in the initial selection(s)
        /// </summary>
        [ExportGroup("InitialSelection")]
        // [Export]
        // TODO - Work out how to export
        public WeightedList<TurretBlueprint> initialTurretSelection;
        /// <summary>
        /// The duplicate check to perform when generating the initial selection
        /// </summary>
        [Export]
        public DuplicateTypes initialDuplicateCheck = DuplicateTypes.None;

        /// <summary>
        /// The weighted chance of a turret card to appear in the selection
        /// </summary>
        [ExportGroup("Selection")]
        [Export]
        public CurvedReference turretOptionWeight;
        /// <summary>
        /// The weighted chance of a module card to appear in the selection
        /// </summary>
        [Export]
        public CurvedReference moduleOptionWeight;
        /// <summary>
        /// The weighted chance of additional lives to appear in the selection
        /// </summary>
        [Export]
        public CurvedReference lifeOptionWeight;
        /// <summary>
        /// How many lives a life card will grant
        /// </summary>
        [Export]
        public int lifeCount;
        /// <summary>
        /// What turrets can appear and their individual chances
        /// </summary>
        // [Export]
        public WeightedCurveList<TurretBlueprint> turrets;
        /// <summary>
        /// The duplicate check to perform when generating a turret card
        /// </summary>
        [Export]
        public DuplicateTypes turretDuplicateCheck = DuplicateTypes.ByName;
        /// <summary>
        /// What modules can appear and their individual chances
        /// </summary>
        // [Export]
        public WeightedCurveList<ModuleChainHandler> moduleHandlers;
        /// <summary>
        /// The duplicate check to perform when generating a module card
        /// </summary>
        [Export]
        public DuplicateTypes moduleDuplicateCheck = DuplicateTypes.ByType;

        /// <summary>
        /// How initial selections should be granted
        /// </summary>
        [ExportGroup("Selection Counts")]
        [Export]
        public int initialSelectionCount = 1;
        /// <summary>
        /// How many options to show in the initial selection(s)
        /// </summary>
        [Export]
        public int initialChoices = 3;
        /// <summary>
        /// How many options to display in non-initial selections
        /// </summary>
        [Export]
        public int selectionChoices = 3;
        /// <summary>
        /// How cards are selected to be hidden
        /// </summary>
        [Export]
        public HiddenMode hiddenMode = HiddenMode.Disabled;
        /// <summary>
        /// How many cards to hide
        /// </summary>
        [Export]
        public int hiddenChoices = 0;
        /// <summary>
        /// The for a card to be hidden
        /// </summary>
        [Export]
        [Range(0f, 1f)]
        public float hiddenChance = 0;
        
        /// <summary>
        /// How costs scale per wave.
        ///
        /// Must be a valid Godot Expression where "x" is the wave number
        /// </summary>
        [ExportGroup("Costs")]
        [Export]
        public string selectionCostFormula;
        
        /// <summary>
        /// Wave based multiplier for health
        /// </summary>
        [ExportGroup("Wave Scaling")]
        [Export]
        public CurvedReference health;
        /// <summary>
        /// Wave based multiplier for count
        /// </summary>
        [Export]
        public CurvedReference enemyCount;
        
        /// <summary>
        /// Lives to reroll ration for the selection
        ///
        /// Use an integer to take 1+ hearts per reroll
        /// Use a decimal to grant extra free rerolls per purchase
        /// </summary>
        [Export]
        public float rerollCost;
        
        /// <summary>
        /// What percentage of the shop cost to refund when selling
        /// </summary>
        [Export]
        public float sellPercentage;
    }
}
