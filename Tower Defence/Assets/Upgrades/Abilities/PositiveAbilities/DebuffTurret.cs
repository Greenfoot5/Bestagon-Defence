using System.Collections.Generic;
using Turrets;
using Turrets.Modules;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "DebuffTurret", menuName = "Enemy Abilities/Debuff Turret")]
    public class DebuffTurret : EnemyAbility
    {
        [Header("Ability Stats")]
        public List<Module> debuffs;
        
        
        /// <summary>
        /// Performs the ability on the target.
        /// In this case, adds debuff(s) to the target
        /// </summary>
        /// <param name="target">The turret to debuff</param>
        /// <returns>If the Ability has expired</returns>
        public override void Activate(GameObject target)
        {
            // Check there is a turret to downgrade
            var turretComponent = target.GetComponent<Turret>();
            if (turretComponent == null)
            {
                return;
            }
            
            // Add the debuffs
            foreach (var Module in debuffs)
            {
                turretComponent.AddModule(Module);
            }
        }

        public override void OnCounterEnd(GameObject target)
        {
            throw new System.NotImplementedException();
        }
    }
}