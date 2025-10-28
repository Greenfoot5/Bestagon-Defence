namespace BestagonDefence.Abstract.Attributes;

/// <summary>
/// A collection of attribute types (or stats) available
/// </summary>
public enum AttributeType
{
    /// <summary>
    /// Will modify nothing
    /// </summary>
    Nil, // = 0
    
    /// <summary>
    /// How far a bullet can travel
    /// </summary>
    BulletRange,
    /// <summary>
    /// How much damage the object deals
    /// </summary>
    Damage,
    /// <summary>
    /// How fast (per second) the object fires/attacks
    /// </summary>
    FireRate,
    /// <summary>
    /// The explosion radius of the object. 1 = 20px radius
    /// </summary>
    ExplosionRadius,
    /// <summary>
    /// How much knockback an attack deals
    /// </summary>
    Knockback,
    
    // = 5
    
    /// <summary>
    /// How long (in seconds) a knockback effect lasts
    /// </summary>
    KnockbackDuration,
    /// <summary>
    /// % reduction to knockback effects (strength, not duration)
    /// </summary>
    KnockbackResistance,
    /// <summary>
    /// How long the laser will fire for
    /// </summary>
    LaserDuration,
    /// <summary>
    /// How long to wait before the laser can be fired again (in seconds)
    /// </summary>
    LaserCooldown,
    /// <summary>
    /// How long an object will last (in seconds)
    /// </summary>
    Lifetime,
    
    // = 10
    
    /// <summary>
    /// The maximum health of an object
    /// </summary>
    MaxHealth,
    /// <summary>
    /// How many parts (bullets) to fire in a shot
    /// </summary>
    PartCount,
    /// <summary>
    /// How wide (in radians) the parts (bullets) are spread out
    /// </summary>
    PartSpread,
    /// <summary>
    /// How far the object can see/attack targets from, 1 = 20px
    /// </summary>
    Range,
    /// <summary>
    /// How fast (in radians/second) an object rotates
    /// </summary>
    RotationSpeed,
    /// <summary>
    /// How many seekers (objects) an object can have active at once
    /// Currently not used
    /// </summary>
    SeekerCount,
    
    // 15
    
    /// <summary>
    /// How fast an object moves (not rotation)
    /// </summary>
    Speed,
    /// <summary>
    /// How fast an object's fire rate decreases when it's not firing
    /// </summary>
    SpinCooldown,
    /// <summary>
    /// How fast an object's fire rate increases when it fires
    /// </summary>
    SpinMultiplier,
}