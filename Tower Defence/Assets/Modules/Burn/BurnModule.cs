using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Enemies;
using Turrets.Gunner;
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
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };

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
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="targets">The target(s) to apply the ability to</param>
        public override void OnHit(IEnumerable<Enemy> targets)
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
