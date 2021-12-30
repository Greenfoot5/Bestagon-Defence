using System.Collections.Generic;
using Turrets;
using Turrets.Upgrades;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "DebuffTurret", menuName = "EnemyAbilities/DebuffTurret")]
    public class DebuffTargets : EnemyAbility
    {
        [Header("Ability Stats")]
        public List<Upgrade> debuffs;
        
        
        /// <summary>
        /// Performs the ability on the target.
        /// In this case, adds debuff(s) to the target
        /// </summary>
        /// <param name="target">The turret to debuff</param>
        /// <returns>If the Ability has expired</returns>
        public override void Activate(GameObject target)
        {
            // Check we have an turret to downgrade
            var turretComponent = target.GetComponent<Turret>();
            if (turretComponent == null)
            {
                return;
            }
            
            // Add the debuffs
            foreach (var upgrade in debuffs)
            {
                turretComponent.AddUpgrade(upgrade);
            }
        }

        public override void OnCounterEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}