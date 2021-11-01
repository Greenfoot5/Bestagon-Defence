using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Turrets.Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeType;
        [SerializeField]
        private int upgradeTier;

        public Color accentColor;
        public string displayName;
        public string tagline;
        public Sprite icon;
        [Multiline]
        public string effectText;
        public TurretType[] validTypes;

        public bool ValidUpgrade(Turret turret)
        {
            return validTypes.Contains(turret.attackType);
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