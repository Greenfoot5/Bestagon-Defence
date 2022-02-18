using MaterialLibrary.Hexagons;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Hexagons))]
    public class HexagonsEditor : UnityEditor.Editor
    {
        // DISPLAY FLAGS
        public bool showHexagons;
        public bool animateHexagons;
        public bool glow;
        public bool import;
        public bool loaded;
        
        // PROPERTIES
        private SerializedProperty _color;
        private SerializedProperty _offsetUV;

        private SerializedProperty _hexagonScale;
        private SerializedProperty _scrollSpeed;
        private SerializedProperty _luminanceShiftSpeed;
        private SerializedProperty _overlayStrength;
        private SerializedProperty _hexagonOpacity;

        private SerializedProperty _glowIntensity;
        private SerializedProperty _glowClamp;
        private SerializedProperty _glowOpacity;

        // IMPORT
        private Material _material;

        private void OnEnable()
        {
            _color = serializedObject.FindProperty("m_Color");
            _offsetUV = serializedObject.FindProperty("OffsetUV");

            _hexagonScale = serializedObject.FindProperty("HexagonScale");
            _scrollSpeed = serializedObject.FindProperty("ScrollSpeed");
            _luminanceShiftSpeed = serializedObject.FindProperty("LuminanceShiftSpeed");
            _overlayStrength = serializedObject.FindProperty("OverlayStrength");
            _hexagonOpacity = serializedObject.FindProperty("HexagonOpacity");

            _glowIntensity = serializedObject.FindProperty("GlowIntensity");
            _glowClamp = serializedObject.FindProperty("GlowClamp");
            _glowOpacity = serializedObject.FindProperty("GlowOpacity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_color);
            EditorGUILayout.PropertyField(_offsetUV);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Hexagons", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_hexagonScale);
            EditorGUILayout.PropertyField(_overlayStrength);
            EditorGUILayout.PropertyField(_hexagonOpacity);

            EditorGUILayout.Separator();

            animateHexagons = EditorGUILayout.Foldout(animateHexagons, "Animation");
            if (animateHexagons)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(_scrollSpeed);
                EditorGUILayout.PropertyField(_luminanceShiftSpeed);
            }

            EditorGUILayout.Separator();

            glow = EditorGUILayout.Foldout(glow, "Inner Edge Glow");
            if (glow)
            {
                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(_glowIntensity);
                EditorGUILayout.PropertyField(_glowClamp);
                EditorGUILayout.PropertyField(_glowOpacity);
            }

            EditorGUILayout.Separator();

            import = EditorGUILayout.Foldout(import, "Import Parameters");
            if (import)
            {
                EditorGUILayout.Separator();

                _material = (Material)EditorGUILayout.ObjectField(_material, typeof(Material), false);

                if (_material != null)
                {
                    try
                    {
                        ((Hexagons)serializedObject.targetObject).ImportMaterial(_material);
                        _material = null;
                        loaded = true;

                        PrefabUtility.RecordPrefabInstancePropertyModifications(serializedObject.targetObject);
                    }
                    catch (UnityException e)
                    {
                        EditorGUILayout.HelpBox($"Failed to load:\n{e.Message}", MessageType.Error);
                        throw;
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
}
