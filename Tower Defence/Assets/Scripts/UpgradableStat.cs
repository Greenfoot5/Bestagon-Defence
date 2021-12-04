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

    public float GetBase()
    {
        return stat;
    }

    public float GetStat()
    {
        return stat * modifier;
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
