using UnityEditor;
using UnityEngine;

namespace Editor.Shader
{
    public class HexagonSpreadEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            materialEditor.SetDefaultGUIWidths();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_HexagonSize", properties),
                new GUIContent("Hexagon Size", "The size of the hexagons"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_Duration", properties),
                new GUIContent("Duration", "The time it takes for hexagons to trigger on the edges from the center"));
            materialEditor.ShaderProperty(
                FindProperty("_AppearDuration", properties),
                new GUIContent("Morph Duration", "The time it takes for a hexagon to complete its animation"));
            materialEditor.ShaderProperty(
                FindProperty("_FactorInvert", properties),
                new GUIContent("Invert", "Changes between a fill and a dissipate animation"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Hexagon Animation", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_Rotation", properties),
                new GUIContent("Rotation", "How much it rotates during its morph."));
            materialEditor.ShaderProperty(
                FindProperty("_Displacement", properties),
                new GUIContent("Displacement", "How much it's offset from the origin during the morph (ease-out)"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Extra", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_Preview", properties),
                new GUIContent("Preview", "Forces the morph along the X axis"));
            materialEditor.ShaderProperty(
                FindProperty("_Loop", properties),
                new GUIContent("Loop", "Makes the animation loop"));
        }
    }
}