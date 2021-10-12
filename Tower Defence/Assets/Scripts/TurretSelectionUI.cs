using TMPro;
using Turrets;
using Turrets.Blueprints;
using Turrets.Upgrades.TurretUpgrades;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectionUI : MonoBehaviour
{
    private TurretBlueprint turretBlueprint;
    public Image iconImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI type;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI upgrades;

    // Called when creating the UI
    public void Init (TurretBlueprint turret, Shop shop)
    {
        turretBlueprint = turret;
        iconImage.sprite = turret.shopIcon;
        displayName.text = turret.displayName;
        type.text = turret.turretType;
        // Set the upgrades values
        upgrades.text = GetUpgradesText();
        
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });;
    }

    // Called when the user clicks on the button
    private void MakeSelection (Shop shop)
    {
        // TODO - Grant the user's choice
        
        transform.parent.gameObject.SetActive (false);
        Time.timeScale = 1f;
        
        shop.SpawnNewTurret(turretBlueprint);
    }

    private string GetUpgradesText()
    {
        return "Upgrades:\n• None";
    }
}
