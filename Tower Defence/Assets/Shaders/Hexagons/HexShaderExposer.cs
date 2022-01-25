using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class HexShaderExposer : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField] private float _glowIntensity;

    private float _glowIntensityCache;
    private static readonly int GlowIntensity = Shader.PropertyToID("_GlowIntensity");

    private void Awake()
    {
        _glowIntensityCache = image.material.GetFloat(GlowIntensity);
    }

    private void LateUpdate()
    {
        if (_glowIntensity != _glowIntensityCache)
        {
            _glowIntensityCache = _glowIntensity;
            image.material.SetFloat(GlowIntensity, _glowIntensity);
        }
    }

}
