using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MaterialLibrary.Hexagons
{
    [ExecuteAlways]
    public class HexShaderExposer : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [FormerlySerializedAs("_glowIntensity")] [SerializeField] private float glowIntensity;

        private float _glowIntensityCache;
        private static readonly int GlowIntensity = Shader.PropertyToID("_GlowIntensity");
        private const float Tolerance = 0.1f;

        private void Awake()
        {
            _glowIntensityCache = image.material.GetFloat(GlowIntensity);
        }

        private void LateUpdate()
        {
            if (!(Math.Abs(glowIntensity - _glowIntensityCache) > Tolerance)) return;
            
            _glowIntensityCache = glowIntensity;
            image.material.SetFloat(GlowIntensity, glowIntensity);
        }

    }
}
