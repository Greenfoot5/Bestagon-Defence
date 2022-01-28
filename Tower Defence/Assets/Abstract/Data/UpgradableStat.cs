using System;

namespace Abstract.Data
{
    /// <summary>
    /// A stat that is upgradable.
    /// This allows for additive upgrades in an easy manner
    /// </summary>
    [Serializable]
    public struct UpgradableStat
    {
        public float stat;
        public float modifier;

        public UpgradableStat(float baseValue)
        {
            stat = baseValue;
            modifier = 1f;
        }
    
        /// <summary>
        /// Gets the base value of the stat.
        /// If the value is less than 0, it will return 0
        /// </summary>
        /// <returns>The base value of the stat</returns>
        public float GetBase()
        {
            return stat <= 0f ? 0f : stat;
        }
    
    
        /// <summary>
        /// Gets the stat after it's been modified
        /// If the value is less than 0, it will return 0
        /// </summary>
        /// <returns>The stat after being multiplied by the modifier</returns>
        public float GetStat()
        {
            return stat * modifier <= 0f ? 0f : stat * modifier;
        }
    
        /// <summary>
        /// Gets the stat after it's been modified
        /// If the value is less than 0, it will still return it's actual value
        /// </summary>
        /// <returns>The stat after being multiplied by the modifier</returns>
        public float GetTrueStat()
        {
            return stat * modifier;
        }
        
        /// <summary>
        /// Increases the stat's modifier by a certain amount
        /// </summary>
        /// <param name="newValue">How much increase the modifier by</param>
        public void AddModifier(float newValue)
        {
            modifier += newValue;
        }

        /// <summary>
        /// Decreases the stat's modifier by a certain amount
        /// </summary>
        /// <param name="oldValue">How much decrease the modifier by</param>
        public void TakeModifier(float oldValue)
        {
            modifier -= oldValue;
        }
        
        /// <summary>
        /// Gets a nice pretty version of our stat in a readable format.
        /// Formatted to 1 d.p.
        /// </summary>
        /// <returns>The stats as a formatted string</returns>
        public override string ToString()
        {
            return $"{modifier * stat:#,##0.#}";
        }
    }
}
