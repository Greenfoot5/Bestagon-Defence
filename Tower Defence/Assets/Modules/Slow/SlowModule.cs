using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Enemies;
using Turrets;
using UnityEngine;

namespace Upgrades.Modules.PositiveModules
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "SlowT0", menuName = "Modules/Slow")]
    public class SlowModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };

        [SerializeField]
        [Tooltip("The percentage the slow the enemy's movement speed")]
        private float slowPercentage;
        [SerializeField]
        private float duration;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (var target in targets)
            {
                Runner.Run(SlowEnemy(target));
            }
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        private IEnumerator SlowEnemy(Enemy target)
        {
            target.speed *= 1f - slowPercentage;

            yield return new WaitForSeconds(duration);

            target.speed /= 1f - slowPercentage;
        }
    }
}
