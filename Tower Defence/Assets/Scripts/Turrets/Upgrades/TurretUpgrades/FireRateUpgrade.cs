using System;
using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/TurretUpgrade/FireRateUpgrade", order = 1)]
    public class FireRateUpgrade : TurretUpgrade
    {
        public void AlterTurretStat (ref Turret turret)
        {
            turret.fireRate *= 1 - GETUpgradeValue();
        }

        public bool ValidUpgrade(ref Turret turret)
        {
            return turret.attackType == TurretType.Bullet || turret.attackType == TurretType.Area;
        }
    }
}
