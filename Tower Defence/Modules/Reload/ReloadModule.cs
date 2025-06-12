using System;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Turrets;
using BestagonDefense.Turrets.Choker;
using BestagonDefense.Turrets.Gunner;
using BestagonDefense.Turrets.Lancer;
using Godot;

namespace BestagonDefense.Modules.Reload;

/// <summary>
/// Chance to attack again
/// </summary>
[GlobalClass]
[Tool]
public partial class ReloadModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Choker), typeof(Gunner), typeof(Lancer)];
        
    /// <summary>
    /// Multiplicative percentage modifier to shooter's damage
    /// </summary>
    [Export]
    private Modifier _shooterDamageChange = new(0f, Operation.Multiplicative);
    /// <summary>
    /// Additive percentage modifier to shooter's fire rate
    /// </summary>
    [Export]
    private Modifier _shooterFireRateChange;
    /// <summary>
    /// Additive percentage modifier to shooter's rotation speed
    /// </summary>
    [Export]
    private Modifier _shooterRotationSpeedChange;
        
    /// <summary>
    /// Additive percentage modifier to lancer's range
    /// </summary>
    [ExportGroup("Lancer")]
    [Export]
    private Modifier _lancerRangeChange;
    /// <summary>
    /// Additive percentage modifier to lancer's damage
    /// </summary>
    [Export]
    private Modifier _lancerDamageChange;
    /// <summary>
    /// Additive percentage modifier to lancer's fire rate
    /// </summary>
    [Export]
    private Modifier _lancerFireRateChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow range
    /// </summary>
    [Export]
    private Modifier _lancerArrowRangeChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow speed
    /// </summary>
    [Export]
    private Modifier _lancerArrowSpeedChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow knockback
    /// </summary>
    [Export]
    private Modifier _lancerArrowKnockbackChange;
        
    /// <summary>
    /// Additive percentage modifier to smasher's range
    /// </summary>
    [ExportGroup("Laser")]
    [Export]
    private Modifier _smasherRangeChange;
    /// <summary>
    /// Additive percentage modifier to smasher's damage
    /// </summary>
    [Export]
    private Modifier _smasherDamageChange;
        
    /// <summary>
    /// Additive percentage modifier to choker's range
    /// </summary>
    [ExportGroup("Choker")]
    [Export]
    private Modifier _chokerRangeChange;
    /// <summary>
    /// Multiplicative percentage modifier to choker's damage
    /// </summary>
    [Export]
    private Modifier _chokerDamageChange = new(0f, Operation.Multiplicative);
    /// <summary>
    /// Additive percentage modifier to choker's fire rate
    /// </summary>
    [Export]
    private Modifier _chokerFireRateChange;
    /// <summary>
    /// Additive percentage modifier to choker's part spread
    /// </summary>
    [Export]
    private Modifier _chokerPartSpreadChange;
    /// <summary>
    /// Additive percentage modifier to choker's part spread
    /// </summary>
    [Export]
    private Modifier _chokerPartCountChange;

    /// <summary>
    /// Handles applying the effects of the module on the damager
    /// </summary>
    /// <param name="damager">The damager to remove the module from</param>
    public override void AddModule(Damager damager)
    {
        damager.OnAttack += OnAttack;
    }

    /// <summary>
    /// Handles removing the effects of the module on the damager
    /// </summary>
    /// <param name="damager">The damager to remove the module from</param>
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
            
        if (GD.Randf() < (0.1f / turret.Stats[AttributeType.FireRate].Value))
        {
            // We don't want to instantly fire again, we want a slight delay to make it clear the turret has attacked again
            turret.FireCountdown *= 0.1f;
        }
    }
}