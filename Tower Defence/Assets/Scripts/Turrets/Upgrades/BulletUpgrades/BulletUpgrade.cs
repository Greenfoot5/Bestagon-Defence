using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    public abstract class BulletUpgrade : ScriptableObject
    {
        private string upgradeType = "Fire Rate Reduction";
        
        [SerializeField]
        private int upgradeTier;
        
        public string GETUpgradeType()
        {
            return upgradeType;
        }

        public int GETUpgradeTier()
        {
            return upgradeTier;
        }

        public abstract Bullet OnShoot(Bullet bullet);

        public abstract void OnHit(Enemy target);

        public bool ValidUpgrade(ref Turret turret)
        {
            return true;
        }
    }
}