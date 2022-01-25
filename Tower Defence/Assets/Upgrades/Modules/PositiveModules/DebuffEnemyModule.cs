using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Turrets.Modules
{
    [CreateAssetMenu(fileName = "DebuffEnemy", menuName = "Modules/Debuff Enemy")]
    public class DebuffEnemyModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [SerializeField]
        private List<EnemyAbility> debuffs;
        
        public override void AddModule(Turret turret) { }

        public override void RemoveModule(Turret turret) { }

        public override void OnShoot(Bullet bullet) { }

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
