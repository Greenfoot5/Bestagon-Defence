using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "BulletDamageT0", menuName = "Upgrades/BulletUpgrade/BulletDamage", order = 2)]
    public class DamageUpgrade : BulletUpgrade
    {
        public void AlterBulletSettings(ref Bullet bullet)
        {
            throw new System.NotImplementedException();
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.AddUpgrade(this);
            bullet.damage += (int) GETUpgradeValue();
        }

        public override void OnHit(Enemy target)
        {
            return;
        }

        public new bool ValidUpgrade(ref Turret turret)
        {
            return turret.attackType == TurretType.Bullet;
        }
    }
}