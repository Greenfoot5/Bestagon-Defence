using System;
using Modules;
using UI.Nodes;
using UnityEngine;

namespace Abstract.Data
{
    [Serializable]
    public struct ModuleChainHandler
    {
        [SerializeField]
        private ModuleChain chain;
        [SerializeField]
        private int tier;

        /// <summary>
        /// Upgrade if we can,
        /// </summary>
        /// <param name="siblingTier"></param>
        /// <returns></returns>
        public bool CanUpgrade(int siblingTier)
        {
            if (siblingTier == tier)
            {
                if (!chain.PerformUpgrade(tier)) return false;
                
                tier += 1;
                NodeUI.instance.UpdateModules();
                NodeUI.instance.UpdateStats();
                return true;
            }

            Debug.LogError("Cannot upgrade modules of different tiers! " +
                           "Current tier: " + tier + "; Sibling tier: " + siblingTier + "; Module: " + chain.displayName, chain);
            return false;
        }
    }
}