using UnityEngine;
using TMPro;
using Turrets.Modules;

public class TurretModulesModule : MonoBehaviour
{
    [SerializeField]
    private ModuleIcon icon;
    [SerializeField]
    private TextMeshProUGUI text;

    public void SetData(Module Module)
    {
        icon.SetData(Module);
        text.text = Module.displayName;
    }
}
