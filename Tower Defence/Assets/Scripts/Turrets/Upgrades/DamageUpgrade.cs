using System.Collections.Generic;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "DamageT0", menuName = "Upgrades/Damage")]
    public class DamageUpgrade : Upgrade
    {
        [SerializeField]
        private float percentageIncrease;
        public override void AddUpgrade(Turret turret)
        {
            turret.damageOverTime += percentageIncrease * turret.damageOverTime;
            turret.smashDamage += percentageIncrease * turret.smashDamage;
        }

        public override void RemoveUpgrade(Turret turret)
        {
            turret.damageOverTime -= percentageIncrease * turret.damageOverTime;
            turret.smashDamage -= percentageIncrease * turret.smashDamage;
        }

        public override void OnShoot(Bullet bullet)
        {
            bullet.damage += (percentageIncrease * bullet.damage);
        }

        public override void OnHit(IEnumerable<Enemy> targets) { }
    }
}