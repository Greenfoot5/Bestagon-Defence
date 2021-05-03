using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    public abstract class TurretUpgrade : ScriptableObject
    {
        private string upgradeType = "Fire Rate Reduction";

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

        void AlterTurretStat(ref Turret turret)
        {
        }

        bool ValidUpgrade(ref Turret turret)
        {
            return true;
        }
    }
}