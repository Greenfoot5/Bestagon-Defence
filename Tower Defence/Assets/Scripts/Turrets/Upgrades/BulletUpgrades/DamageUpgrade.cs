using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "BulletDamageT0", menuName = "Upgrades/BulletDamage")]
    public class DamageUpgrade : Upgrade
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
        {
            bullet.damage += (int) (GETUpgradeValue() * bullet.damage);
        }

        public override void OnHit(Enemy[] targets) { }
    }
}