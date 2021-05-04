using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    public abstract class BulletUpgrade : ScriptableObject
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
        
        public void AlterBulletSettings(ref Bullet bullet)
        {
        }

        public bool ValidUpgrade(ref Turret turret)
        {
            return true;
        }
    }
}