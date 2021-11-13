using System.Collections.Generic;
using Turrets.Blueprints;
using Turrets.Upgrades;
using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = "LevelName", menuName = "LevelData", order = 2)]
    public class LevelData : ScriptableObject
    {
        [Header("Selection")]
        public WeightedList<TurretBlueprint> turrets;
        public WeightedList<Upgrade> upgrades;
        public int initialSelectionCost;
        public int selectionCostIncrement;
    }
}
