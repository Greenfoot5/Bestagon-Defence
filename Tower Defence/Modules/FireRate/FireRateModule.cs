using System;
using Abstract.Attributes;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;
using Turrets.Smasher;


namespace Modules.FireRate;

/// <summary>
/// Increases the fire rate of a turret
/// </summary>
[GlobalClass]
public partial class FireRateModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Smasher), typeof(Gunner), typeof(Lancer), typeof(Choker)];
        
    /// <summary>
    /// What percentage to modify the fire rate of the turret by.
    /// 
    /// If it's a gunner turret, the percentage to modify the fire rate cap, spin cooldown and spin multiplier
    /// </summary>
    [Export]
    private AttributeModifier _percentageChange;
        
    /// <summary>
    /// Increases the fire rate of a turret
    /// </summary>
    /// <param name="damager">The turret to increase the fire rate for</param>
    public override void AddModule(Damager damager)
    {
        switch (damager)
        {
            case Gunner gunner:
                gunner.Stats[AttributeType.FireRate].Add(GetSceneUniqueId(), _percentageChange);
                gunner.Stats[AttributeType.SpinCooldown].Add(GetSceneUniqueId(), _percentageChange);
                gunner.Stats[AttributeType.SpinMultiplier].Add(GetSceneUniqueId(), _percentageChange);
                gunner.Stats[AttributeType.FireRate].Add(GetSceneUniqueId() + Operation.AddMax, 
                    new AttributeModifier(_percentageChange.Value, Operation.AddMax));
                break;
            case Turret turret:
                turret.Stats[AttributeType.FireRate].Add(GetSceneUniqueId(), _percentageChange);
                break;
        }
    }
        
    /// <summary>
    /// Removes the fire rate increase
    /// </summary>
    /// <param name="damager">The turret to remove the fire rate for</param>
    public override void RemoveModule(Damager damager)
    {
        switch (damager)
        {
            case Gunner gunner:
                gunner.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId());
                gunner.Stats[AttributeType.SpinCooldown].Remove(GetSceneUniqueId());
                gunner.Stats[AttributeType.SpinMultiplier].Remove(GetSceneUniqueId());
                gunner.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId() + Operation.AddMax);
                break;
            case Turret turret:
                turret.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId());
                break;
        }
    }
}