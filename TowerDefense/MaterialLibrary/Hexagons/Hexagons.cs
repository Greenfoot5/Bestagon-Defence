
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MaterialLibrary.Hexagons
{
    [ExecuteInEditMode]
    public class Hexagons : Graphic
    {
        private const string MShaderName = "Custom/Hexagon/Background";

        // Sometimes material resets when 'Reverting'
        // This is to reload that
        private void LoadMaterial()
        {
            if (material == null || material.shader.Name != MShaderName)
                material = new Material(Shader.Find(MShaderName));
        }

        public Color Color { get => color; set => color = value; }
        [FormerlySerializedAs("OffsetUV")] public Vector2 offsetUV;

        [FormerlySerializedAs("HexagonScale")] public float hexagonScale = 5;
        [FormerlySerializedAs("ScrollSpeed")] public float scrollSpeed = .03f;
        [FormerlySerializedAs("LuminanceShiftSpeed")] public float luminanceShiftSpeed = .75f;
        [FormerlySerializedAs("OverlayStrength")] public float overlayStrength = -.3f;
        [FormerlySerializedAs("HexagonOpacity")] public float hexagonOpacity = 1;

        [FormerlySerializedAs("GlowIntensity")] public float glowIntensity = 1;
        [FormerlySerializedAs("GlowClamp")] public float glowClamp = 5;
        [FormerlySerializedAs("GlowOpacity")] public float glowOpacity = 1;
        private static readonly int OffsetUV = Shader.PropertyToID("_OffsetUV");
        private static readonly int HexScale = Shader.PropertyToID("_HexScale");
        private static readonly int ScrollSpeed = Shader.PropertyToID("_ScrollSpeed");
        private static readonly int ShiftSpeed = Shader.PropertyToID("_ShiftSpeed");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int Overlay = Shader.PropertyToID("_Overlay");
        private static readonly int GlowIntensity = Shader.PropertyToID("_GlowIntensity");
        private static readonly int GlowClamp = Shader.PropertyToID("_GlowClamp");
        private static readonly int GlowOpacity = Shader.PropertyToID("_GlowOpacity");

        protected override void Awake()
        {
            LoadMaterial();
        }
    
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            RefreshMaterial();
            UpdateGeometry();
            LoadMaterial();
        }
#endif

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            // BOUNDING BOX

            Vector2 corner1 = Vector2.zero;
            Vector2 corner2 = Vector2.zero;

            corner1.X = 0f;
            corner1.Y = 0f;
            corner2.X = 1f;
            corner2.Y = 1f;


            // OFFSET

            Vector2 pivot = rectTransform.pivot;
            corner1.X -= pivot.X;
            corner1.Y -= pivot.Y;
            corner2.X -= pivot.X;
            corner2.Y -= pivot.Y;

            Rect rect = rectTransform.rect;
            corner1.X *= rect.width;
            corner1.Y *= rect.height;
            corner2.X *= rect.width;
            corner2.Y *= rect.height;


            // VERTICES

            vh.Clear();

            UIVertex vert = UIVertex.simpleVert;

            vert.position = new Vector2(corner1.X, corner1.Y);
            vert.color = color;
            vert.uv0 = new Vector2(0, 0);
            vh.AddVert(vert);

            vert.position = new Vector2(corner1.X, corner2.Y);
            vert.color = color;
            vert.uv0 = new Vector2(0, 1);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.X, corner2.Y);
            vert.color = color;
            vert.uv0 = new Vector2(1, 1);
            vh.AddVert(vert);

            vert.position = new Vector2(corner2.X, corner1.Y);
            vert.color = color;
            vert.uv0 = new Vector2(1, 0);
            vh.AddVert(vert);


            // TRIANGLES

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        public void ImportMaterial(Material mat)
        {
            if (mat.shader.Name != MShaderName)
                throw new UnityException("Invalid shader");

            offsetUV = mat.GetVector(OffsetUV);

            hexagonScale = mat.GetFloat(HexScale);
            scrollSpeed = mat.GetFloat(ScrollSpeed);
            luminanceShiftSpeed = mat.GetFloat(ShiftSpeed);
            overlayStrength = mat.GetFloat(Overlay);
            hexagonOpacity = mat.GetFloat(Opacity);

            glowIntensity = mat.GetFloat(GlowIntensity);
            glowClamp = mat.GetFloat(GlowClamp);
            glowOpacity = mat.GetFloat(GlowOpacity);
        
#if UNITY_EDITOR
            OnValidate();
#endif
        }

        private void RefreshMaterial()
        {
            material.SetVector(OffsetUV, offsetUV);

            material.SetFloat(HexScale, hexagonScale);
            material.SetFloat(ScrollSpeed, scrollSpeed);
            material.SetFloat(ShiftSpeed, luminanceShiftSpeed);
            material.SetFloat(Overlay, overlayStrength);
            material.SetFloat(Opacity, hexagonOpacity);

            material.SetFloat(GlowIntensity, glowIntensity);
            material.SetFloat(GlowClamp, glowClamp);
            material.SetFloat(GlowOpacity, glowOpacity);
        }

        public override void _Process(double delta)
        {
            RefreshMaterial();
        }
    }
}