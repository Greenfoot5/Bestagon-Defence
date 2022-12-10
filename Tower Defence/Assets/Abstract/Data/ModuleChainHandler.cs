using System;
using Modules;
using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// A handle between a ModuleChain (SO) and a GameObject
    /// As an SO cannot be instantiated, it cannot store the tier variable
    /// Also handles a few other useful things
    /// </summary>
    [Serializable]
    public struct ModuleChainHandler
    {
        // The display levels of the modules
        private static readonly string[] Levels =
        {
            "-", "I", "II", "III", "IV", "V",
            "VI", "VII", "VIII", "IX", "X",
            "XI", "XII", "XIII", "XIV", "XV",
            "XVI", "XVII", "XVIII", "XIX", "XX"
        };
        
        [SerializeField]
        private ModuleChain chain;
        [SerializeField]
        private int tier;

        /// <summary>
        /// Upgrade if we can.
        /// </summary>
        /// <param name="siblingTier">The tier we're trying to upgrade to</param>
        /// <returns>True if the module was upgraded</returns>
        public bool Upgrade(int siblingTier)
        {
            // If the upgrade is of a different tier, 
            if (siblingTier != tier || !chain.CanUpgrade(tier)) return false;
                
            tier += 1;
            return true;
        }
        
        /// <summary>
        /// Gets the module in the chain of the current tier
        /// </summary>
        /// <returns>The current module that's being handled</returns>
        public Module GetModule()
        {
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
            return Levels[chain.GetModule(tier).moduleTier];
        }
        
        /// <summary>
        /// Gets the name of the current module and it's tier
        /// </summary>
        /// <returns>Name & tier in roman numerals</returns>
        public string GetDisplayName()
        {
            return chain.displayName + " " + GetTierDisplay();
        }
    }
}