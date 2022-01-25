using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Turrets.Modules;

public class ModuleIcon : MonoBehaviour
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

    public void SetData(Module Module)
    {
        SetSprite(Module.icon);
        SetLevel(Module.ModuleTier);
    }
}
