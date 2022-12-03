using System;
using Turrets;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Reload
{
    /// <summary>
    /// Chance to attack again
    /// </summary>
    [CreateAssetMenu(fileName = "ReloadT0", menuName = "ModuleTiers/Reload")]
    public class ReloadModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer) };
        
        [Tooltip("Percentage chance to deal attack again")]
        [SerializeField]
        private float reloadChance;

        /// <summary>
        /// When attacking, checks to see if the turret should attack again
        /// </summary>
        /// <param name="turret">The turret that attacked</param>
        public override void OnAttack(Turret turret)
        {
            if (Random.value < (reloadChance / turret.fireRate.GetStat()))
            {
                // We don't want to instantly fire again, we want a slight delay to make it clear the turret has attacked again
                turret.fireCountdown *= 0.1f;
            }
        }
    }
}