using System.Collections.Generic;
using Turrets.Blueprints;
using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = "LevelName", menuName = "LevelData", order = 2)]
    public class LevelData : ScriptableObject
    {
        [Header("Turrets")]
        public List<TurretBlueprint> turrets = new List<TurretBlueprint>();
        [Header("Upgrades")]
        public List<Upgrade> upgrades = new List<Upgrade>();
    }
}
