using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using UnityEngine;
using Upgrades.Abilities;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "DebuffEnemy", menuName = "Modules/Debuff Enemy")]
    public class DebuffEnemyModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser) };

        [SerializeField]
        private List<EnemyAbility> debuffs;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
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
