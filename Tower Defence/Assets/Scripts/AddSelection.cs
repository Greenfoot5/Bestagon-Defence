using Turrets.Blueprints;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddSelection : MonoBehaviour
{
    public GameObject turretSelectionUI;
    public GameObject evolutionSelectionUI;
    private GameManager _gameManager;
    public Shop shop;

    private void Init()
    {
        if (_gameManager == null) _gameManager = BuildManager.instance.GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        Init();
        
        // Pause the game so the user can think
        Time.timeScale = 0f;
        
        // Destroy the previous selection
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        // TODO - Perhaps modify amount of choices
        for (var i = 0; i < 3; i++)
        {
            // TODO - Choose which reward type to give
            var choice = Random.Range(0, 2);
            // TODO - Avoid duplicates
            if (choice == 0)
            {
                // TODO - Choose which reward in that type to give
                var upgrades = _gameManager.levelData.upgrades;
                GenerateEvolutionUI(upgrades[Random.Range(0, upgrades.Count - 1)]);
            }
            else if (choice == 1)
            {
                var turrets = _gameManager.levelData.turrets;
                GenerateTurretUI(turrets[Random.Range(0, turrets.Count - 1)]);
            }
        }
    }

    private void GenerateEvolutionUI(Upgrade upgrade)
    {
        // Create the ui as a child
        GameObject evolutionUI = Instantiate(evolutionSelectionUI, transform);
        Debug.Log(upgrade);
        evolutionUI.GetComponent<EvolutionSelectionUI>().Init((TurretUpgrade)upgrade);
    }

    private void GenerateTurretUI(TurretBlueprint turret)
    {
        GameObject turretUI = Instantiate(turretSelectionUI, transform);
        turretUI.GetComponent<TurretSelectionUI>().Init(turret, shop);
    }
}
