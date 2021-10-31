using TMPro;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectionUI : MonoBehaviour
{
    private TurretBlueprint _turretBlueprint;
    
    // Content
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI tagline;
    public Image icon;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI noneText;

    [Header("Colors")]
    public Image bg;
    public Image upgradesBG;
    public TextMeshProUGUI upgradesTitle;

    // Called when creating the UI
    public void Init (TurretBlueprint turret, Shop shop)
    {
        _turretBlueprint = turret;
        displayName.text = turret.displayName;
        tagline.text = turret.tagline;
        icon.sprite = turret.shopIcon;
        // TODO - Stats
        // TODO - Display Upgrades
        
        // Colors
        tagline.color = turret.titleColor;
        upgradesTitle.color = turret.titleColor;
        bg.color = turret.bgColor;
        upgradesBG.color = turret.upgradeBGColor;

        bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
    }

    // Called when the user clicks on the button
    private void MakeSelection (Shop shop)
    {
        transform.parent.gameObject.SetActive (false);
        Time.timeScale = 1f;
        
        shop.SpawnNewTurret(_turretBlueprint);
    }
}
