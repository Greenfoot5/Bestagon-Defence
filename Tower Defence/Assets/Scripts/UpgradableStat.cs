using System;

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

    public void AddModifier(float newValue)
    {
        modifier += newValue;
    }

    public void TakeModifier(float oldValue)
    {
        modifier -= oldValue;
    }

    public override string ToString()
    {
        return $"{modifier * stat:#,##0.#}";
    }
}
