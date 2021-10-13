using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    public abstract class BulletUpgrade : Upgrade
    {
        public abstract void OnShoot(Bullet bullet);

        public abstract void OnHit(Enemy target);

        public bool ValidUpgrade(ref Turret turret)
        {
            return true;
        }
    }
}