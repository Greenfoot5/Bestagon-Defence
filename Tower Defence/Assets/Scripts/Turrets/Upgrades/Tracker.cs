using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "TrackerUpgradeT0", menuName = "Upgrades/Tracker")]
    public class Tracker : Upgrade
    {
        [SerializeField]    
        private float turnSpeed;
        public override void AddUpgrade(Turret turret)
        {
            ((DynamicTurret)turret).turnSpeed *= 1 + turnSpeed;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            ((DynamicTurret)turret).turnSpeed /= 1 + turnSpeed;
        }

        public override void OnShoot(Bullet bullet) { }
        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}
