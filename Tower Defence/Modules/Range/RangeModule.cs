using System;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Turrets;
using BestagonDefense.Turrets.Lancer;
using Godot;

namespace BestagonDefense.Modules.Range;

/// <summary>
/// Increases the range of a turret
/// </summary>
[GlobalClass]
[Tool]
public partial class RangeModule : Module
{
    protected override Type[] ValidTypes => [typeof(Turret)];  // any
        
    /// <summary>
    /// The percentage to modify the range of the turret by
    /// </summary>
    [Export]
    private Modifier _percentageChange;
        
    /// <summary>
    /// Increases the range of a turret
    /// </summary>
    /// <param name="damager">The turret to increase range for</param>
    public override void AddModule(Damager damager)
    {
        switch (damager)
        {
            case Lancer:
                damager.Stats[AttributeType.BulletRange].Add(GetSceneUniqueId(), _percentageChange);
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _percentageChange);
                break;
            case Turret:
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _percentageChange);
                break;
        }
    }
        
    /// <summary>
    /// Removes the range increase from a turret
    /// </summary>
    /// <param name="damager">The turret to decrease the range for</param>
    public override void RemoveModule(Damager damager)
    {
        switch (damager)
        {
            case Lancer:
                damager.Stats[AttributeType.BulletRange].Remove(GetSceneUniqueId());
                damager.Stats[AttributeType.Range].Remove(GetSceneUniqueId());
                break;
            case Turret:
                damager.Stats[AttributeType.Range].Remove(GetSceneUniqueId());
                break;
        }
    }
}