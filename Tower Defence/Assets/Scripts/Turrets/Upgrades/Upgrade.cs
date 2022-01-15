using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

namespace Turrets.Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeType;

        public int upgradeTier;

        public Color accentColor;
        public string displayName;
        public string tagline;
        public Sprite icon;
        [Multiline]
        public string effectText;

        protected abstract Type[] ValidTypes { get; }

        public bool ValidUpgrade(Turret turret)
        {
            return ValidTypes == null || ValidTypes.Contains(turret.GetType());
        }

        public Type[] GetValidTypes()
        {
            return ValidTypes;
        }

        public abstract void AddUpgrade(Turret turret);

        public abstract void RemoveUpgrade(Turret turret);

        public abstract void OnShoot(Bullet bullet);

        public abstract void OnHit(IEnumerable<Enemy> targets);

        public string GetUpgradeType()
        {
            return upgradeType;
        }
    }
}