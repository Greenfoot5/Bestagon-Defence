using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades.BulletUpgrades
{
    [CreateAssetMenu(fileName = "BulletDamageT0", menuName = "Upgrades/BulletDamage")]
    public class DamageUpgrade : Upgrade
    {
        public override void AddUpgrade(Turret turret)
        {
            turret.damageOverTime += GETUpgradeValue() * turret.damageOverTime;
            turret.smashDamage += GETUpgradeValue() * turret.smashDamage;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.damageOverTime -= GETUpgradeValue() * turret.damageOverTime;
            turret.smashDamage -= GETUpgradeValue() * turret.smashDamage;
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.damage += (int) (GETUpgradeValue() * bullet.damage);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}