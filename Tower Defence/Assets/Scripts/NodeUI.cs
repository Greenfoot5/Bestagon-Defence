using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    
    private Node _target;
    
    public TMP_Text upgradeText;
    public Button upgradeButton;
    public TMP_Text sellText;

    public void SetTarget(Node node)
    {
        _target = node;

        transform.position = _target.transform.position;

        if (_target.isUpgraded)
        {
            upgradeText.text = "<b>Upgrade\nPurchased</b>";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeText.text = "<b>Upgrade</b>\n<sprite=\"UI-Icons\" name=\"Coin\"> " + _target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }

        sellText.text = "<b>Sell</b>\n<sprite=\"UI-Icons\" name=\"Coin\"> " + _target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        _target.UpgradeTurret();
    }

    public void Sell()
    {
        _target.SellTurret();
    }
}
