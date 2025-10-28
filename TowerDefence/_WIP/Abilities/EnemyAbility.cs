using Godot;

namespace BestagonDefence._WIP.Abilities;

/// <summary>
/// The base for any enemy ability
/// </summary>
public abstract partial class EnemyAbility : Resource
{
    /// <summary>
    /// What should the ability target?
    /// </summary>
    [Export]
    public AbilityTarget targetingType = AbilityTarget.Single;
    /// <summary>
    /// What should trigger the ability?
    /// </summary>
    // [Export(PropertyHint.ArrayType)]
    public AbilityTrigger[] triggers;
        
    /// <summary>
    /// The icon to place above the enemy's health bar
    /// </summary>
    [Export]
    public Texture2D abilityIcon;
    /// <summary>
    /// The particle effect to play when activating the ability
    /// </summary>
    public PackedScene abilityEffect;
        
    /// <summary>
    /// The range of the ability (if using radius targeting)
    /// </summary>
    [ExportGroup("Radius Stats")]
    [Export]
    public float range = 3f;

    /// <summary>
    /// The duration of the ability (if applicable)
    /// </summary>
    [ExportGroup("Timer Stats")]
    [Export]
    public float timer = 5f;

    /// <summary>
    /// Set as -1 to not use timer
    /// </summary>
    [ExportGroup("Counter")]
    [Export]
    public float startCount = -1f;
        
    /// <summary>
    /// Activates the ability
    /// </summary>
    /// <param name="target">What the ability is activated on</param>
    public abstract void Activate(GodotObject target);
        
    /// <summary>
    /// Ends the ability when the timer ends
    /// </summary>
    /// <param name="target">What to clean up the ability on</param>
    public abstract void OnCounterEnd(GodotObject target);
}