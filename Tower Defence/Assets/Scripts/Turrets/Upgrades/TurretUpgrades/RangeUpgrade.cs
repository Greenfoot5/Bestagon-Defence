using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    [CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Upgrades/RangeUpgrade")]
    public class RangeUpgrade : Upgrade
    {
        public override void AddUpgrade(Turret turret)
        {
            turret.range *= 1 + GETUpgradeValue();
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 + GETUpgradeValue();
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(Enemy[] targets) { }
    }
}
