using UnityEngine;

namespace Modules
{
    /// <summary>
    /// A base abstract class to create a turret module from.
    /// </summary>
    [CreateAssetMenu(fileName = "ModuleChain", menuName = "Module Chain")]
    public class ModuleChain : ScriptableObject
    {
        // The display levels of the modules
        private static readonly string[] Levels =
        {
            "-", "I", "II", "III", "IV", "V",
            "VI", "VII", "VIII", "IX", "X",
            "XI", "XII", "XIII", "XIV", "XV",
            "XVI", "XVII", "XVIII", "XIX", "XX"
        };
        
        [Tooltip("The tier number of the module")]
        public Module[] moduleTiers;
        
        [Tooltip("The main colour of the module, is displayed in various ways")]
        public Color accentColor;
        [Tooltip("The name to display for the module, should not include the tier in Roman numerals")]
        public string displayName;
        [Tooltip("The tagline of the module. It's not a description, just a witty little remark")]
        public string tagline;
        [Tooltip("The module's icon")]
        public Sprite icon;
        [Multiline(3)]
        [Tooltip("The description of the module and what it does")]
        public string effectText;

        public bool PerformUpgrade(int tier)
        {
            if (tier == moduleTiers.Length)
            {
                return false;
            }

            Module currentTier = moduleTiers[tier];
            Module nextTier = moduleTiers[tier + 1];
            // Check the next tier isn't skipped and can be upgraded to
            return currentTier.moduleTier + 1 == nextTier.moduleTier
                   && nextTier.upgradable;
        }

        public string GetTierDisplay(int tier)
        {
            return Levels[tier + 1];
        }
    }
}