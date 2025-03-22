using System;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.Reload
{
    /// <summary>
    /// Chance to attack again
    /// </summary>
    [CreateAssetMenu(fileName = "ReloadT0", menuName = "Modules/Reload")]
    public class ReloadModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Choker), typeof(Gunner), typeof(Lancer) };
        
        [Tooltip("Multiplicative percentage modifier to shooter's damage")]
        [SerializeField]
        private float shooterDamageChange;
        [Tooltip("Additive percentage modifier to shooter's fire rate")]
        [SerializeField]
        private float shooterFireRateChange;
        [Tooltip("Additive percentage modifier to shooter's rotation speed")]
        [SerializeField]
        private float shooterRotationSpeedChange;
        
        [Header("Lancer")]
        [Tooltip("Additive percentage modifier to lancer's range")]
        [SerializeField]
        private float lancerRangeChange;
        [Tooltip("Additive percentage modifier to lancer's damage")]
        [SerializeField]
        private float lancerDamageChange;
        [Tooltip("Additive percentage modifier to lancer's fire rate")]
        [SerializeField]
        private float lancerFireRateChange;
        [Tooltip("Additive percentage modifier to lancer's arrow range")]
        [SerializeField]
        private float lancerArrowRangeChange;
        [Tooltip("Additive percentage modifier to lancer's arrow speed")]
        [SerializeField]
        private float lancerArrowSpeedChange;
        [Tooltip("Additive percentage modifier to lancer's arrow knockback")]
        [SerializeField]
        private float lancerArrowKnockbackChange;

        [Header("Laser")]
        [Tooltip("Additive percentage modifier to smasher's range")]
        [SerializeField]
        private float smasherRangeChange;
        [Tooltip("Additive percentage modifier to smasher's damage")]
        [SerializeField]
        private float smasherDamageChange;

        [Header("Choker")]
        [Tooltip("Additive percentage modifier to choker's range")]
        [SerializeField]
        private float chokerRangeChange;
        [Tooltip("Multiplicative percentage modifier to choker's damage")]
        [SerializeField]
        private float chokerDamageChange;
        [Tooltip("Additive percentage modifier to choker's fire rate")]
        [SerializeField]
        private float chokerFireRateChange;
        [Tooltip("Additive percentage modifier to choker's part spread")]
        [SerializeField]
        private float chokerPartSpreadChange;
        [Tooltip("Additive percentage modifier to choker's part spread")]
        [SerializeField]
        private float chokerPartCountChange;

        public override void AddModule(Damager damager)
        {
            damager.OnAttack += OnAttack;
        }

        public override void RemoveModule(Damager damager)
        {
            damager.OnAttack -= OnAttack;
        }

        /// <summary>
        /// When attacking, checks to see if the turret should attack again
        /// </summary>
        /// <param name="damager">The turret that attacked</param>
        private void OnAttack(Damager damager)
        {
            if (damager is not Turret turret) return;
            
            if (Random.value < (reloadChance / turret.fireRate.GetStat()))
            {
                // We don't want to instantly fire again, we want a slight delay to make it clear the turret has attacked again
                turret.fireCountdown *= 0.1f;
            }
        }
    }
}