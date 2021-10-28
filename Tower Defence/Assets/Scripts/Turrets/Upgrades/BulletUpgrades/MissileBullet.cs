using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/MissileBullet")]
    public class MissileBullet : Upgrade
    {
        public override void AddUpgrade(Turret turret)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUpgrade(Turret turret)
        {
            throw new System.NotImplementedException();
        }

        public override void OnShoot(Bullet bullet)
        {;
            bullet.explosionRadius += GETUpgradeValue();
        }

        public override void OnHit(Enemy[] targets) { }
    }
}