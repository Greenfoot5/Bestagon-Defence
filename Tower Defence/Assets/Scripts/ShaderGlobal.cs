using UnityEngine;

public class ShaderGlobal : MonoBehaviour
{
    private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

    void Update()
    {
        Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
    }
}
