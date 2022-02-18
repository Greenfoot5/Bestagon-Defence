using Abstract.Data;
using Modules;
using Turrets;
using UnityEngine;

namespace Levels.Maps
{
    /// <summary>
    /// A way to disable duplicates in random selections
    /// </summary>
    public enum DuplicateTypes
    {
        None,
        ByName,
        ByType
    }
    
    /// <summary>
    /// Allows us to save data to allow levels to be different
    /// </summary>
    [CreateAssetMenu(fileName = "LevelName", menuName = "Level Data", order = 2)]
    public class LevelData : ScriptableObject
    {
        [Header("InitialSelection")]
        [Tooltip("These chances overwrite base chances, they are only present of the player's first selection")]
        public WeightedList<TurretBlueprint> initialTurretSelection;
        [Tooltip("What type of duplicate check to perform, if any, against the rest of the selection")]
        public DuplicateTypes initialDuplicateCheck = DuplicateTypes.None;

        [Header("Selection")]
        [Tooltip("What turrets can appear and their weighted chance of appearing on a turret selection card")]
        public WeightedList<TurretBlueprint> turrets;
        [Tooltip("What type of duplicate check to perform, if any, against the rest of the selection")]
        public DuplicateTypes turretDuplicateCheck = DuplicateTypes.ByName;
        [Tooltip("The weighted chance of picking a turret card to appear in the selection")]
        public float turretOptionWeight = 1f;
        [Tooltip("What module can appear and their weighted chance of appearing on a module selection card")]
        public WeightedList<Module> modules;
        [Tooltip("What type of duplicate check to perform, if any, against the rest of the selection")]
        public DuplicateTypes moduleDuplicateCheck = DuplicateTypes.ByType;
        [Tooltip("The weighted chance of picking a module card to appear in the selection")]
        public float moduleOptionWeight = 1f;
        
        [Header("Costs")]
        [Tooltip("The initial cost to open the selection")]
        public int initialSelectionCost;
        [Tooltip("The amount to increase the selection cost by every time it's is opened")]
        public int selectionCostIncrement;

        [Header("Wave Scaling")]
        [Tooltip("What to multiply the enemy health by for every wave")]
        public float health = 1f;
        [Tooltip("What to multiply the enemy count by for every wave")]
        public float enemyCount = 1f;
    }
}
