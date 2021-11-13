using Turrets.Blueprints;
using Turrets.Upgrades;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddSelection : MonoBehaviour
{
    public GameObject turretSelectionUI;
    public GameObject evolutionSelectionUI;
    private GameManager _gameManager;
    public Shop shop;

    private bool firstPurchase;

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

        Time.timeScale = 0f;
        shop.IncrementSelectionCost();
    }

    private void OnEnable()
    {
        Init();

        // Destroy the previous selection
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // TODO - Perhaps modify amount of choices
        for (var i = 0; i < 3; i++)
        {
            // If it's the first time opening the shop this level, we should display a different selection
            if (firstPurchase)
            {
                var turrets = _gameManager.levelData.initialTurretSelection;
                GenerateTurretUI(turrets.GETRandomItem());

                continue;
            }
            
            // TODO - Choose which reward type to give
            var choice = Random.Range(0, 2);
            switch (choice)
            {
                // TODO - Avoid duplicates
                case 0:
                {
                    // TODO - Choose which reward in that type to give
                    var upgrades = _gameManager.levelData.upgrades;
                    GenerateEvolutionUI(upgrades.GETRandomItem());
                    break;
                }
                case 1:
                {
                    var turrets = _gameManager.levelData.turrets;
                    GenerateTurretUI(turrets.GETRandomItem());
                    break;
                }
            }
        }

        if (firstPurchase) firstPurchase = false;
    }

    private void GenerateEvolutionUI(Upgrade upgrade)
    {
        // Create the ui as a child
        var evolutionUI = Instantiate(evolutionSelectionUI, transform);
        evolutionUI.GetComponent<UpgradeSelectionUI>().Init(upgrade, shop);
    }

    private void GenerateTurretUI(TurretBlueprint turret)
    {
        var turretUI = Instantiate(turretSelectionUI, transform);
        turretUI.GetComponent<TurretSelectionUI>().Init(turret, shop);
    }
}
