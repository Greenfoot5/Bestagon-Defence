using UnityEngine;
using UnityEngine.UI;

public class Upgrade : ScriptableObject
{
    [SerializeField]
    private string upgradeType;

    [Range(0f, 1f)]
    [SerializeField]
    private float effectPercentage;
    [SerializeField]
    private int upgradeTier;
        
    // TODO - Generate display name from update type and tier
    public string displayName;
    public Sprite icon;
    public string effectText;
    public string[] restrictionsText;
    
    public string GETUpgradeType()
    {
        return upgradeType;
    }

    public float GETUpgradeValue()
    {
        return effectPercentage;
    }

    protected int GETUpgradeTier()
    {
        return upgradeTier;
    }

    public GameObject GenerateButton(GameObject button, Shop shop)
    {
        // Set image
        button.GetComponent<Image>().sprite = icon;
        // var buttonTransform = button.GetComponent<RectTransform>();
        // buttonTransform.sizeDelta = new Vector2(buttonTransform.rect.height, icon.rect.width);
        button.GetComponent<Button>().onClick.AddListener(delegate { shop.SelectUpgrade(this); });
        return button;
    }
}