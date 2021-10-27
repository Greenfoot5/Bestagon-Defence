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
        public string displayName;
        public string turretType;
        
        [Header("Turret Info")]
        [Tooltip("The prefab to use when the turret is built.")]
        public GameObject prefab;
        // Preset upgrades
        public List<Upgrade> upgrades = new List<Upgrade>();
    }
}
