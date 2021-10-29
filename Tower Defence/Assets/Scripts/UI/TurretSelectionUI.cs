using TMPro;
using Turrets.Blueprints;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectionUI : MonoBehaviour
{
    private TurretBlueprint _turretBlueprint;
    public Image iconImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI type;
    public TextMeshProUGUI upgrades;

    // Called when creating the UI
    public void Init (TurretBlueprint turret, Shop shop)
    {
        _turretBlueprint = turret;
        iconImage.sprite = turret.shopIcon;
        displayName.text = turret.displayName;
        type.text = turret.turretType;
        // Set the upgrades values
        if (_turretBlueprint.upgrades.Count == 0)
        {
            upgrades.text += "\n• None";
        }
        else
        {
            foreach (var upgrade in _turretBlueprint.upgrades)
            {
                upgrades.text += "\n• " + upgrade.displayName;
            }
        }
        
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
    }

    // Called when the user clicks on the button
    private void MakeSelection (Shop shop)
    {
        transform.parent.gameObject.SetActive (false);
        Time.timeScale = 1f;
        
        shop.SpawnNewTurret(_turretBlueprint);
    }
}
