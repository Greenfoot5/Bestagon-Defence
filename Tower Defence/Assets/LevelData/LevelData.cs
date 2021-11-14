using System.ComponentModel;
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
    [CreateAssetMenu(fileName = "LevelName", menuName = "LevelData", order = 2)]
    public class LevelData : ScriptableObject
    {
        [Header("InitialSelection")]
        [Tooltip("These chances overwrite base chances")]
        public WeightedList<TurretBlueprint> initialTurretSelection;
        public DuplicateTypes initialDuplicateCheck = DuplicateTypes.None;
        
        [Header("Selection")]
        public WeightedList<TurretBlueprint> turrets;
        public DuplicateTypes turretDuplicateCheck = DuplicateTypes.ByName;
        public WeightedList<Upgrade> upgrades;
        public DuplicateTypes upgradeDuplicateCheck = DuplicateTypes.ByType;
        
        [Header("Costs")]
        public int initialSelectionCost;
        public int selectionCostIncrement;
    }
}
