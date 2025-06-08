using System.Collections.Generic;
using Abstract;
using Abstract.Attributes;
using Godot;

namespace Enemies;

[GlobalClass]
public partial class EnemyStats : Resource
{
    [Export]
    public Attributes Attributes = new(
        new Godot.Collections.Dictionary<AttributeType, Attribute> { 
            [AttributeType.Speed] = new(AttributeType.Speed, 2f, min:0.8f),
            [AttributeType.MaxHealth] = new(AttributeType.MaxHealth, 20f),
        });

    [ExportGroup("Visuals")]
    [Export]
    public Texture2D Sprite;
    
    /// <summary>
    /// The amount of money to grant the player when the enemy is killed
    /// </summary>
    [ExportGroup("Death Stats")]
    [Export]
    public int DeathMoney = 10;
    /// <summary>
    /// The amount of lives lost if the enemy finishes the path
    /// </summary>
    [Export]
    public int DeathLives = 1;
    /// <summary>
    /// The amount of money to grant the player if the enemy finishes the path
    /// </summary>
    [Export]
    public int EndPathMoney = 10;
    
    /// <summary>
    /// If the enemy is a boss
    /// </summary>
    [ExportGroup("Bosses")]
    [Export]
    // TODO - Maybe put in immunities?
    public bool IsBoss;
    
    /// <summary>
    /// If the enemy rotates towards the next waypoint
    /// </summary>
    [ExportGroup("Movement")]
    [Export]
    public bool DoesRotation = true;
    /// <summary>
    /// The particle effect prefab to spawn when the enemy dies
    /// </summary>
    [Export]
    public PackedScene DeathEffect;
    /// <summary>
    /// The radius of a circle the bullet can collide with this target
    /// </summary>
    [Export]
    public float HitboxSize = 0.25f;
    /// <summary>
    /// The distance from enemy to waypoint before it's considered reached
    /// </summary>
    [Export] 
    public float DistanceToWaypoint = 0.05f;
    
    /// <summary>
    /// A list of the effect names (internal names) that the enemy is immune to
    /// 
    /// During runtime, also contains any unique effects applied to the enemy as they are immune to it
    /// </summary>
    [ExportGroup("Effect Immunities")]
    // TODO - List Export
    // [Export]
    public List<string> UniqueEffects;
    public readonly Dictionary<string, EnemyEffect> ActiveEffects = new();
}