using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    [CreateAssetMenu(fileName = "RangeUpgrade", menuName = "Upgrades/TurretUpgrade/RangeUpgrade", order = 2)]
    public class RangeUpgrade : TurretUpgrade
    {
        public override Turret AddUpgrade(Turret turret)
        {
            turret.range *= 1 + GETUpgradeValue();
            return turret;
        }

        public override Turret RemoveUpgrade(Turret turret)
        {
            turret.fireRate /= 1 + GETUpgradeValue();
            return turret;
        }

        public override bool ValidUpgrade(Turret turret)
        {
            return turret.attackType == TurretType.Bullet || turret.attackType == TurretType.Area;
        }
    }
}
