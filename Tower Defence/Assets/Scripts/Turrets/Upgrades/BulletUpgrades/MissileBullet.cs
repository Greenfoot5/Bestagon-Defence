using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/BulletUpgrade/MissileBullet", order = 3)]
    public class MissileBullet : BulletUpgrade
    {
        public void AlterBulletSettings(ref Bullet bullet)
        {
            throw new System.NotImplementedException();
        }

        public override Bullet OnShoot(Bullet bullet)
        {;
            bullet.explosionRadius += GETUpgradeValue();
            return bullet;
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