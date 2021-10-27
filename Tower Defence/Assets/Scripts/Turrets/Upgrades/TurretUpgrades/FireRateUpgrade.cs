using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRateUpgrade")]
    public class FireRateUpgrade : Upgrade
    {
        public override void AddUpgrade(Turret turret)
        {
            turret.fireRate *= 1 - GETUpgradeValue();
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 - GETUpgradeValue();
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(Enemy[] targets) { }

        public override bool ValidUpgrade(Turret turret)
        {
            return turret.attackType == TurretType.Bullet || turret.attackType == TurretType.Area;
        }
    }
}
