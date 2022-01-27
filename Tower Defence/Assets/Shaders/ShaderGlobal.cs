using UnityEngine;

namespace Shaders
{
    public class ShaderGlobal : MonoBehaviour
    {
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

        private void Update()
        {
            Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
        }
    }
}
