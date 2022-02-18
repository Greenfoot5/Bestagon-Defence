using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Enemies;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Stun
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "StunT0", menuName = "Modules/Stun")]
    public class StunModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };

        [SerializeField]
        [Tooltip("The percentage chance to stun an enemy")]
        private float stunChance;
        [SerializeField]
        [Tooltip("How long to stun the enemy for")]
        private float duration;

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
        {
            foreach (Enemy target in targets)
            {
                Runner.Run(StunEnemy(target));
            }
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        private IEnumerator StunEnemy(Enemy target)
        {
            // Check the target isn't already stunned and we hit the chance
            if (target.speed.GetBase() > 0 && Random.value < stunChance) yield break;
            
            float originalSpeed = target.speed.GetBase();
            target.speed.SetBase(0);

            yield return new WaitForSeconds(duration);

            target.speed.SetBase(originalSpeed);
        }
    }
}
