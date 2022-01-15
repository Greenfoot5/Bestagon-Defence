using System;
using System.Collections.Generic;
using Turrets.Upgrades;
using UnityEngine;

namespace Turrets.Blueprints
{
    [CreateAssetMenu(fileName = "TurretBlueprint", menuName = "Turret Blueprint", order = 0)]
    public class TurretBlueprint : ScriptableObject
    {
        [Header("Shop Info")]
        public Sprite shopIcon;
        public string displayName;
        public string tagline;

        public Color accent;
        
        [Header("Turret Info")]
        [Tooltip("The prefab to use when the turret is built.")]
        public GameObject prefab;
        // Preset upgrades
        public List<Upgrade> upgrades = new List<Upgrade>();
    }
}
