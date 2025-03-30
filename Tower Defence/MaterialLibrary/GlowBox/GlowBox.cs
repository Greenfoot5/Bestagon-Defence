
using UnityEngine.UI;

namespace MaterialLibrary.GlowBox
{
    [ExecuteInEditMode]
    public class GlowBox : Graphic
    {
        //private const string MShaderName = "Custom/GlowBox/Background";
        private const string MShaderName = "UI/Default/GlowBox";

        // Sometimes material resets when 'Reverting'
        // This is to reload that
        private void LoadMaterial()
        {
            if (material == null || material.shader.Name != MShaderName)
                material = new Material(Shader.Find(MShaderName));
        }

        public Color Color { get => color; set => color = value; }
        public float baseOpacity = 1;
        public float glowIntensity = 1;
        public float glowClamp = 5;
        public float glowOpacity = 1;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
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
            
            baseOpacity = mat.GetFloat(Opacity);

            glowIntensity = mat.GetFloat(GlowIntensity);
            glowClamp = mat.GetFloat(GlowClamp);
            glowOpacity = mat.GetFloat(GlowOpacity);
        
#if UNITY_EDITOR
            OnValidate();
#endif
        }

        private void RefreshMaterial()
        {
            material.SetFloat(Opacity, baseOpacity);

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