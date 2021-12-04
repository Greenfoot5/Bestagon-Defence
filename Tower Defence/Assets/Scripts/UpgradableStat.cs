using System;

[Serializable]
public struct UpgradableStat
{
    private float _stat;
    private float _modifier;

    public UpgradableStat(float baseValue)
    {
        _stat = baseValue;
        _modifier = 0f;
    }

    public float GetBase()
    {
        return _stat;
    }

    public float GetStat()
    {
        return _stat * _modifier;
    }

    public void AddModifier(float newValue)
    {
        _modifier += newValue;
    }

    public void TakeModifier(float oldValue)
    {
        _modifier -= oldValue;
    }
}
