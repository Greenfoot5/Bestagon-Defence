namespace Abstract.Attributes;

public enum AttributeType
{
    /// <summary>
    /// Will modify nothing
    /// </summary>
    Nil, // = 0
    
    BulletRange,
    Damage,
    FireRate,
    ExplosionRadius,
    Knockback, // = 5
    KnockbackDuration,
    KnockbackResistance,
    LaserDuration,
    LaserCooldown,
    Lifetime, // = 10
    MaxHealth,
    PartCount,
    PartSpread,
    Range,
    RotationSpeed, // = 15
    SeekerCount,
    Speed,
    SpinCooldown,
    SpinMultiplier,
}