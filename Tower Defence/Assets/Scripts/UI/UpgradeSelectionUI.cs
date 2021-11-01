using TMPro;
using Turrets.Upgrades;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionUI : MonoBehaviour
{
    public Upgrade upgrade;

    public Image bg;

    public TextMeshProUGUI displayName;
    public TextMeshProUGUI tagline;

    public Image iconImage;

    public TextMeshProUGUI effect;
    public TextMeshProUGUI restrictions;

    // Called when creating the UI
    public void Init (Upgrade initUpgrade, Shop shop)
    {
        upgrade = initUpgrade;
        
        bg.color = initUpgrade.accentColor;

        displayName.text = initUpgrade.displayName;
        tagline.text = initUpgrade.tagline;
        tagline.color = initUpgrade.accentColor;
        
        iconImage.sprite = initUpgrade.icon;
        
        effect.text = initUpgrade.effectText;
        effect.color = initUpgrade.accentColor;

        // Set the restrictions values
        if (initUpgrade.validTypes.Length == 0)
        {
            restrictions.text += "\n• Any";
        }
        else
        {
            foreach (var turretType in initUpgrade.validTypes)
            {
                restrictions.text += "\n• " + turretType + " Turret";
            }
        }
        
        bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
    }

    // Called when the user clicks on the button
    private void MakeSelection (Shop shop)
    {
        // TODO - Grant the user's choice
        transform.parent.gameObject.SetActive (false);
        Time.timeScale = 1f;
        
        shop.SpawnNewUpgrade(upgrade);
    }
}
