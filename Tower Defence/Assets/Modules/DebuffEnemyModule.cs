using System;
using System.Collections.Generic;
using _WIP.Abilities;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Laser;
using Turrets.Shooter;
using UnityEngine;

namespace Modules
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "DebuffEnemy", menuName = "Modules/Debuff Enemy")]
    public class DebuffEnemyModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Laser), typeof(Gunner) };

        [SerializeField]
        private List<EnemyAbility> debuffs;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (Enemy target in targets)
            {
                foreach (EnemyAbility debuff in debuffs)
                {
                    target.GrantAbility(debuff);
                }
            }
        }
    }
}