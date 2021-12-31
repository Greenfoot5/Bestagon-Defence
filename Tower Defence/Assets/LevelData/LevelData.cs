using Turrets.Blueprints;
using Turrets.Upgrades;
using UnityEngine;

namespace LevelData
{
    public enum DuplicateTypes
    {
        None,
        ByName,
        ByType
    }
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
        public WeightedList<Upgrade> upgrades;
        public DuplicateTypes upgradeDuplicateCheck = DuplicateTypes.ByType;
        public float upgradeOptionWeight = 1f;
        
        [Header("Costs")]
        public int initialSelectionCost;
        public int selectionCostIncrement;

        [Header("Wave Scaling")]
        public float health = 1f;
        public float enemyCount = 1f;
    }
}
