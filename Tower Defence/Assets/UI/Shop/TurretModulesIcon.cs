using UnityEngine;
using TMPro;
using Turrets.Modules;

public class TurretModulesIcon : MonoBehaviour
{
    [SerializeField]
    private ModuleIcon icon;
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetData(Module module)
    {
        icon.SetData(module);
        text.text = module.displayName;
    }
}
