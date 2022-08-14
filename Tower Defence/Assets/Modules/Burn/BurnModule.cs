using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Laser;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;

namespace Modules.Burn
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "BurnT0", menuName = "Modules/Burn")]
    public class BurnModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };

        [SerializeField]
        [Tooltip("The percentage damage to deal to an enemy every tick")]
        private float burnDamage;
        [SerializeField]
        [Tooltip("How many ticks to burn the enemy for")]
        private int tickCount;
        [SerializeField]
        [Tooltip("How long each tick is in seconds")]
        private float tickDuration;

        [SerializeField]
        [Tooltip("The VFX to spawn each time a tick passes")]
        private GameObject tickEffect;
        
        /// <summary>
        /// Check if the module can be applied to the turret
        /// The turret must be a valid type
        /// The turret cannot already have the burn module applied
        /// </summary>
        /// <param name="turret">The turret the module might be applied to</param>
        /// <returns>If the module can be applied</returns>
        public override bool ValidModule(Turret turret)
        {
            return turret.modules.All(module => module.GetType() != typeof(BurnModule))
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
                Runner.Run(BurnEnemy(target));
            }
        }
        
        /// <summary>
        /// Handles the burn effect
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        private IEnumerator BurnEnemy(Enemy target)
        {
            // Check the enemy isn't already burning/has immunity
            if (target.uniqueEffects.Contains("Burn"))
            {
                yield break;
            }
            target.uniqueEffects.Add("Burn");
            
            // Loop until we've gone through every tick
            int ticksLeft = tickCount;
            while (ticksLeft > 0)
            {
                // Wait 1 tick
                yield return new WaitForSeconds(tickDuration);
                
                // Check we still have a target
                if (target == null)
                    yield break;
                
                // Deal Damage
                target.TakeDamageWithoutAbilities(burnDamage * target.maxHealth);
                ticksLeft--;
                
                // Spawn ability effect
                // GameObject effect = Instantiate(tickEffect, target.transform.position, Quaternion.identity);
                // effect.name = "_" + effect.name;
                // Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }
}
