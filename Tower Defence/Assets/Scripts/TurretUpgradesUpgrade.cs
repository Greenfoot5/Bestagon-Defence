using UnityEngine;
using TMPro;
using Turrets.Upgrades;

public class TurretUpgradesUpgrade : MonoBehaviour
{
    [SerializeField]
    private UpgradeIcon icon;
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetData(Upgrade upgrade)
    {
        icon.SetData(upgrade);
        text.text = upgrade.displayName;
    }
}
