using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using Enemies;
using Turrets;
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
        /// Check if the module can be applied to the turret
        /// The turret must be a valid type
        /// The turret cannot already have the stun module applied
        /// </summary>
        /// <param name="turret">The turret the module might be applied to</param>
        /// <returns>If the module can be applied</returns>
        public override bool ValidModule(Turret turret)
        {
            return turret.modules.All(module => module.GetType() != typeof(StunModule))
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
                Runner.Run(StunEnemy(target));
            }
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        private IEnumerator StunEnemy(Enemy target)
        {
            // Check the target isn't already stunned and the turret hit the chance
            if (target.speed.GetBase() > 0 && Random.value < stunChance) yield break;
            
            float originalSpeed = target.speed.GetBase();
            target.speed.SetBase(0);

            yield return new WaitForSeconds(duration);

            target.speed.SetBase(originalSpeed);
        }
    }
}
