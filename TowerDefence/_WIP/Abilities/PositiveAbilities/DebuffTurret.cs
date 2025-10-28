using System;
using System.Collections.Generic;
using BestagonDefence.Abstract.Data;
using Godot;

namespace BestagonDefence._WIP.Abilities.PositiveAbilities;

/// <summary>
/// Applies a module to turret(s)
/// </summary>
public partial class DebuffTurret : EnemyAbility
{
    /// <summary>
    /// The list of debuffs to apply to the turret
    /// </summary>
    [ExportGroup("Ability Stats")]
    // [Export]
    private List<ModuleChainHandler> debuffs;
        
        
    /// <summary>
    /// Applies a module to a turret
    /// </summary>
    /// <param name="target">The turret to debuff</param>
    public override void Activate(GodotObject target)
    {
        // TODO - Check Node is turret
        // Check there is a turret to downgrade
        // var turretComponent = target.GetComponent<Turret>();
        // if (turretComponent == null)
        // {
        //     return;
        // }
            
        // Add the debuffs
        foreach (ModuleChainHandler handler in debuffs)
        {
            // turretComponent.AddModule(handler);
        }
    }
        
    /// <summary>
    /// Removes the debuff from a turret
    /// </summary>
    /// <param name="target">The turret to remove the debuff for</param>
    /// <exception cref="NotImplementedException">The function isn't implemented yet</exception>
    public override void OnCounterEnd(GodotObject target)
    {
        throw new NotImplementedException();
    }
}