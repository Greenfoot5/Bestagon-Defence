using System;
using System.Collections.Generic;
using Enemies;
using Turrets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Reload
{
    /// <summary>
    /// Chance to deal double damage
    /// </summary>
    [CreateAssetMenu(fileName = "ReloadT0", menuName = "Modules/Reload")]
    public class ReloadModule : Module
    {
        protected override Type[] ValidTypes => null;
        
        [Tooltip("Percentage chance to deal attack again")]
        [SerializeField]
        private float reloadChance;

        /// <summary>
        /// Attempts to deal double damage on all enemies hit
        /// </summary>
        /// <param name="targets">The targets to attempt to critically strike</param>
        /// <param name="turret">The turret that attacked the enemies</param>
        /// <param name="bullet">The bullet (if any) that hit the enemies</param>
        public override void OnHit(IEnumerable<Enemy> targets, Turret turret, Bullet bullet = null)
        {
            if (Random.value < (reloadChance / turret.fireRate.GetStat()))
            {
                // We don't want to instantly fire again, we want a slight delay to make it clear the turret has attacked again
                turret.fireCountdown *= 0.1f;
            }
        }
    }
}