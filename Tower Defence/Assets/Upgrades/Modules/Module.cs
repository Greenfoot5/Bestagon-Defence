using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Turrets.Modules
{
    public abstract class Module : ScriptableObject
    {
        [SerializeField]
        private string ModuleType;

        public int ModuleTier;

        public Color accentColor;
        public string displayName;
        public string tagline;
        public Sprite icon;
        [Multiline]
        public string effectText;

        protected abstract Type[] ValidTypes { get; }

        public bool ValidModule(Turret turret)
        {
            return ValidTypes == null || ValidTypes.Contains(turret.GetType());
        }

        public Type[] GetValidTypes()
        {
            return ValidTypes;
        }

        public abstract void AddModule(Turret turret);

        public abstract void RemoveModule(Turret turret);

        public abstract void OnShoot(Bullet bullet);

        public abstract void OnHit(IEnumerable<Enemy> targets);

        public string GetModuleType()
        {
            return ModuleType;
        }
    }
}