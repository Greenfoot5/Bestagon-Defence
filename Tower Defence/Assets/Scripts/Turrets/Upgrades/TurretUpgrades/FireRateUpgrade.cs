using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/TurretUpgrade/FireRateUpgrade", order = 1)]
    public class FireRateUpgrade : TurretUpgrade
    {
        public override Turret AddUpgrade(Turret turret)
        {
            turret.fireRate *= 1 - GETUpgradeValue();
            return turret;
        }

        public override Turret RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 - GETUpgradeValue();
            return turret;
        }

        public override bool ValidUpgrade(Turret turret)
        {
            return turret.attackType == TurretType.Bullet || turret.attackType == TurretType.Area;
        }
    }
}
