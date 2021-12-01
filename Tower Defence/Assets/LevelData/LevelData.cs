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
        public List<TurretBlueprint> turrets = new List<TurretBlueprint>();
        public List<Upgrade> upgrades = new List<Upgrade>();
        public int initialSelectionCost;
        public int selectionCostIncrement;

        [Header("Wave Scaling")]
        public float health = 1f;
        public float enemyCount = 1f;
    }
}
