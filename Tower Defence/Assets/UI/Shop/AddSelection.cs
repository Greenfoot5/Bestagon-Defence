using System;
using System.Linq;
using LevelData;
using Turrets.Blueprints;
using Turrets.Modules;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddSelection : MonoBehaviour
{
    public GameObject turretSelectionUI;
    public GameObject ModuleSelectionUI;
    private LevelData.LevelData _levelData;
    public Shop shop;
    
    [SerializeField]
    public TypeSpriteLookup glyphsLookup;

    private bool _firstPurchase = true;
    
    /// <summary>
    /// Setups references, checks we have enough gold and freezes the game when enabled
    /// </summary>
    private void Init()
    {
        // Setup Level Manager reference
        _levelData = BuildManager.instance.GetComponent<GameManager>().levelData;

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
    
    /// <summary>
    /// Creates the new selection based on the GameManager's LevelData
    /// </summary>
    /// <exception cref="OverflowException">Removed duplicates too many times. Likely to have too few options</exception>
    /// <exception cref="ArgumentOutOfRangeException">We cannot pick a new item from the LevelData lists</exception>
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
        // So we don't keep retying to select a non-duplicate option forever
        var lagCounter = 0;
        const int lagCap = 5000;

        // TODO - Perhaps modify amount of choices
        for (var i = 0; i < 3; i++)
        {
            // If it's the first time opening the shop this level, we should display a different selection
            if (_firstPurchase)
            {
                // Add a new turret to the selection
                var turrets = _levelData.initialTurretSelection;
                var selected = turrets.GETRandomItem();

                // Gets a new turret if we have a duplicate (depending on settings)
                switch (_levelData.initialDuplicateCheck)
                {
                    case DuplicateTypes.None:
                        break;

                    case DuplicateTypes.ByName:
                        while (selectedNames.Contains(selected.displayName))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }

                        break;

                    case DuplicateTypes.ByType:
                        while (selectedTypes.Contains(selected.GetType()))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Adds our selection to the UI
                GenerateTurretUI(selected);

                // Add the selection to our selected arrays to avoid duplicates
                selectedTypes[i] = selected.GetType();
                selectedNames[i] = selected.displayName;
                continue;
            }

            // Select if we should get an Module or a turret
            var choice = Random.Range(0f, _levelData.turretOptionWeight + _levelData.ModuleOptionWeight);
            if (choice > _levelData.turretOptionWeight)
            {
                // Grants an Module option
                var Modules = _levelData.Modules;
                var selected = Modules.GETRandomItem();

                // Gets a new Module if we have picked a duplicate (depending on settings)
                switch (_levelData.ModuleDuplicateCheck)
                {
                    case DuplicateTypes.None:
                        break;

                    case DuplicateTypes.ByName:
                        while (selectedNames.Contains(selected.displayName))
                        {
                            selected = Modules.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new Module");
                        }

                        break;

                    case DuplicateTypes.ByType:
                        while (selectedTypes.Contains(selected.GetType()))
                        {
                            selected = Modules.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new Module");
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Adds the Module as an option to the player
                GenerateModuleUI(selected);

                // Add it to our "history" to avoid duplicates on our next selection
                selectedTypes[i] = selected.GetType();
                selectedNames[i] = selected.displayName;
            }
            else
            {
                // Grants a turret option
                var turrets = _levelData.turrets;
                var selected = turrets.GETRandomItem();
                
                // Check we didn't pick something we've already picked (depending on duplicate checking type)
                switch (_levelData.turretDuplicateCheck)
                {
                    case DuplicateTypes.None:
                        break;
                    
                    case DuplicateTypes.ByName:
                        while (selectedNames.Contains(selected.displayName))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }
                        break;
                    
                    case DuplicateTypes.ByType:
                        while (selectedTypes.Contains(selected.GetType()))
                        {
                            selected = turrets.GETRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new turret");
                        }
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                // Add the turret to the ui for the player to pick
                GenerateTurretUI(selected);
                
                // Add the turret to our history so we don't pick it again
                selectedTypes[i] = selected.GetType();
                selectedNames[i] = selected.displayName;
            }
        }
        
        // We can only have a first purchase once
        if (_firstPurchase) _firstPurchase = false;
    }
    
    /// <summary>
    /// Adds a new Module UI option to the player's choice
    /// </summary>
    /// <param name="Module">The Module the player can pick</param>
    private void GenerateModuleUI(Module Module)
    {
        // Create the ui as a child
        var ModuleUI = Instantiate(ModuleSelectionUI, transform);
        ModuleUI.GetComponent<ModuleSelectionUI>().Init(Module, shop, glyphsLookup);
    }
    
    /// <summary>
    /// Adds a new turret UI option to the player's choice
    /// </summary>
    /// <param name="turret">The turret the player can pick</param>
    private void GenerateTurretUI(TurretBlueprint turret)
    {
        var turretUI = Instantiate(turretSelectionUI, transform);
        turretUI.GetComponent<TurretSelectionUI>().Init(turret, shop, glyphsLookup);
    }
}
