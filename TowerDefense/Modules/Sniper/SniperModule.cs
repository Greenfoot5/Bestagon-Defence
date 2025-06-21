using System;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Turrets;
using BestagonDefense.Turrets.Choker;
using BestagonDefense.Turrets.Lancer;
using BestagonDefense.Turrets.Laser;
using Godot;

namespace BestagonDefense.Modules.Sniper;

/// <summary>
/// Extends the Module class to create a Sniper upgrade
/// </summary>
[GlobalClass]
[Icon("./TA_Sniper.tres")]
[Tool]
public partial class SniperModule : Module
{
    protected override Type[] ValidTypes => [typeof(Shooter), typeof(Laser), typeof(Choker), typeof(Lancer)];
        
    /// <summary>
    /// Additive percentage modifier to shooter's range
    /// </summary>
    [ExportGroup("Shooter")]
    [Export]
    private Modifier _shooterRangeChange;
    /// <summary>
    /// Additive percentage modifier to shooter's damage
    /// </summary>
    [Export]
    private Modifier _shooterDamageChange;
    /// <summary>
    /// Additive percentage modifier to shooter's fire rate
    /// </summary>
    [Export]
    private Modifier _shooterFireRateChange;
    /// <summary>
    /// Additive percentage modifier to bullet speed
    /// </summary>
    [Export]
    private Modifier _shooterBulletSpeedChange;
    /// <summary>
    /// Additive percentage modifier to bullet knockback
    /// </summary>
    [Export]
    private Modifier _shooterBulletKnockbackChange;
        
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
    /// Additive percentage modifier to laser's range
    /// </summary>
    [ExportGroup("Laser")]
    [Export]
    private Modifier _laserRangeChange;
    /// <summary>
    /// Additive percentage modifier to laser's damage
    /// </summary>
    [Export]
    private Modifier _laserDamageChange;
    /// <summary>
    /// Multiplicative percentage modifier to laser's laser duration
    /// </summary>
    [Export]
    private Modifier _laserLaserDuration = new(0f, Operation.Multiplicative);
        
    /// <summary>
    /// Additive percentage modifier to choker's range
    /// </summary>
    [ExportGroup("Choker")]
    [Export]
    private Modifier _chokerRangeChange;
    /// <summary>
    /// Additive percentage modifier to choker's damage
    /// </summary>
    [Export]
    private Modifier _chokerDamageChange;
    /// <summary>
    /// Additive percentage modifier to choker's fire rate
    /// </summary>
    [Export]
    private Modifier _chokerFireRateChange;
    /// <summary>
    /// Multiplicative percentage modifier to choker's part spread
    /// </summary>
    [Export]
    private Modifier _chokerPartSpreadChange = new(0f, Operation.Multiplicative);
        
    /// <summary>
    /// Modifies a turret's stats
    /// </summary>
    /// <param name="damager">The turret's stats to modify</param>
    public override void AddModule(Damager damager)
    {
        switch (damager)
        {
            // Modify the Shooter's stats
            case Shooter:
                damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _shooterDamageChange);
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _shooterRangeChange);
                damager.Stats[AttributeType.FireRate].Add(GetSceneUniqueId(), _shooterFireRateChange);
                damager.OnShoot += OnShoot;
                break;
            // Modify the Lancer's stats
            case Lancer:
                damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _lancerDamageChange);
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _lancerRangeChange);
                damager.Stats[AttributeType.FireRate].Add(GetSceneUniqueId(), _lancerFireRateChange);
                damager.Stats[AttributeType.BulletRange].Add(GetSceneUniqueId(), _lancerArrowRangeChange);
                damager.OnShoot += OnShoot;
                break;
            // Modify the Laser's stats
            case Laser:
                damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _laserDamageChange);
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _laserRangeChange);
                damager.Stats[AttributeType.LaserDuration].Add(GetSceneUniqueId(), _laserLaserDuration);
                break;
            // Modify the Choker's stats
            case Choker:
                damager.Stats[AttributeType.Damage].Add(GetSceneUniqueId(), _chokerDamageChange);
                damager.Stats[AttributeType.Range].Add(GetSceneUniqueId(), _chokerRangeChange);
                damager.Stats[AttributeType.FireRate].Add(GetSceneUniqueId(), _chokerFireRateChange);
                damager.Stats[AttributeType.PartSpread].Add(GetSceneUniqueId(), _chokerPartSpreadChange);
                break;
        }
    }
        
    /// <summary>
    /// Removes any stats modifications from the module
    /// </summary>
    /// <param name="damager">The turret to remove the modifications from</param>
    /// <exception cref="ArgumentOutOfRangeException">An invalid turret</exception>
    public override void RemoveModule(Damager damager)
    {
        damager.Stats[AttributeType.Damage].Remove(GetSceneUniqueId());
        damager.Stats[AttributeType.Range].Remove(GetSceneUniqueId());
        switch (damager)
        {
            // Modify the Shooter's stats
            case Shooter:
                damager.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId());
                damager.OnShoot -= OnShoot;
                break;
            // Modify the Lancer's stats
            case Lancer:
                damager.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId());
                damager.Stats[AttributeType.BulletRange].Remove(GetSceneUniqueId());
                damager.OnShoot -= OnShoot;
                break;
            // Modify the Laser's stats
            case Laser:
                damager.Stats[AttributeType.LaserDuration].Remove(GetSceneUniqueId());
                break;
            // Modify the Choker's stats
            case Choker:
                damager.Stats[AttributeType.FireRate].Remove(GetSceneUniqueId());
                damager.Stats[AttributeType.PartSpread].Remove(GetSceneUniqueId());
                break;
        }
    }

    /// <summary>
    /// Applies stat modifications when the bullet when fired
    /// </summary>
    /// <param name="bullet">The bullet to modify</param>
    private void OnShoot(Bullet bullet)
    {
        switch (bullet.Source)
        {
            case Shooter:
                bullet.Stats[AttributeType.Speed].Add(GetSceneUniqueId(), _shooterBulletSpeedChange);
                bullet.Stats[AttributeType.Knockback].Add(GetSceneUniqueId(), _shooterBulletKnockbackChange);
                break;
            case Lancer:
                bullet.Stats[AttributeType.Speed].Add(GetSceneUniqueId(), _lancerArrowSpeedChange);
                bullet.Stats[AttributeType.Knockback].Add(GetSceneUniqueId(), _lancerArrowKnockbackChange);
                break;
        }
    }
}