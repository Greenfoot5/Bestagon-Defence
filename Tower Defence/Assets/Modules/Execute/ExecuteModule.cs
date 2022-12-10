using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using UnityEngine;

namespace Modules.Execute
{
    /// <summary>
    /// Extends the module class to create the execute module
    /// </summary>
    [CreateAssetMenu(fileName = "ExecuteT0", menuName = "Modules/Execute")]
    public class ExecuteModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner), typeof(Lancer) };
        
        [Tooltip("The maximum percentage of health the enemy can have before they get executed")]
        [SerializeField]
        private float percentageHealthRemaining;

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
                if ((target.health / target.maxHealth) <= percentageHealthRemaining)
                {
                    target.TakeDamage(target.maxHealth, null);
                }
            }
        }
    }
}