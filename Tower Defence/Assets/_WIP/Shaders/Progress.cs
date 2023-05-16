using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _WIP.Shaders {
    [ExecuteInEditMode]
    public class Progress : Graphic {
        private const string MShaderName = "Custom/ProgressBar";

        // Sometimes material resets when 'Reverting'
        // This is to reload that
        private void LoadMaterial() {
            if (material == null || material.shader.name != MShaderName)
                material = new Material(Shader.Find(MShaderName));
        }
        
        [FormerlySerializedAs("barColor")] [Header("Important Stuff")]
        public Color barColorA;
        public Color barColorB;
        [FormerlySerializedAs("bgColor")] public Color bgColorA;
        public Color bgColorB;
        [Range(0, 1)]
        public float percentage;
        [Range(0, 1)]
        public float trapeziumPercentage;
        public bool flipTrapezium;

        [Header("Outline")]
        public Color outColorA;
        public Color outColorB;
        [Range(0, 1)]
        public float outPercentage;
        [Range(0, 1)]
        public float outOffset;
        [Range(0, 1)]
        public float outY;

        [Range(-1, 1)]
        public float showHalf;
        
        private static readonly int Percentage = Shader.PropertyToID("_Percentage");
        private static readonly int BarColorA = Shader.PropertyToID("_BarColorA");
        private static readonly int BarColorB = Shader.PropertyToID("_BarColorB");
        private static readonly int BgColorA = Shader.PropertyToID("_BgColorA");
        private static readonly int BgColorB = Shader.PropertyToID("_BgColorB");
        private static readonly int OutColorA = Shader.PropertyToID("_OutColorA");
        private static readonly int OutColorB = Shader.PropertyToID("_OutColorB");
        
        private static readonly int TrapPercent = Shader.PropertyToID("_TrapPercent");
        private static readonly int Flip = Shader.PropertyToID("_Flip");
        private static readonly int OutPercentage = Shader.PropertyToID("_OutX");
        private static readonly int OutOffset = Shader.PropertyToID("_OutOffset");
        private static readonly int OutY = Shader.PropertyToID("_OutY");
        private static readonly int ShowHalf = Shader.PropertyToID("_ShowHalf");


        protected override void Awake() {
            LoadMaterial();
        }
    
#if UNITY_EDITOR
        protected override void OnValidate() {
            RefreshMaterial();
            UpdateGeometry();
            LoadMaterial();
        }
#endif

        protected override void OnPopulateMesh(VertexHelper vh) {
            // BOUNDING BOX
            Vector2 corner1 = Vector2.zero;
            Vector2 corner2 = Vector2.zero;

            corner1.x = 0f;
            corner1.y = 0f;
            corner2.x = 1f;
            corner2.y = 1f;


            // OFFSET

            Vector2 pivot = rectTransform.pivot;
            corner1.x -= pivot.x;
            corner1.y -= pivot.y;
            corner2.x -= pivot.x;
            corner2.y -= pivot.y;

            Rect rect = rectTransform.rect;
            corner1.x *= rect.width;
            corner1.y *= rect.height;
            corner2.x *= rect.width;
            corner2.y *= rect.height;


            // VERTICES

            vh.Clear();

            UIVertex vert = UIVertex.simpleVert;

            vert.position = new Vector2(corner1.x, corner1.y);
            vert.color = color;
            vert.uv0 = new Vector2(0, 0);
            vh.AddVert(vert);

            vert.position = new Vector2(corner1.x, corner2.y);
            vert.color = color;
            vert.uv0 = new Vector2(0, 1);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.x, corner2.y);
            vert.color = color;
            vert.uv0 = new Vector2(1, 1);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.x, corner1.y);
            vert.color = color;
            vert.uv0 = new Vector2(1, 0);
            vh.AddVert(vert);


            // TRIANGLES

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        public void ImportMaterial(Material mat) {
            if (mat.shader.name != MShaderName)
                throw new UnityException("Invalid shader");
            
            // glowOpacity = mat.GetFloat(GlowOpacity);
            barColorA = mat.GetColor(BarColorA);
            barColorB = mat.GetColor(BarColorB);
            bgColorA = mat.GetColor(BgColorA);
            bgColorB = mat.GetColor(BgColorB);
            outColorA = mat.GetColor(OutColorA);
            outColorB = mat.GetColor(OutColorB);
            percentage = mat.GetFloat(Percentage);
            trapeziumPercentage = mat.GetFloat(TrapPercent);
            const float tolerance = 0.1f;
            flipTrapezium = Math.Abs(mat.GetFloat(Flip) - 1.0) < tolerance;
            outPercentage = mat.GetFloat(OutPercentage);
            outOffset = mat.GetFloat(OutOffset);
            outY = mat.GetFloat(OutY);
            showHalf = mat.GetFloat(ShowHalf);
            
        
#if UNITY_EDITOR
            OnValidate();
#endif
        }

        private void RefreshMaterial() {
            // material.SetFloat(GlowOpacity, glowOpacity);
            material.SetColor(BgColorA, bgColorA);
            material.SetColor(BgColorB, bgColorB);
            material.SetColor(BarColorA, barColorA);
            material.SetColor(BarColorB, barColorB);
            material.SetColor(OutColorA, outColorA);
            material.SetColor(OutColorB, outColorB);
            material.SetFloat(Percentage, percentage);
            material.SetFloat(TrapPercent, trapeziumPercentage);
            material.SetFloat(Flip, flipTrapezium ? 1 : 0);
            material.SetFloat(OutPercentage, outPercentage);
            material.SetFloat(OutOffset, outOffset);
            material.SetFloat(OutY, outY);
            material.SetFloat(ShowHalf, showHalf);
        }

        private void Update() {
            RefreshMaterial();
        }
    }
}