using TMPro;
using Turrets.Upgrades;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionUI : MonoBehaviour
{
    public Upgrade upgrade;

    public Hexagons bg;

    public TextMeshProUGUI displayName;
    public TextMeshProUGUI tagline;

    public UpgradeIcon icon;

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

        icon.SetData(initUpgrade);
        
        effect.text = initUpgrade.effectText;
        effect.color = initUpgrade.accentColor;

        // Set the restrictions values
        // TODO
        
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
