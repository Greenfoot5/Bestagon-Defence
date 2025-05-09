using System;
using Godot;
using Modules;
using Turrets;

namespace Abstract.Data;

/// <summary>
/// A handle between a ModuleChain (Resource) and a GodotObject
/// As a Resource cannot be instantiated, it cannot store the tier variable
/// Also handles a few other useful things
/// </summary>
[Serializable]
[Tool]
[GlobalClass]
public partial class ModuleChainHandler : Resource, IEquatable<ModuleChainHandler>, ISubtypeable
{
    // The display levels of the modules
    private static readonly string[] Levels =
    {
        "-", "I", "II", "III", "IV", "V",
        "VI", "VII", "VIII", "IX", "X",
        "XI", "XII", "XIII", "XIV", "XV",
        "XVI", "XVII", "XVIII", "XIX", "XX"
    };
        
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

    public ModuleChainHandler(ModuleChain chain, int tier)
    {
        this.chain = chain;
        this.tier = tier;
    }

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
    /// Checks if the module can be upgraded from it's current tier
    /// </summary>
    /// <param name="otherTier">The tier of the other module this module is trying to upgrade with</param>
    /// <returns></returns>
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
        
    public bool ValidModule(Damager damager)
    {
        return chain.GetModule(tier).ValidModule(damager);
    }

    public bool Equals(ModuleChainHandler other)
    {
        return Equals(chain, other.chain) && tier == other.tier;
    }

    public override bool Equals(object obj)
    {
        return obj is ModuleChainHandler other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(chain, tier);
    }

    public override string ToString()
    {
        return GetModule().GetName() + " (" + GetTierDisplay() + ")";
    }
        
    public Type GetSubtype()
    {
        return GetModule().GetType();
    }
}