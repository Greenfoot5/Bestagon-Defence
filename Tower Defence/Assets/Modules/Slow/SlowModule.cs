using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Enemies;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.Slow
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "SlowT0", menuName = "Modules/Slow")]
    public class SlowModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };
        
        [Tooltip("The percentage the slow the enemy's movement speed")]
        [SerializeField]
        private float slowPercentage;
        [Tooltip("How long each slow stack should last")]
        [SerializeField]
        private float duration;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (Enemy target in targets)
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
            float slowValue = Mathf.Max(1f - slowPercentage, 0.2f);
            
            target.speed.MultiplyModifier(slowValue);

            yield return new WaitForSeconds(duration);

            target.speed.DivideModifier(slowValue);
        }
    }
}
