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
    public float random;

    private void Init()
    {
        // Setup Game Manager reference
        if (_gameManager == null) _gameManager = BuildManager.instance.GetComponent<GameManager>();

        // Check we can afford to open the shop
        if (GameStats.money < shop.GetSelectionCost())
        {
            Debug.Log("Not enough gold!");
            gameObject.SetActive(false);
            return;
        }
        shop.IncrementSelectionCost();
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
            switch (choice)
            {
                // TODO - Avoid duplicates
                case 0:
                {
                    // TODO - Choose which reward in that type to give
                    var upgrades = _gameManager.levelData.upgrades;
                    GenerateEvolutionUI(upgrades[Random.Range(0, upgrades.Count)]);
                    break;
                }
                case 1:
                {
                    var turrets = _gameManager.levelData.turrets;
                    GenerateTurretUI(turrets[Random.Range(0, turrets.Count)]);
                    break;
                }
            }
        }
    }

    private void GenerateEvolutionUI(Upgrade upgrade)
    {
        // Create the ui as a child
        GameObject evolutionUI = Instantiate(evolutionSelectionUI, transform);
        evolutionUI.GetComponent<EvolutionSelectionUI>().Init(upgrade, shop);
    }

    private void GenerateTurretUI(TurretBlueprint turret)
    {
        GameObject turretUI = Instantiate(turretSelectionUI, transform);
        turretUI.GetComponent<TurretSelectionUI>().Init(turret, shop);
    }
}
