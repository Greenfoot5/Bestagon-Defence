using System;
using BestagonDefense.Modules;
using BestagonDefense.Turrets;
using Godot;

namespace BestagonDefense.Abstract.Data;

/// <summary>
/// A handle between a ModuleChain (Resource) and a GodotObject
/// As a Resource cannot be instantiated, it cannot store the tier variable
/// Also handles a few other useful things
/// </summary>
[Tool]
[GlobalClass]
public partial class ModuleChainHandler : Resource, IEquatable<ModuleChainHandler>, ISubtypable
{
    // The display levels of the modules
    private static readonly string[] Levels =
    [
        "-", "I", "II", "III", "IV", "V",
        "VI", "VII", "VIII", "IX", "X",
        "XI", "XII", "XIII", "XIV", "XV",
        "XVI", "XVII", "XVIII", "XIX", "XX"
    ];
        
    /// <summary>
    /// The module chain to handle
    /// </summary>
    [Export]
    private ModuleChain chain;
    /// <summary>
    /// The tier of module being handled from the chain
    /// </summary>
    [Export]
    private int tier;

    /// <summary>
    /// Creates a new ModuleChainHandler with the specified values
    /// </summary>
    /// <param name="chain">The ModuleChain to use in the handler</param>
    /// <param name="tier">The tier of the ModuleChain the handler represents</param>
    public ModuleChainHandler(ModuleChain chain, int tier)
    {
        this.chain = chain;
        this.tier = tier;
    }

    /// <summary>
    /// Creates an empty ModuleChainHandler
    /// </summary>
    public ModuleChainHandler()
    {
        tier = 1;
    }

    /// <summary>
    /// Upgrade if we can.
    /// </summary>
    /// <param name="sibling">The Chain Handler we're trying to upgrade to</param>
    /// <returns>True if the module was upgraded</returns>
    public bool Upgrade(ModuleChainHandler sibling)
    {
        // If the upgrade is of a different type,
        if (sibling.GetModule().GetType() != GetModule().GetType()) return false;
        // If the upgrade is of a different tier, 
        if (!CanUpgrade(sibling.GetTier())) return false;
                
        tier += 1;
        return true;
    }

    /// <summary>
    /// Checks if the module can be upgraded from its current tier
    /// </summary>
    /// <param name="otherTier">The tier of the other module this module is trying to upgrade with</param>
    /// <returns>If the module can be upgraded</returns>
    public bool CanUpgrade(int otherTier)
    {
        return otherTier == tier && chain.CanUpgrade(otherTier);
    }
        
    /// <summary>
    /// Gets the module in the chain of the current tier
    /// </summary>
    /// <returns>The current module that's being handled</returns>
    public Module GetModule()
    {
        if (chain == null || tier == 0)
        {
            GD.PushWarning("Attempt to obtain an invalid module");
            return null;
        }
        return chain.GetModule(tier);
    }
        
    /// <summary>
    /// Gets the chain the handler is handling
    /// </summary>
    /// <returns>The ModuleChain of the handler</returns>
    public ModuleChain GetChain()
    {
        return chain;
    }
        
    /// <summary>
    /// Gets the current tier of the handler
    /// </summary>
    /// <returns>The current tier</returns>
    public int GetTier()
    {
        return tier;
    }
        
    /// <summary>
    /// Gets the current tier in roman numerals
    /// </summary>
    /// <returns>Roman numerals for the tier</returns>
    public string GetTierDisplay()
    {
        // The tier supplied is the array index, it might not be the actual tier.
        return Levels[chain.GetModule(tier).ModuleTier];
    }
        
    /// <summary>
    /// Gets the name of the current module and it's tier
    /// </summary>
    /// <returns>Name & tier in roman numerals</returns>
    public string GetDisplayName()
    {
        return chain.DisplayName + " " + GetTierDisplay();
    }
    
    /// <summary>
    /// Checks if the Damager is of a valid type for the Module
    /// </summary>
    /// <param name="damager">The Damager to check for</param>
    /// <returns>True if the Module can be applied to the Damager</returns>
    public bool ValidModule(Damager damager)
    {
        return chain.GetModule(tier).ValidModule(damager);
    }

    /// <summary>
    /// Checks if another ModuleChainHandler is the same as this
    /// </summary>
    /// <param name="other">The handler to check against</param>
    /// <returns>true if the handlers have the same chain at the same tier</returns>
    public bool Equals(ModuleChainHandler other)
    {
        return other != null && Equals(chain, other.chain) && tier == other.tier;
    }

    /// <summary>
    /// Checks if an object is the same as this ModuleChainHandler
    /// </summary>
    /// <param name="obj">The object to check against</param>
    /// <returns>true if both are handlers and the handlers are equal</returns>
    public override bool Equals(object obj)
    {
        return obj is ModuleChainHandler other && Equals(other);
    }

    /// <summary>
    /// Hashes the current state of the ModuleChainHandler
    /// </summary>
    /// <returns>A hash of the object's current state</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(chain, tier);
    }

    /// <summary>
    /// Converts the chain to a readable string
    /// </summary>
    /// <returns>A readable string with the handler's tier</returns>
    public override string ToString()
    {
        return GetModule().GetName() + " (" + GetTierDisplay() + ")";
    }
    
    /// <summary>
    /// Get the handler's module type
    /// </summary>
    /// <returns>The Type of the handler's module chain module</returns>
    public Type GetSubtype()
    {
        return GetModule().GetType();
    }
}