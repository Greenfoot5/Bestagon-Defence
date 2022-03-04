using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.CriticalStrike
{
    /// <summary>
    /// Chance to deal double damage
    /// </summary>
    [CreateAssetMenu(fileName = "CriticalStrikeT0", menuName = "Modules/Critical Strike")]
    public class CriticalStrike : Module
    {
        protected override Type[] ValidTypes => null;
        
        [Tooltip("Percentage chance to deal double damage")]
        [SerializeField]
        private float criticalChance;

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
                if (!(Random.value < criticalChance)) continue;

                target.TakeDamageWithoutAbilities(bullet == null ? turret.damage.GetStat() : bullet.damage.GetStat());
            }
        }
    }
}