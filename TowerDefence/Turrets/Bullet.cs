using System.Collections.Generic;
using BestagonDefence.Abstract;
using BestagonDefence.Abstract.Attributes;
using BestagonDefence.Enemies;
using Godot;

namespace BestagonDefence.Turrets;

/// <summary>
/// The bullet shot from a turret
/// </summary>
public partial class Bullet : Node2D
{
    /// <summary>
    /// The bullet's stats
    /// </summary>
    [Export]
    public Attributes Stats = new(
        new Godot.Collections.Dictionary<AttributeType, Attribute> { 
            [AttributeType.Speed] = new(AttributeType.Speed, 30f),
            [AttributeType.RotationSpeed] = new(AttributeType.RotationSpeed, 2.3f),
            [AttributeType.ExplosionRadius] = new(AttributeType.ExplosionRadius, 1f, min:0f),
            [AttributeType.Knockback] = new(AttributeType.Knockback, 1f, min:0f),
        });
    
    /// <summary>
    /// The area where the bullet collides with enemies
    /// </summary>
    [Export]
    public Area2D Area;
        
    /// <summary>
    /// Hits all enemies on path
    /// </summary>
    [ExportGroup("Types")]
    [Export]
    public bool IsEthereal;
    /// <summary>
    /// Hits the first enemy it touches, rather than just target
    /// </summary>
    [Export]
    public bool WillHitFirst;
    
    /// <summary>
    /// The effect spawned when the bullet hits a target
    /// </summary>
    [Export]
    private PackedScene _impactEffect;
    /// <summary>
    /// Explosion effect
    /// </summary>
    [Export]
    private PackedScene _explodeEffect;
    
    /// <summary>
    /// The trail
    /// </summary>
    [ExportGroup("Trail")]
    [Export]
    protected Line2D Line;
    /// <summary>
    /// How many points there should be
    /// </summary>
    [Export]
    private int maxPoints = 5;
    /// <summary>
    /// How far apart the points should be
    /// </summary>
    [Export]
    private float pointSpacing = 20;
    /// <summary>
    /// Current distance from last line point
    /// </summary>
    private float distance;
    
    // <summary>
    // The explosion Area
    // </summary>
    [ExportGroup("Explosion")]
    [Export]
    private Area2D explodeArea;
    
    public Turret Source;
    public Enemy Target;
    public Vector2 TargetLocation;
    public bool UseLocation;
    
    /// <summary>
    /// Which enemies have been hit so far, so they don't get hit multiple times
    /// </summary>
    private readonly List<ulong> _hitEnemies = [];
    private ulong deathTime;
    private bool isDead;

    /// <summary>
    /// Creates the bullet
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        
        explodeArea.Scale *= Stats[AttributeType.ExplosionRadius].Value;
        Area.AreaEntered += OnAreaEntered;
    }

    /// <summary>
    /// Removes the listeners when the bullet dies
    /// </summary>
    public override void _ExitTree()
    {
        Area.AreaEntered -= OnAreaEntered;
    }

    /// <summary>
    /// Sets the new transform the bullet shoot go towards
    /// </summary>
    /// <param name="newTarget">The transform of the new target</param>
    /// <param name="turret">The turret telling the bullet to seek a target</param>
    public void Seek(Enemy newTarget, Turret turret)
    {
        Target = newTarget;
        Source = turret;
        UseLocation = false;
        Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
    }
        
    /// <summary>
    /// Sets the location the bullet goes towards
    /// </summary>
    /// <param name="location">The location the bullet goes towards</param>
    /// <param name="turret">The turret telling the bullet to seek the location</param>
    public void Seek(Vector2 location, Turret turret)
    {
        TargetLocation = location;
        Source = turret;
        UseLocation = true;
        Stats[AttributeType.Damage] = new Attribute(turret.Stats[AttributeType.Damage]);
    }

    /// <summary>
    /// Moves the bullet towards the target and check if it hits
    /// </summary>
    public override void _Process(double delta)
    {
        if (isDead)
        {
            Die();
            return;
        }

        // Check the bullet still have a target to move towards
        if (!IsInstanceValid(Target) && !UseLocation)
        {
            QueueFree();
            return;
        }
            
        if (UseLocation)
            SeekTarget(TargetLocation, false, delta);
        else
            SeekTarget(Target.GlobalPosition, true, delta);

        if (!IsInstanceValid(Line))
            return;

        if (Line.GetPointCount() > 0)
        {
            Vector2 lastPoint = ToGlobal(Line.GetPointPosition(Line.GetPointCount() - 1));
            float additional = Area.GlobalPosition.DistanceTo(lastPoint);
            distance += additional;
        }
        else
        {
            distance = pointSpacing;
        }

        if (distance >= pointSpacing)
        {
            Line.AddPoint(ToLocal(Area.GlobalPosition));
            distance = 0.0f;
            if (Line.GetPointCount() > maxPoints)
                Line.RemovePoint(0);
        }
    }

    /// <summary>
    /// Moves the bullet towards a target location
    /// </summary>
    /// <param name="location">The location to move towards</param>
    /// <param name="isEnemy">If the location is an enemy</param>
    /// <param name="delta">The length of the frame</param>
    private void SeekTarget(Vector2 location, bool isEnemy, double delta)
    {
        LookAtTarget(location, delta);
            
        // Get the direction of the target, and the distance to move this frame
        var distanceThisFrame = (float)(Stats[AttributeType.Speed].Value * delta);
            
        // Move bullet towards target
        Area.GlobalPosition -= new Vector2(distanceThisFrame * Mathf.Sin(-Area.GlobalRotation), distanceThisFrame * Mathf.Cos(Area.GlobalRotation));
            
        Vector2 difference = location - Area.GlobalPosition;
        const float targetSize = 0.25f;
        // Has the bullet "hit" the target?
        if (!isEnemy && difference.LengthSquared() <= targetSize * targetSize)
        {
            HitTarget(false); 
        }
    }
        
    /// <summary>
    /// Rotates the bullet towards our target
    /// </summary>
    private void LookAtTarget(Vector2 location, double delta)
    {
        float rotationAngleNeed = Area.GetAngleTo(location) + float.Pi / 2;
            
        double zAngle = Mathf.Clamp(rotationAngleNeed, -Stats[AttributeType.RotationSpeed].Value * delta,
            Stats[AttributeType.RotationSpeed].Value * delta);
        Area.GlobalRotation += (float)zAngle;
    }

    /// <summary>
    /// Called when the bullet hits the target
    /// </summary>
    protected virtual void HitTarget(bool isEnemy, Enemy enemy = null)
    {
        enemy ??= Target;
            
        // TODO - Impact Effects
            
        // var effect = impactEffect.Instantiate<Node2D>();
        // effect.Name = "_" + effect.Name;
        // effect.Position = Position;
        // effect.Rotation = Rotation;
        //
        // GetTree().CreateTimer(2).Timeout += () => { effect.QueueFree(); };

        if (isEnemy)
        {
            // If the bullet has AoE damage or not
            if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                Explode();
            else
                Damage(enemy);
        }
        else
        {
            if (Stats[AttributeType.ExplosionRadius].Value > 0f)
                Explode();
        }

        // Destroy so the bullet only hits the target once
        Die();
    }

    
    /// <summary>
    /// Used to deal damage to a single enemy
    /// </summary>
    /// <param name="enemy">The enemy to deal damage to</param>
    protected void Damage(Enemy enemy)
    {
        if (enemy == null) return;
            
        Source.Hit(enemy, Source, this);

        if (Stats[AttributeType.Knockback].Value > 0)
        {
            // TODO - Get Movement
            enemy.TakeKnockback(Stats[AttributeType.Knockback].Value, Source.GlobalPosition);
        }

        enemy.TakeDamage(Stats[AttributeType.Damage].Value, this);
    }
    
    /// <summary>
    /// Used to deal damage to multiple enemies
    /// </summary>
    protected void Explode()
    {
        if (_explodeEffect is not null)
        {
            // Spawn explode effect
            var effect = _explodeEffect.Instantiate<DeathVfx>();
            effect.Name = "_" + effect.Name;
            effect.GlobalPosition = explodeArea.GlobalPosition;
            effect.GlobalRotation = explodeArea.GlobalRotation;
            // TODO - Not use ProcessMaterial.Scale as it applies to all
            ((ParticleProcessMaterial)effect.ProcessMaterial).Scale *= explodeArea.Scale;
            GetTree().GetRoot().AddChild(effect);
        }

        // Gets all the enemies in the AoE and calls Damage on them
        foreach (Area2D a in explodeArea.GetOverlappingAreas())
        {
            var enemy = (Enemy)a;
            Damage(enemy);
        }
    }
        
    /// <summary>
    /// Deals damage to hit enemies the first time when the bullet should
    /// </summary>
    /// <param name="col">The collider that was touched</param>
    private void OnAreaEntered(Area2D col)
    {
        if (col is not Enemy enemy) return;

        if (_hitEnemies.Contains(col.GetInstanceId())) return;
            
        _hitEnemies.Add(col.GetInstanceId());

        if (IsInstanceValid(enemy) && !UseLocation && Target.GetInstanceId() == col.GetInstanceId())
        {
            GD.Print("Hit without UseLocation");
            HitTarget(true, enemy);
            return;
        }

        if (IsEthereal || UseLocation)
        {
            GD.Print("Hit with UseLocation!");
            if (!WillHitFirst)
            {
                GD.Print("Hit without first");
                Damage(enemy);
            }
            else
            {
                GD.Print("Hit with first");
                HitTarget(true, enemy);
            }
        }
    }
    
    /// <summary>
    /// Handles the bullet ending its life
    /// </summary>
    protected void Die()
    {
        Area.Visible = false;
        Area.SetDeferred("monitoring", false);
        isDead = true;
        
        if (deathTime + 50 < Time.GetTicksMsec())
        {
            Line.Width *= 1f - (1f / Line.Points.Length);
            deathTime = Time.GetTicksMsec();
            Line.RemovePoint(0);
            if (Line.Points.Length <= 0)
            {
                QueueFree();
            }
        }
    }
}