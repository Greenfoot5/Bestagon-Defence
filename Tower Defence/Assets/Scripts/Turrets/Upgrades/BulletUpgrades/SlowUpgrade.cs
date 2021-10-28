using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "SlowUpgrade", menuName = "Upgrades/SlowsEnemyUpgrade")]
    public class SlowUpgrade : Upgrade
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
            bullet.AddUpgrade(this);
        }

        public override void OnHit(Enemy[] targets)
        {
            foreach (var target in targets)
            {
                target.Slow(GETUpgradeValue());
            }
        }
    }
}
