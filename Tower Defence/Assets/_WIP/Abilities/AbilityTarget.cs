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
        // ReSharper disable InconsistentNaming
        OnEnd,
        OnDeath,
        OnTimer,
        OnDamage,
        OnGrant
        // ReSharper restore InconsistentNaming
    }
}
