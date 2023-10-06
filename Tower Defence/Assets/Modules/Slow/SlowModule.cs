using System;
using System.Collections;
using Abstract;
using Abstract.Data;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using UnityEngine.Localization;

namespace Modules.Slow
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "SlowT0", menuName = "Modules/Slow")]
    public class SlowModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };
        
        [SerializeField]
        [Tooltip("The percentage the slow the enemy's movement speed")]
        private float slowPercentage;
        
        [Header("Gunner & Shooter")]
        [SerializeField]
        [Tooltip("How long each slow stack should last")]
        private float duration;
        
        [Header("Gunner")]
        [SerializeField]
        [Tooltip("The percentage change to the gunner's spin up multiplier")]
        private float gunnerSpinUpChange;
        [SerializeField]
        [Tooltip("The percentage change to the gunner's spin down multiplier")]
        private float gunnerSpinDownChange;

        [Header("Smasher")]
        [SerializeField]
        [Tooltip("The percentage the slow the enemy's movement speed")]
        private float smasherSlowPercentage;
        [DisplayName("Duration")]
        [Tooltip("The percentage of the attack speed cooldown the slow will last")]
        [SerializeField]
        private float smasherDuration;
        [SerializeField]
        [Tooltip("The percentage change to the smasher's range")]
        private float smasherRangeChange;

        /// <summary>
        /// Modifies the stats of the turret when applied
        /// </summary>
        /// <param name="damager">The turret to modify the stats for</param>
        public override void AddModule(Damager damager)
        {
            switch (damager)
            {
                case Gunner gunner:
                    gunner.spinMultiplier.AddModifier(gunnerSpinUpChange);
                    gunner.spinCooldown.AddModifier(gunnerSpinDownChange);
                    break;
                case Smasher smasher:
                    smasher.range.AddModifier(smasherRangeChange);
                    break;
            }

            damager.OnHit += OnHit;
        }
        
        /// <summary>
        /// Removes stats modifications of the turret
        /// </summary>
        /// <param name="damager">The turrets to remove the modifications of</param>
        public override void RemoveModule(Damager damager)
        {
            switch (damager)
            {
                case Gunner gunner:
                    gunner.spinMultiplier.TakeModifier(gunnerSpinUpChange);
                    gunner.spinCooldown.TakeModifier(gunnerSpinDownChange);
                    break;
                case Smasher smasher:
                    smasher.range.TakeModifier(smasherRangeChange);
                    break;
            }
        }

        /// <summary>
        /// Adds the EnemyAbility to some target(s)
        /// </summary>
        /// <param name="target">The target(s) to apply the ability to</param>
        /// <param name="damager">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public void OnHit(Enemy target, Damager damager, Bullet bullet = null)
        {
            if (damager is not Turret turret) return;
            Runner.Run(turret.GetType() == typeof(Smasher)
                ? SmasherSlowEnemy(target, turret.fireRate)
                : SlowEnemy(target));
        }
        
        /// <summary>
        /// Applies the slow effect for a set duration
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        private IEnumerator SlowEnemy(Enemy target)
        {
            // Check the enemy is immune
            if (target.uniqueEffects.Contains("Slow"))
            {
                yield break;
            }
            
            float slowValue = 1f - slowPercentage;

            if (target.speed.GetModifier() * slowValue <= 0.4f)
            {
                yield break;
            }
            
            target.speed.MultiplyModifier(slowValue);

            yield return new WaitForSeconds(duration);

            target.speed.DivideModifier(slowValue);
        }

        /// <summary>
        /// Applies the slow effect for a set duration for smasher
        /// </summary>
        /// <param name="target">The enemy to slow</param>
        /// <param name="fireRate">The fire rate of the smasher</param>
        private IEnumerator SmasherSlowEnemy(Enemy target, UpgradableStat fireRate)
        {
            // Check the enemy is immune
            if (target.uniqueEffects.Contains("Slow") || fireRate.GetStat() == 0)
            {
                yield break;
            }
            
            float slowValue = 1f - smasherSlowPercentage;

            if (target.speed.GetModifier() * slowValue <= 0.4f)
            {
                yield break;
            }
            
            target.speed.MultiplyModifier(slowValue);
            
            yield return new WaitForSeconds(1f / (fireRate.GetStat() * smasherDuration));

            target.speed.DivideModifier(slowValue);
        }
    }
}
