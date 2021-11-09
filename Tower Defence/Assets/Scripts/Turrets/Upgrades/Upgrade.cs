using System;
using System.Collections.Generic;
using System.Linq;
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

        public abstract Type[] ValidTypes { get; }

        public bool ValidUpgrade(Turret turret)
        {
            if (ValidTypes == null) return true;
            return ValidTypes.Contains(turret.GetType());
        }

        public abstract void AddUpgrade(Turret turret);

        public abstract void RemoveUpgrade(Turret turret);

        public abstract void OnShoot(Bullet bullet);

        public abstract void OnHit(IEnumerable<Enemy> targets);

        public string GETUpgradeType()
        {
            return upgradeType;
        }
    }
}