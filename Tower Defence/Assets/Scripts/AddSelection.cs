using System;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddSelection : MonoBehaviour
{
    public GameObject turretSelectionUI;
    // TODO - Remove test
    public TurretUpgrade[] test_upgrades;
    public GameObject evolutionSelectionUI;

    private void OnEnable()
    {
        // Pause the game so the user can think
        Time.timeScale = 0f;
        
        // Destroy the previous selection
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        // TODO - Perhaps modify amount of choices
        for (int i = 0; i < 3; i++)
        {
            // TODO - Choose which reward type to give
            String choiceType = "TurretEvolution";
            // TODO - Avoid duplicates
            switch (choiceType)
            {
                case "TurretEvolution":
                    // TODO - Choose which reward in that type to give
                    GenerateEvolutionUI(test_upgrades[Random.Range(0, test_upgrades.Length - 1)]);
                    break;
                
                default:
                    Debug.LogError("Invalid choice type selected");
                    break;
            }
        }
    }

    private void GenerateEvolutionUI(TurretUpgrade upgrade)
    {
        // Create the ui as a child
        GameObject evolutionUI = Instantiate(evolutionSelectionUI, transform);
        evolutionUI.GetComponent<EvolutionSelectionUI>().Init(upgrade);
    }
}
