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

        public override Bullet OnShoot(Bullet bullet)
        {
            Debug.Log("OhShoot1 " + bullet.damage);
            //bullet.AddUpgrade(this);
            bullet.damage += (int) (GETUpgradeValue() * bullet.damage);
            Debug.Log("OhShoot2 " + bullet.damage);
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