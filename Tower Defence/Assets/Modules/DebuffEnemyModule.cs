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
        
        [Tooltip("Which debuff enemy abilities to apply to enemies on hit")]
        [SerializeField]
        private List<EnemyAbility> debuffs;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        /// <param name="turret">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public override void OnHit(IEnumerable<Enemy> targets, Turret turret, Bullet bullet = null)
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
