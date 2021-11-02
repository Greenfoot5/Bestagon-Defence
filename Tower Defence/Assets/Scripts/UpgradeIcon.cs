using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Turrets.Upgrades;

public class UpgradeIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI text;

    private static readonly string[] _levels = new string[] { "-", "I", "II", "III", "IV", "V" };

    public void SetSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetLevel(int level)
    {
        text.text = _levels[level];
    }

    public void SetData(Upgrade upgrade)
    {
        SetSprite(upgrade.icon);
        SetLevel(upgrade.upgradeTier);
    }
}
