using System.Collections.Generic;
using System.Linq;
using Turrets;
using UnityEngine;
using UnityEngine.UI;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField]
    private string upgradeType;

    [Range(0f, 1f)]
    [SerializeField]
    private float effectPercentage;
    [SerializeField]
    private int upgradeTier;
        
    // TODO - Generate display name from update type and tier
    public string displayName;
    public Sprite icon;
    public string effectText;
    public TurretType[] validTypes;

    public bool ValidUpgrade(Turret turret)
    {
        return validTypes.Contains(turret.attackType);
    }

    public abstract void AddUpgrade(Turret turret);

    public abstract void RemoveUpgrade(Turret turret);

    public abstract void OnShoot(Bullet bullet);

    public abstract void OnHit(IEnumerable<Enemy> targets);

    protected float GETUpgradeValue()
    {
        return effectPercentage;
    }
}