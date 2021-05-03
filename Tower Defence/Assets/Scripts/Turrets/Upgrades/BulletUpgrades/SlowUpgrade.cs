using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Upgrades/BulletUpgrade/SlowsEnemyUpgrade", order = 1)]
    public class SlowUpgrade : BulletUpgrade
    {
        public void AlterBulletSettings(ref Bullet bullet)
        {
            throw new System.NotImplementedException();
        }

        public bool ValidUpgrade(ref Turret turret)
        {
            return turret.attackType == TurretType.Bullet;
        }
    }
}
