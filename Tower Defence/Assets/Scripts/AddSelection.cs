using System;
using System.Linq;
using LevelData;
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

    private bool _firstPurchase = true;

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
        
        // Tracks what we've given, so we don't give duplicates
        var selectedTypes = new Type[3];
        var selectedNames = new string[3];
        var lagCounter = 0;

        // TODO - Perhaps modify amount of choices
        for (var i = 0; i < 3; i++)
        {
            // If it's the first time opening the shop this level, we should display a different selection
            if (_firstPurchase)
            {
                var turrets = _gameManager.levelData.initialTurretSelection;
                var selected = turrets.GETRandomItem();
                switch (_gameManager.levelData.initialDuplicateCheck)
                {
                    case DuplicateTypes.None:
                        break;
                    case DuplicateTypes.ByName:
                        while (selectedNames.Contains(selected.displayName))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > 5000)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }

                        break;
                    case DuplicateTypes.ByType:
                        while (selectedTypes.Contains(selected.GetType()))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > 5000)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                GenerateTurretUI(selected);

                selectedTypes[i] = selected.GetType();
                selectedNames[i] = selected.displayName;
                continue;
            }
            
            // TODO - Choose which reward type to give with ratios/weights
            var choice = Random.Range(0, 2);
            switch (choice)
            {
                case 0:
                {
                    var upgrades = _gameManager.levelData.upgrades;
                    var selected = upgrades.GETRandomItem();
                    switch (_gameManager.levelData.upgradeDuplicateCheck)
                    {
                        case DuplicateTypes.None:
                            break;
                        case DuplicateTypes.ByName:
                            while (selectedNames.Contains(selected.displayName))
                            {
                                selected = upgrades.GETRandomItem();
                                lagCounter++;
                                if (lagCounter > 5000)
                                    throw new OverflowException("Too many attempts to pick new upgrade");
                            }
                            break;
                        case DuplicateTypes.ByType:
                            while (selectedTypes.Contains(selected.GetType()))
                            {
                                selected = upgrades.GETRandomItem();
                                lagCounter++;
                                if (lagCounter > 5000)
                                    throw new OverflowException("Too many attempts to pick new upgrade");
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    GenerateEvolutionUI(selected);
                    
                    selectedTypes[i] = selected.GetType();
                    selectedNames[i] = selected.displayName;
                    break;
                }
                case 1:
                {
                    var turrets = _gameManager.levelData.turrets;
                    var selected = turrets.GETRandomItem();
                    switch (_gameManager.levelData.turretDuplicateCheck)
                    {
                        case DuplicateTypes.None:
                            break;
                        case DuplicateTypes.ByName:
                            while (selectedNames.Contains(selected.displayName))
                            {
                                selected = turrets.GETRandomItem();
                                lagCounter++;
                                if (lagCounter > 5000)
                                    throw new OverflowException("Too many attempts to pick new turret");
                            }
                            break;
                        case DuplicateTypes.ByType:
                            while (selectedTypes.Contains(selected.GetType()))
                            {
                                selected = turrets.GETRandomItem();
                                lagCounter++;
                                if (lagCounter > 5000)
                                    throw new OverflowException("Too many attempts to pick new turret");
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    GenerateTurretUI(selected);
                    
                    selectedTypes[i] = selected.GetType();
                    selectedNames[i] = selected.displayName;
                    break;
                }
            }
        }

        if (_firstPurchase) _firstPurchase = false;
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
