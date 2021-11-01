using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretStat : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;

    public void SetColor(Color color)
    {
        icon.color = color;
    }

    public void SetData(string data)
    {
        text.text = data;
    }
    public void SetData(object data) => SetData(data.ToString());
}
