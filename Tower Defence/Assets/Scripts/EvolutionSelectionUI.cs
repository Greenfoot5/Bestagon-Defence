using TMPro;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionSelectionUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI effect;
    public TextMeshProUGUI restrictions;

    // Called when creating the UI
    public void Init (TurretUpgrade upgrade)
    {
        iconImage.sprite = upgrade.icon;
        displayName.text = upgrade.displayName;
        effect.text = upgrade.effectText;
        // Set the restrictions values
        if (upgrade.restrictionsText.Length == 0)
        {
            restrictions.text += "\n• Any";
        }
        else
        {
            foreach (string turretRestriction in upgrade.restrictionsText)
            {
                restrictions.text += "\n• " + turretRestriction;
            }
        }
        
        gameObject.GetComponent<Button>().onClick.AddListener(MakeSelection);
    }

    // Called when the user clicks on the button
    private void MakeSelection ()
    {
        // TODO - Grant the user's choice
        transform.parent.gameObject.SetActive (false);
    }
}
