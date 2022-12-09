using System;
using System.Collections.Generic;
using System.Linq;
using Abstract;
using Abstract.Data;
using Gameplay;
using Levels.Maps;
using Turrets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Shop
{
    public class AddSelection : MonoBehaviour
    {
        [Tooltip("The game object for a turret selection card")]
        [SerializeField]
        private GameObject turretSelectionUI;
        [Tooltip("The game object for a module selection card")]
        [SerializeField]
        private GameObject moduleSelectionUI;
        private LevelData _levelData;
        [Tooltip("The Shop component in the scene")]
        [SerializeField]
        private Gameplay.Shop shop;
        
        [Tooltip("The GlyphsLookup index in the scene")]
        [SerializeField]
        public TypeSpriteLookup glyphsLookup;
        
        [Tooltip("The turrets already purchased")]
        private readonly List<Type> _turretTypes = new List<Type>();
    
        /// <summary>
        /// Setups references, checks the player has enough gold and freezes the game when enabled
        /// </summary>
        private void Init()
        {
            // Setup Level Manager reference
            _levelData = BuildManager.instance.GetComponent<GameManager>().levelData;

            // Check the player can afford to open the shop
            if (GameStats.money < shop.selectionCost)
            {
                //Debug.Log("Not enough gold!");
                transform.parent.gameObject.SetActive(false);
                return;
            }

            Time.timeScale = 0f;
            shop.IncrementSelectionCost();
            
        }
    
        /// <summary>
        /// Creates the new selection based on the GameManager's LevelData
        /// </summary>
        /// <exception cref="OverflowException">Removed duplicates too many times. Likely to have too few options</exception>
        /// <exception cref="ArgumentOutOfRangeException">The game cannot pick a new item from the LevelData lists</exception>
        private void OnEnable()
        {
            Init();
        
            GenerateSelection();
        }

        public void GenerateSelection()
        {
            // Destroy the previous selection
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            
            // Tracks what the game has given the player, so the game don't give duplicates
            var selectedTypes = new Type[3];
            var selectedNames = new string[3];
            // So the game doesn't keep retying to select a non-duplicate option forever
            var lagCounter = 0;
            const int lagCap = 5000;
            
            int selectionCount = shop.HasPlayerMadePurchase() ? _levelData.initialSelectionCount : _levelData.selectionCount;
            
            for (var i = 0; i < selectionCount; i++)
            {
                // If it's the first time opening the shop this level, the game should display a different selection
                if (!shop.HasPlayerMadePurchase())
                {
                    // Add a new turret to the selection
                    WeightedList<TurretBlueprint> turrets = _levelData.initialTurretSelection;
                    TurretBlueprint selected = turrets.GetRandomItem();

                    // Gets a new turret if there is a duplicate (depending on settings)
                    switch (_levelData.initialDuplicateCheck)
                    {
                        case DuplicateTypes.None:
                            break;

                        case DuplicateTypes.ByName:
                            while (selectedNames.Contains(selected.displayName))
                            {
                                selected = turrets.GetRandomItem();
                                lagCounter++;
                                if (lagCounter > lagCap)
                                    throw new OverflowException("Too many attempts to pick new turret");
                            }

                            break;

                        case DuplicateTypes.ByType:
                            while (selectedTypes.Contains(selected.GetType()))
                            {
                                selected = turrets.GetRandomItem();
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

                // Select if the game should get an Module or a turret
                float choice = Random.Range(0f, _levelData.turretOptionWeight.Value.Evaluate(GameStats.Rounds)
                                                + _levelData.moduleOptionWeight.Value.Evaluate(GameStats.Rounds));
                if (choice > _levelData.turretOptionWeight.Value.Evaluate(GameStats.Rounds))
                {
                    // Grants an Module option
                    WeightedList<ModuleChainHandler> modules = _levelData.moduleHandlers.ToWeightedList(GameStats.Rounds);
                    ModuleChainHandler selected = modules.GetRandomItem();
                    
                    // Check the player actually has a turret of the modules type
                    // But only if they have actually bought some turrets
                    if (_turretTypes.Any())
                    {
                        while (!(selected.GetModule().GetValidTypes() == null ||
                                 _turretTypes.Any(x => selected.GetModule().GetValidTypes().Contains(x))))
                        {
                            selected = modules.GetRandomItem();
                            lagCounter++;
                            if (lagCounter > lagCap)
                                throw new OverflowException("Too many attempts to pick new Module");
                        }
                    }

                    // Gets a new Module if the random has picked a duplicate (depending on settings)
                    switch (_levelData.moduleDuplicateCheck)
                    {
                        case DuplicateTypes.None:
                            break;

                        case DuplicateTypes.ByName:
                            while (selectedNames.Contains(selected.GetChain().displayName))
                            {
                                selected = modules.GetRandomItem();
                                lagCounter++;
                                if (lagCounter > lagCap)
                                    throw new OverflowException("Too many attempts to pick new Module");
                            }

                            break;

                        case DuplicateTypes.ByType:
                            while (selectedTypes.Contains(selected.GetModule().GetType()))
                            {
                                selected = modules.GetRandomItem();
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
                    selectedTypes[i] = selected.GetModule().GetType();
                    selectedNames[i] = selected.GetChain().displayName;
                }
                else
                {
                    // Grants a turret option
                    WeightedList<TurretBlueprint> turrets = _levelData.turrets.ToWeightedList(GameStats.Rounds);
                    TurretBlueprint selected = turrets.GetRandomItem();
                
                    // Check the game didn't pick something it's already picked (depending on duplicate checking type)
                    switch (_levelData.turretDuplicateCheck)
                    {
                        case DuplicateTypes.None:
                            break;
                    
                        case DuplicateTypes.ByName:
                            while (selectedNames.Contains(selected.displayName))
                            {
                                selected = turrets.GetRandomItem();
                                lagCounter++;
                                if (lagCounter > lagCap)
                                    throw new OverflowException("Too many attempts to pick new turret");
                            }
                            break;
                    
                        case DuplicateTypes.ByType:
                            while (selectedTypes.Contains(selected.GetType()))
                            {
                                selected = turrets.GetRandomItem();
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
                
                    // Add the turret to our history so the game don't pick it again
                    selectedTypes[i] = selected.GetType();
                    selectedNames[i] = selected.displayName;
                }
            }
        }
    
        /// <summary>
        /// Adds a new Module UI option to the player's choice
        /// </summary>
        /// <param name="handler">The Module the player can pick</param>
        private void GenerateModuleUI(ModuleChainHandler handler)
        {
            // Create the ui as a child
            GameObject moduleUI = Instantiate(moduleSelectionUI, transform);
            moduleUI.name = "_" + moduleUI.name;
            moduleUI.GetComponent<ModuleSelectionUI>().Init(handler, shop, glyphsLookup);
        }
    
        /// <summary>
        /// Adds a new turret UI option to the player's choice
        /// </summary>
        /// <param name="turret">The turret the player can pick</param>
        private void GenerateTurretUI(TurretBlueprint turret)
        {
            GameObject turretUI = Instantiate(turretSelectionUI, transform);
            turretUI.name = "_" + turretUI.name;
            turretUI.GetComponent<TurretSelectionUI>().Init(turret, shop, glyphsLookup);
        }
        
        /// <summary>
        /// Adds a turret type to the selected type.
        /// Makes sure we have a full list of turret the player has purchased
        /// so we can display only ones they can use
        /// </summary>
        /// <param name="type">The type of the turret to add</param>
        public void AddTurretType(Type type)
        {
            if (!_turretTypes.Contains(type))
                _turretTypes.Add(type);
        }

        public void AddTurret(Turret turret)
        {
            AddTurretType(turret.GetType());
        }
    }
}
