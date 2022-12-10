using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Modules.Stun
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "StunT0", menuName = "ModuleTiers/Stun")]
    public class StunModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };

        [FormerlySerializedAs("stunChance")]
        [SerializeField]
        [Tooltip("The percentage chance to stun an enemy")]
        private float enemyStunChance;
        [FormerlySerializedAs("duration")]
        [SerializeField]
        [Tooltip("How long to stun the enemy for")]
        private float enemyDuration;
        [SerializeField]
        [Tooltip("percentage chance to stun the tower each attack")]
        private float turretStunChance;
        [SerializeField]
        [Tooltip("How long to stun the tower for")]
        private float turretDuration;
        
        /// <summary>
        /// Check if the module can be applied to the turret
        /// The turret must be a valid type
        /// The turret cannot already have the stun module applied
        /// </summary>
        /// <param name="turret">The turret the module might be applied to</param>
        /// <returns>If the module can be applied</returns>
        public override bool ValidModule(Turret turret)
        {
            return turret.moduleHandlers.All(module => module.GetType() != typeof(StunModule))
                   && ((IList)ValidTypes).Contains(turret.GetType());
        }

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
                Runner.Run(StunEnemy(target, turret));
            }
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to stun</param>
        /// <param name="turret">The turret attempting to stun</param>
        private IEnumerator StunEnemy(Enemy target, Turret turret)
        {
            // Check the enemy isn't already stunned/has immunity
            if (target.uniqueEffects.Contains("Stun"))
            {
                yield break;
            }
            target.uniqueEffects.Add("Stun");
            
            float originalSpeed = target.speed.GetBase();
            // Check the target isn't already stunned (again) and the turret hit the chance
            if (originalSpeed <= 0 || Random.value > (enemyStunChance / turret.fireRate.GetStat())) yield break;
            
            target.speed.SetBase(0);

            yield return new WaitForSeconds(enemyDuration);

            target.speed.SetBase(originalSpeed);
        }
        
        public override void OnAttack(Turret turret)
        {
            if (Random.value < turretStunChance) 
                Runner.Run(StunTurret(turret));
        }

        private IEnumerator StunTurret(Turret turret)
        {
            float originalFireRate = turret.fireRate.GetBase();
            
            // Check the turret isn't already stunned
            if (originalFireRate <= 0)
                yield break;
            
            turret.fireRate.SetBase(0);
            
            yield return new WaitForSeconds(turretDuration);
            
            turret.fireRate.SetBase(originalFireRate);
        }
    }
}
