using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    public abstract class TurretUpgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeType;

        [Range(0f, 1f)]
        [SerializeField]
        private float effectPercentage;
        [SerializeField]
        private int upgradeTier;
        
        // TODO - Generate display name from update type and tier
        public string displayName;
        public Sprite icon;
        public string effectText;
        public string[] restrictionsText;

        public string GETUpgradeType()
        {
            return upgradeType;
        }

        public float GETUpgradeValue()
        {
            return effectPercentage;
        }

        public int GETUpgradeTier()
        {
            return upgradeTier;
        }

        public abstract Turret AddUpgrade(Turret turret);

        public abstract Turret RemoveUpgrade(Turret turret);

        public abstract bool ValidUpgrade(Turret turret);
    }
}