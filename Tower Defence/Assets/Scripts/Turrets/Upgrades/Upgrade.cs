using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Turrets.Upgrades
{
    public abstract class Upgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeType;

        [Range(0f, 1f)]
        [SerializeField]
        private int upgradeTier;
        
        // TODO - Generate display name from update type and tier
        public string displayName;
        public Sprite icon;
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
    }
}