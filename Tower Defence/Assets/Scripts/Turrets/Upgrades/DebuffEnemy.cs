using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Turrets.Upgrades
{
    [CreateAssetMenu(fileName = "DebuffEnemy", menuName = "Upgrades/DebuffEnemy")]
    public class DebuffEnemy : Upgrade
    {
        protected override Type[] ValidTypes => null;  // any

        [SerializeField]
        private List<EnemyAbility> debuffs;
        
        
        public override void AddUpgrade(Turret turret) { }

        public override void RemoveUpgrade(Turret turret) { }

        public override void OnShoot(Bullet bullet)
        {
            bullet.AddUpgrade(this);
        }

        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (var target in targets)
            {
                foreach (var debuff in debuffs)
                {
                    target.GrantAbility(debuff);
                }
            }
        }
    }
}
