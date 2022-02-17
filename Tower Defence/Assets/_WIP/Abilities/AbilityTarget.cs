namespace _WIP.Abilities
{
    /// <summary>
    /// Specifies what targets are in range
    /// </summary>
    public enum AbilityTarget
    {
        All,
        Radius,
        Single
    }
    
    /// <summary>
    ///  Specifies what can trigger an ability
    /// </summary>
    public enum AbilityTrigger
    {
        OnEnd,
        OnDeath,
        OnTimer,
        OnDamage,
        OnGrant
    }
}
