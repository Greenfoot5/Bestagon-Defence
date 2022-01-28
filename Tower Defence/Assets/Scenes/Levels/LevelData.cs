using Abstract.Data;
using Turrets.Blueprints;
using UnityEngine;
using Upgrades.Modules;

namespace Scenes.Levels
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
        [Tooltip("These chances overwrite base chances")]
        public WeightedList<TurretBlueprint> initialTurretSelection;
        public DuplicateTypes initialDuplicateCheck = DuplicateTypes.None;

        [Header("Selection")]
        public WeightedList<TurretBlueprint> turrets;
        public DuplicateTypes turretDuplicateCheck = DuplicateTypes.ByName;
        public float turretOptionWeight = 1f;
        public WeightedList<Module> modules;
        public DuplicateTypes moduleDuplicateCheck = DuplicateTypes.ByType;
        public float moduleOptionWeight = 1f;
        
        [Header("Costs")]
        public int initialSelectionCost;
        public int selectionCostIncrement;

        [Header("Wave Scaling")]
        public float health = 1f;
        public float enemyCount = 1f;
    }
}
