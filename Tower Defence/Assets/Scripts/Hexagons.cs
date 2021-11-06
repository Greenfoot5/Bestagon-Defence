using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Hexagons : Graphic
{
    private static readonly string m_ShaderName = "Unlit/Hex Background";

    protected override void Awake()
    {
        material = new Material(Shader.Find(m_ShaderName));
    }

    public Color Color { get => color; set => color = value; }
    public Vector2 OffsetUV;

    public float HexagonScale;
    public float ScrollSpeed;
    public float LuminanceShiftSpeed;
    public float OverlayStrength;
    public float HexagonOpacity;

    public float GlowIntensity;
    public float GlowClamp;
    public float GlowOpacity;

    protected override void OnValidate()
    {
        RefreshMaterial();
        UpdateGeometry();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        // BOUNDING BOX

        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;


        // OFFSET

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= rectTransform.rect.width;
        corner1.y *= rectTransform.rect.height;
        corner2.x *= rectTransform.rect.width;
        corner2.y *= rectTransform.rect.height;


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

    public void ImportMaterial(Material material)
    {
        if (material.shader.name != m_ShaderName)
            throw new UnityException("Invalid shader");

        OffsetUV = material.GetVector("_OffsetUV");

        HexagonScale = material.GetFloat("_HexScale");
        ScrollSpeed = material.GetFloat("_ScrollSpeed");
        LuminanceShiftSpeed = material.GetFloat("_ShiftSpeed");
        OverlayStrength = material.GetFloat("_Overlay");
        HexagonOpacity = material.GetFloat("_Opacity");

        GlowIntensity = material.GetFloat("_GlowIntensity");
        GlowClamp = material.GetFloat("_GlowClamp");
        GlowOpacity = material.GetFloat("_GlowOpacity");

        OnValidate();
    }

    private void RefreshMaterial()
    {
        material.SetVector("_OffsetUV", OffsetUV);

        material.SetFloat("_HexScale", HexagonScale);
        material.SetFloat("_ScrollSpeed", ScrollSpeed);
        material.SetFloat("_ShiftSpeed", LuminanceShiftSpeed);
        material.SetFloat("_Overlay", OverlayStrength);
        material.SetFloat("_Opacity", HexagonOpacity);

        material.SetFloat("_GlowIntensity", GlowIntensity);
        material.SetFloat("_GlowClamp", GlowClamp);
        material.SetFloat("_GlowOpacity", GlowOpacity);
    }

    private void Update()
    {
        RefreshMaterial();
    }
}