using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Hexagons))]
public class Hexagons_Editor : Editor
{
    // DISPLAY FLAGS

    public bool showHexagons;
    public bool animateHexagons;
    public bool glow;
    public bool import;
    public bool loaded = false;


    // PROPERTIES

    private SerializedProperty Color;
    private SerializedProperty OffsetUV;

    private SerializedProperty HexagonScale;
    private SerializedProperty ScrollSpeed;
    private SerializedProperty LuminanceShiftSpeed;
    private SerializedProperty OverlayStrength;
    private SerializedProperty HexagonOpacity;

    private SerializedProperty GlowIntensity;
    private SerializedProperty GlowClamp;
    private SerializedProperty GlowOpacity;


    // IMPORT

    private Material Material;

    private void OnEnable()
    {
        Color = serializedObject.FindProperty("m_Color");
        OffsetUV = serializedObject.FindProperty("OffsetUV");

        HexagonScale = serializedObject.FindProperty("HexagonScale");
        ScrollSpeed = serializedObject.FindProperty("ScrollSpeed");
        LuminanceShiftSpeed = serializedObject.FindProperty("LuminanceShiftSpeed");
        OverlayStrength = serializedObject.FindProperty("OverlayStrength");
        HexagonOpacity = serializedObject.FindProperty("HexagonOpacity");

        GlowIntensity = serializedObject.FindProperty("GlowIntensity");
        GlowClamp = serializedObject.FindProperty("GlowClamp");
        GlowOpacity = serializedObject.FindProperty("GlowOpacity");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(Color);
        EditorGUILayout.PropertyField(OffsetUV);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Hexagons", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(HexagonScale);
        EditorGUILayout.PropertyField(OverlayStrength);
        EditorGUILayout.PropertyField(HexagonOpacity);

        EditorGUILayout.Separator();

        animateHexagons = EditorGUILayout.Foldout(animateHexagons, "Animation");
        if (animateHexagons)
        {
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(ScrollSpeed);
            EditorGUILayout.PropertyField(LuminanceShiftSpeed);
        }

        EditorGUILayout.Separator();

        glow = EditorGUILayout.Foldout(glow, "Inner Edge Glow");
        if (glow)
        {
            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(GlowIntensity);
            EditorGUILayout.PropertyField(GlowClamp);
            EditorGUILayout.PropertyField(GlowOpacity);
        }

        EditorGUILayout.Separator();

        import = EditorGUILayout.Foldout(import, "Import Parameters");
        if (import)
        {
            EditorGUILayout.Separator();

            Material = (Material)EditorGUILayout.ObjectField(Material, typeof(Material), false);

            if (Material != null)
            {
                try
                {
                    ((Hexagons)serializedObject.targetObject).ImportMaterial(Material);
                    Material = null;
                    loaded = true;

                    PrefabUtility.RecordPrefabInstancePropertyModifications(serializedObject.targetObject);
                }
                catch (UnityException e)
                {
                    EditorGUILayout.HelpBox($"Failed to load:\n{e.Message}", MessageType.Error);
                    throw e;
                }
            }

            if (loaded)
                EditorGUILayout.HelpBox($"Loaded!", MessageType.Info);
        }
        else
        {
            loaded = false;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
