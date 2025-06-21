using System.Collections.Generic;
using System.Linq;
using BestagonDefense.Abstract.Attributes;
using BestagonDefense.Abstract.Data;
using BestagonDefense.Enemies;
using Godot;

namespace BestagonDefense.Turrets;

public abstract partial class Damager : PlacedObject
{
    [Export]
    public Attributes Stats = new(
        new Godot.Collections.Dictionary<AttributeType, Attribute> { 
            [AttributeType.Damage] = new(AttributeType.Damage, 100f, min:0f),
        });
        
    /// <summary>
    /// Which modules the turret has applied
    /// </summary>
    // TODO - Likely need a tool to manage this
    public List<ModuleChainHandler> ModuleHandlers = [];
        
    // Events
    public delegate void AttackEvent(Damager damager);
    public delegate void ShootEvent(Bullet bullet);
    public delegate void HitEvent(Enemy target, Damager damager, Bullet bullet = null);

    public event AttackEvent OnAttack;
    public event ShootEvent OnShoot;
    public event HitEvent OnHit;

    /// <summary>
    /// Handles an attack for the object
    /// </summary>
    /// <param name="delta">The time since last frame (in seconds)</param>
    protected abstract void Attack(float delta);

    /// <summary>
    /// Turret types will override this as attack type will be different for each turret
    /// </summary>
    protected void Attack(Damager damager)
    {
        OnAttack?.Invoke(damager);
    }
        
    /// <summary>
    /// Turret types will override this as attack type will be different for each turret
    /// </summary>
    protected void Shoot(Bullet bullet)
    {
        OnShoot?.Invoke(bullet);
    }

    /// <summary>
    /// Perform attack on many targets
    /// </summary>
    /// <param name="targets">The targets to hit</param>
    /// <param name="damager">The object performing the attack</param>
    /// <param name="bullet">The bullet (if any) that hit</param>
    public void HitMany(IEnumerable<Enemy> targets, Damager damager, Bullet bullet = null)
    {
        foreach (Enemy target in targets)
        {
            OnHit?.Invoke(target, damager, bullet);
        }
    }

    /// <summary>
    /// Perform an attack on an enemy
    /// </summary>
    /// <param name="target">The target to hit</param>
    /// <param name="damager">The object performing the attack</param>
    /// <param name="bullet">The bullet (if any) that hit</param>
    public void Hit(Enemy target, Damager damager, Bullet bullet = null)
    {
        OnHit?.Invoke(target, damager, bullet);
    }
        
    /// <summary>
    /// Adds Modules to our turret after checking they're valid.
    /// </summary>
    /// <param name="handler">The ModuleChainHandler to apply to the turret</param>
    /// <returns>true If the Module was applied successfully</returns>
    public virtual bool AddModule(ModuleChainHandler handler)
    {
        if (!handler.ValidModule(this))
        {
            return false;
        }
            
        // Checks if the module is unique
        // Then if there is a module of the same type but different tier,
        // it cannot be upgraded
        if (handler.GetChain().Unique && 
            (ModuleHandlers.Any(x => x.GetModule().GetType() == handler.GetModule().GetType() &&
                                     !handler.CanUpgrade(x.GetTier()))))
        {
            return false;
        }

        handler = CalculateUpgrades(handler);
        // TODO - Duplicate handler
        ModuleHandlers.Add(handler);
        handler.GetModule().AddModule(this);
            
        return true;
    }
        
    /// <summary>
    /// Removes a module from the turret
    /// </summary>
    /// <param name="handler">The handler of the module to remove</param>
    protected virtual void RemoveModule(ModuleChainHandler handler)
    {
        ModuleHandlers.Remove(handler);
        handler.GetModule().RemoveModule(this);
    }
        
    /// <summary>
    /// Performs any module upgrades that are possible with the addition of a new handler
    /// </summary>
    /// <param name="handler">The handler to check for upgrades against</param>
    private ModuleChainHandler CalculateUpgrades(ModuleChainHandler handler)
    {
        var i = 0;
        while (i < ModuleHandlers.Count)
        {
            bool canUpgrade = handler.Upgrade(ModuleHandlers[i]);
            if (canUpgrade)
            {
                RemoveModule(ModuleHandlers[i]);
                handler = CalculateUpgrades(handler);
                break;
            }

            i++;
        }

        return handler;
    }
}