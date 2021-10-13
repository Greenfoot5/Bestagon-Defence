using System.Linq;
using UnityEngine;

namespace Turrets.Upgrades.TurretUpgrades
{
    public abstract class TurretUpgrade : Upgrade
    {
        public bool isEvolution;

        public abstract void AddUpgrade(Turret turret);

        public abstract Turret RemoveUpgrade(Turret turret);

        public virtual bool ValidUpgrade(Turret turret)
        {
            return true;
            // Make sure the turret doesn't already have an evolution (as they will currently conflict)
            return turret.turretUpgrades.All(upgrade => !upgrade.isEvolution);
        }
    }
}