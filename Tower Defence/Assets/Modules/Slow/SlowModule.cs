using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using Enemies;
using Turrets;
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
        
        [SerializeField]
        [Tooltip("The percentage the slow the enemy's movement speed")]
        private float slowPercentage;
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
        [Tooltip("The percentage change to the smasher's range")]
        private float smasherRangeChange;
        
        /// <summary>
        /// Modifies the stats of the turret when applied
        /// </summary>
        /// <param name="turret">The turret to modify the stats for</param>
        public override void AddModule(Turret turret)
        {
            if (turret.GetType() == typeof(Gunner))
            {
                var gunner = (Gunner)turret;
                gunner.spinMultiplier.AddModifier(gunnerSpinUpChange);
                gunner.spinCooldown.AddModifier(gunnerSpinDownChange);
            }
            else if (turret.GetType() == typeof(Smasher))
            {
                var smasher = (Smasher)turret;
                smasher.range.AddModifier(smasherRangeChange);
            }
        }
        
        /// <summary>
        /// Removes stats modifications of the turret
        /// </summary>
        /// <param name="turret">The turrets to remove the modifications of</param>
        public override void RemoveModule(Turret turret)
        {
            if (turret.GetType() == typeof(Gunner))
            {
                var gunner = (Gunner)turret;
                gunner.spinMultiplier.TakeModifier(gunnerSpinUpChange);
                gunner.spinCooldown.TakeModifier(gunnerSpinDownChange);
            }
            else if (turret.GetType() == typeof(Smasher))
            {
                var smasher = (Smasher)turret;
                smasher.range.TakeModifier(smasherRangeChange);
            }
        }

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
