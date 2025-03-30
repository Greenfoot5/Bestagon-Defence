using Godot;

namespace Modules
{
    /// <summary>
    /// A chain of modules, allowing traversal up or down the chain
    /// </summary>
    [GlobalClass]
    public partial class ModuleChain : Resource
    {
        /// <summary>
        /// The tiers of the module
        /// </summary>
        [Export]
        public Module[] moduleTiers;

        /// <summary>
        /// If different tiers of the module can be applied to the same turret
        /// </summary>
        [Export]
        public bool unique;
        
        // Display
        /// <summary>
        /// The main colour of the module
        /// </summary>
        [Export]
        public Color accentColor = new(0, 0, 0);
        /// <summary>
        /// The module's icon
        /// </summary>
        [Export]
        public Texture2D icon;
        
        /// <summary>
        /// The name to display for the module, should not include the tier
        /// </summary>
        [ExportGroup("Text")]
        [Export]
        public string displayName;
        /// <summary>
        /// The tagline of the module. It's not a description, just a witty little remark
        /// </summary>
        [Export]
        public string tagline;
        /// <summary>
        /// The description of the module and what it does
        /// </summary>
        [Export]
        public string description;
        
        /// <summary>
        /// Checks if a module at a tier can be upgraded
        /// </summary>
        /// <param name="tier">The tier to check against</param>
        /// <returns>If the module can be upgraded</returns>
        public bool CanUpgrade(int tier)
        {
            if (tier >= moduleTiers.Length)
            {
                return false;
            }
            
            Module currentTier = moduleTiers[tier - 1];
            Module nextTier = moduleTiers[tier];
            // Check the next tier isn't skipped and can be upgraded to
            return currentTier.moduleTier + 1 == nextTier.moduleTier
                   && nextTier.isUpgradableTo;
        }
        
        /// <summary>
        /// Returns a module given a provided tier
        /// </summary>
        /// <param name="tier">The tier of the module to return</param>
        /// <returns>The module of the provided tier</returns>
        public Module GetModule(int tier)
        {
            return moduleTiers[tier - 1];
        }
    }
}