using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "MissileBulletT0", menuName = "Upgrades/MissileBullet")]
    public class MissileBullet : Upgrade
    {
        [SerializeField]
        private float explosionRadius;
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {;
            bullet.explosionRadius += explosionRadius;
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}