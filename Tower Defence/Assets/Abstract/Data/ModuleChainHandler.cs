using System;
using Modules;
using UI.Nodes;
using UnityEngine;

namespace Abstract.Data
{
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
        /// Upgrade if we can,
        /// </summary>
        /// <param name="siblingTier"></param>
        /// <returns></returns>
        public bool Upgrade(int siblingTier)
        {
            if (siblingTier == tier)
            {
                if (!chain.PerformUpgrade(tier)) return false;
                
                Debug.Log("Increasing Tier");
                tier += 1;
                return true;
            }

            Debug.LogError("Cannot upgrade modules of different tiers! " +
                           "Current tier: " + tier + "; Sibling tier: " + siblingTier + "; Module: " + chain.displayName, chain);
            return false;
        }

        public Module GetModule()
        {
            return chain.GetTier(tier);
        }

        public ModuleChain GetChain()
        {
            return chain;
        }

        public int GetTier()
        {
            return tier;
        }
        
        public string GetTierDisplay()
        {
            // The tier supplied is the array index, it might not be the actual tier.
            return Levels[chain.GetTier(tier).moduleTier];
        }
    }
}