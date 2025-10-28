using System;
using BestagonDefence.Abstract.Attributes;
using BestagonDefence.Turrets;
using Godot;

namespace BestagonDefence.Modules.Damage;

/// <summary>
/// Extends the Module class to create a Damage upgrade
/// </summary>
[GlobalClass]
[Icon("./TA_Damage.tres")]
[Tool]
public partial class DamageModule : Module
{
    protected override Type[] ValidTypes => [typeof(Turret)];  // any
        
    /// <summary>
    /// What percentage to modify the damage by
    /// </summary>
    [Export]
    private Modifier percentageChange;
        
    /// <summary>
    /// Increases the damage for a turret
    /// </summary>
    /// <param name="damager">The turret to increase damage for</param>
    public override void AddModule(Damager damager)
    {
        damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), percentageChange);
    }
        
    /// <summary>
    /// Removes a damage upgrade for a turret
    /// </summary>
    /// <param name="damager">The turret to remove a damage upgrade for</param>
    public override void RemoveModule(Damager damager)
    {
        damager.Stats[AttributeType.Damage].Remove(GetSceneUniqueId());
    }
}