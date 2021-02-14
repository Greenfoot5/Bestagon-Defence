using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    [Tooltip("The prefab to use when the turret is built.")]
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;
    
    [Range(0, 1)]
    [Tooltip("The percentage reduction to apply to the price when selling")]
    public float sellReduction = 0.5f;

    public int GetSellAmount()
    {
        // Implement an increase if upgraded.
        return (int) (cost * sellReduction);
    }
}
