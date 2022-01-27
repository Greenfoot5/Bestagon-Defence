using System.Collections.Generic;
using Turrets.Modules;
using UnityEngine;

namespace Turrets.Blueprints
{
    /// <summary>
    /// Allows us to save turret data in an so without creating a prefab per turret
    /// </summary>
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
        // Preset Modules
        public List<Module> modules = new List<Module>();
    }
}
