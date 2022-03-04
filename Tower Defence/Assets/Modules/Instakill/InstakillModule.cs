using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Modules.Instakill
{
    /// <summary>
    /// Chance to instakill an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "InstakillT0", menuName = "Modules/Instakill")]
    public class InstakillModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };
        
        [FormerlySerializedAs("percentageChange")]
        [Tooltip("The percentage chance to kill the enemy")]
        [SerializeField]
        private float instakillChance;

        /// <summary>
        /// Attempt to instakill all hit enemies
        /// </summary>
        /// <param name="targets">The targets to attempt to instakill</param>
        /// <param name="turret">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public override void OnHit(IEnumerable<Enemy> targets, Turret turret, Bullet bullet = null)
        {
            foreach (Enemy target in targets)
            {
                if (Random.value < instakillChance && target.health > 0 && !target.isBoss) 
                    target.TakeDamage(target.maxHealth, null);
            }
        }
    }
}