using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using UnityEngine;

namespace Modules.Giantslayer
{
    /// <summary>
    /// Deals extra damage to bosses
    /// </summary>
    [CreateAssetMenu(fileName = "GiantslayerT0", menuName = "Modules/Giantslayer")]
    public class GiantslayerModule : Module
    {
        protected override Type[] ValidTypes => null;

        [SerializeField]
        [Tooltip("The percentage damage change if attacking a boss")]
        private float damagePercentage;
        
        /// <summary>
        /// Attempts to deal double damage on all enemies hit
        /// </summary>
        /// <param name="targets">The targets to attempt to critically strike</param>
        /// <param name="turret">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public override void OnHit(IEnumerable<Enemy> targets, Turret turret, Bullet bullet = null)
        {
            foreach (Enemy target in targets)
            {
                if (!target.isBoss) continue;

                float baseDamage = bullet == null ? turret.damage.GetStat() : bullet.damage.GetStat();
                target.TakeDamageWithoutAbilities(baseDamage * damagePercentage);
            }
        }
    }
}