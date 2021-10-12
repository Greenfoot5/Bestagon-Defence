using System.Collections.Generic;
using Turrets.Upgrades.BulletUpgrades;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;

namespace Turrets.Blueprints
{
    [CreateAssetMenu(fileName = "TurretBlueprint", menuName = "TurretBlueprint", order = 0)]
    public class TurretBlueprint : ScriptableObject
    {
        [Header("Shop Info")]
        public Sprite shopIcon;
        public int cost;
        public string displayName;
        public string turretType;
        
        [Header("Turret Info")]
        [Tooltip("The prefab to use when the turret is built.")]
        public GameObject prefab;
        // Preset upgrades
        public List<TurretUpgrade> turretUpgrades = new List<TurretUpgrade>();
        public List<BulletUpgrade> bulletUpgrades = new List<BulletUpgrade>();
        
        [Header("Misc")]
        [Range(0, 1)]
        [Tooltip("The percentage reduction to apply to the price when selling")]
        public float sellReduction = 0.5f;

        public int GetSellAmount()
        {
            // Implement an increase if upgraded.
            return (int) (cost * sellReduction);
        }
    }
}
