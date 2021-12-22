using System.Collections.Generic;
using Turrets;
using Turrets.Upgrades;
using UI;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "DebuffTurret", menuName = "EnemyAbilities/DebuffTurret")]
    public class DebuffTargets : EnemyAbility
    {
        [Header("Ability Stats")]
        public List<Upgrade> debuffs;

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
    }
}