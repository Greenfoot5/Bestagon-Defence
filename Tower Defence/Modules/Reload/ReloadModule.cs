using System;
using Abstract.Attributes;
using Godot;
using Turrets;
using Turrets.Choker;
using Turrets.Gunner;
using Turrets.Lancer;
using Turrets.Shooter;

namespace Modules.Reload;

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
    private AttributeModifier _shooterDamageChange = new(0f, Operation.Multiplicative);
    /// <summary>
    /// Additive percentage modifier to shooter's fire rate
    /// </summary>
    [Export]
    private AttributeModifier _shooterFireRateChange;
    /// <summary>
    /// Additive percentage modifier to shooter's rotation speed
    /// </summary>
    [Export]
    private AttributeModifier _shooterRotationSpeedChange;
        
    /// <summary>
    /// Additive percentage modifier to lancer's range
    /// </summary>
    [ExportGroup("Lancer")]
    [Export]
    private AttributeModifier _lancerRangeChange;
    /// <summary>
    /// Additive percentage modifier to lancer's damage
    /// </summary>
    [Export]
    private AttributeModifier _lancerDamageChange;
    /// <summary>
    /// Additive percentage modifier to lancer's fire rate
    /// </summary>
    [Export]
    private AttributeModifier _lancerFireRateChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow range
    /// </summary>
    [Export]
    private AttributeModifier _lancerArrowRangeChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow speed
    /// </summary>
    [Export]
    private AttributeModifier _lancerArrowSpeedChange;
    /// <summary>
    /// Additive percentage modifier to lancer's arrow knockback
    /// </summary>
    [Export]
    private AttributeModifier _lancerArrowKnockbackChange;
        
    /// <summary>
    /// Additive percentage modifier to smasher's range
    /// </summary>
    [ExportGroup("Laser")]
    [Export]
    private AttributeModifier _smasherRangeChange;
    /// <summary>
    /// Additive percentage modifier to smasher's damage
    /// </summary>
    [Export]
    private AttributeModifier _smasherDamageChange;
        
    /// <summary>
    /// Additive percentage modifier to choker's range
    /// </summary>
    [ExportGroup("Choker")]
    [Export]
    private AttributeModifier _chokerRangeChange;
    /// <summary>
    /// Multiplicative percentage modifier to choker's damage
    /// </summary>
    [Export]
    private AttributeModifier _chokerDamageChange = new(0f, Operation.Multiplicative);
    /// <summary>
    /// Additive percentage modifier to choker's fire rate
    /// </summary>
    [Export]
    private AttributeModifier _chokerFireRateChange;
    /// <summary>
    /// Additive percentage modifier to choker's part spread
    /// </summary>
    [Export]
    private AttributeModifier _chokerPartSpreadChange;
    /// <summary>
    /// Additive percentage modifier to choker's part spread
    /// </summary>
    [Export]
    private AttributeModifier _chokerPartCountChange;

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
            
        if (GD.Randf() < (0.1f / turret.Stats[AttributeType.FireRate].Value))
        {
            // We don't want to instantly fire again, we want a slight delay to make it clear the turret has attacked again
            turret.FireCountdown *= 0.1f;
        }
    }
}