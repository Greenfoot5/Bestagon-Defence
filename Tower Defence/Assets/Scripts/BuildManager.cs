using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
            return;
        }
        instance = this;
    }

    public GameObject standardTurretPrefab;

    void Start()
    {
        _turretToBuild = standardTurretPrefab;
    }

    private GameObject _turretToBuild;

    public GameObject GetTurretToBuild()
    {
        return _turretToBuild;
    }
}
