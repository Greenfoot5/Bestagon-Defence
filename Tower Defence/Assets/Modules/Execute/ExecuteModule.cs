using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Turrets;
using Turrets.Gunner;
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
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Gunner) };
        
        [Tooltip("The maximum percentage of health the enemy can have before they get executed")]
        [SerializeField]
        private float percentageHealthRemaining;
        
        /// <summary>
        /// Check if the module can be applied to the turret
        /// The turret must be a valid type
        /// The turret cannot already have the execute module applied
        /// </summary>
        /// <param name="turret">The turret the module might be applied to</param>
        /// <returns>If the module can be applied</returns>
        public override bool ValidModule(Turret turret)
        {
            return turret.modules.All(module => module.GetType() != typeof(ExecuteModule))
                   && ((IList)ValidTypes).Contains(turret.GetType());
        }
        
        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
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