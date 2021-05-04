using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    public abstract class TurretUpgrade : ScriptableObject
    {
        [SerializeField]
        private string upgradeType;

        [Range(0f, 1f)]
        [SerializeField]
        private float fireRateReduction;
        [SerializeField]
        private int upgradeTier;
        
        public string GETUpgradeType()
        {
            return upgradeType;
        }

        public float GETUpgradeValue()
        {
            return fireRateReduction;
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