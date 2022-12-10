using UnityEngine;

namespace MaterialLibrary
{
    public class ShaderGlobal : MonoBehaviour
    {
        private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");
        
        /// <summary>
        /// Updates the hexagon shader with the new time
        /// </summary>
        private void Update()
        {
            Shader.SetGlobalFloat(UnscaledTime, Time.unscaledTime);
        }
    }
}
