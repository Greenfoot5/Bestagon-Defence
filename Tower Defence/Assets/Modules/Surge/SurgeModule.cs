using System;
using System.Collections;
using System.Linq;
using Abstract;
using Turrets;
using Turrets.Gunner;
using Turrets.Shooter;
using Turrets.Smasher;
using UI.Nodes;
using UnityEngine;

namespace Modules.Surge
{
    /// <summary>
    /// Extends the Module class to create a DebuffEnemy upgrade,
    /// Used to add effects to enemies
    /// </summary>
    [CreateAssetMenu(fileName = "SurgeT0", menuName = "Modules/Surge")]
    public class SurgeModule : Module
    {
        protected override Type[] ValidTypes => new[] { typeof(Shooter), typeof(Smasher), typeof(Gunner) };
        
        [SerializeField]
        [Tooltip("The damage to deal to an enemy every tick")]
        private float fireRateChange;
        [SerializeField]
        [Tooltip("How many ticks to burn the enemy for")]
        private int duration;
        [SerializeField]
        [Tooltip("How long each tick is in seconds")]
        private float cooldown;
        
        [SerializeField]
        [Tooltip("The VFX to spawn when the turret surges")]
        private GameObject surgeEffect;
        [SerializeField]
        [Tooltip("The VFX to spawn when the ends it's turret surge")]
        private GameObject surgeEndEffect;
        
        /// <summary>
        /// Check if the module can be applied to the turret
        /// The turret must be a valid type
        /// The turret cannot already have the surge module applied
        /// </summary>
        /// <param name="turret">The turret the module might be applied to</param>
        /// <returns>If the module can be applied</returns>
        public override bool ValidModule(Turret turret)
        {
            return turret.modules.All(module => module.GetType() != typeof(SurgeModule))
                   && ((IList)ValidTypes).Contains(turret.GetType());
        }

        /// <summary>
        /// Begins the surge effect on the turret
        /// </summary>
        /// <param name="turret">The turret to start the surge loop on</param>
        public override void AddModule(Turret turret)
        {
            Runner.Run(Surge(turret));
        }
        
        /// <summary>
        /// Handles the poison effect
        /// </summary>
        /// <param name="turret">The turret to increase the fire rate for</param>
        private IEnumerator Surge(Turret turret)
        {
            while (turret.modules.All(module => module.GetType() == typeof(SurgeModule)))
            {
                // Wait the cooldown
                yield return new WaitForSeconds(cooldown);
                
                // Check we still have the module
                if (turret.modules.Any(module => module.GetType() != typeof(SurgeModule)))
                    yield break;
                
                // SURGE!
                Debug.Log("Surge Begins!");
                turret.fireRate.AddModifier(fireRateChange);
                NodeUI.instance.UpdateStats();
                GameObject effect = Instantiate(surgeEffect, turret.transform.position, Quaternion.identity);
                effect.name = "_" + effect.name;
                Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
                

                yield return new WaitForSeconds(duration);
                
                Debug.Log("Surge Ends...");
                turret.fireRate.TakeModifier(fireRateChange);
                NodeUI.instance.UpdateStats();
                GameObject endEffect = Instantiate(surgeEndEffect, turret.transform.position, Quaternion.identity);
                endEffect.name = "_" + endEffect.name;
                Destroy(endEffect, endEffect.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }
}