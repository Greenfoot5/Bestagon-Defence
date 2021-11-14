using Turrets.Blueprints;
using Turrets.Upgrades;
using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = "LevelName", menuName = "LevelData", order = 2)]
    public class LevelData : ScriptableObject
    {
        [Header("InitialSelection")]
        [Tooltip("These chances overwrite base chances")]
        public WeightedList<TurretBlueprint> initialTurretSelection;
        [Header("Selection")]
        public WeightedList<TurretBlueprint> turrets;
        public WeightedList<Upgrade> upgrades;
        [Header("Costs")]
        public int initialSelectionCost;
        public int selectionCostIncrement;
    }
}
