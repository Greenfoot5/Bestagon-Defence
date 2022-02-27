﻿using System.Collections.Generic;
using Modules;
using UnityEngine;

namespace Turrets
{
    /// <summary>
    /// Allows us to save turret data in an so without creating a prefab per turret
    /// </summary>
    [CreateAssetMenu(fileName = "TurretBlueprint", menuName = "Turret Blueprint", order = 0)]
    public class TurretBlueprint : ScriptableObject
    {
        [Header("Shop Info")]
        [Tooltip("The icon that appears on the selection card")]
        public Sprite shopIcon;
        [Tooltip("The turret name that appears on the selection card")]
        public string displayName;
        [Tooltip("The tagline of the turret. It's not a description, just a witty little remark")]
        public string tagline;
        
        [Tooltip("The main colour of the turret. Is used in various ways for display")]
        public Color accent;
        
        [Header("Turret Info")]
        [Tooltip("The prefab to use when the turret is built.")]
        public GameObject prefab;
        [Tooltip("Any modules that come pre-applied when the player places the turret")]
        public List<Module> modules = new List<Module>();
    }
}
