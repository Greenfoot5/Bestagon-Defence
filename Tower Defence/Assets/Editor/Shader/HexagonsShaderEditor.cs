using UnityEditor;
using UnityEngine;

namespace Editor.Shader
{
    public class HexagonsShaderEditor : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            materialEditor.SetDefaultGUIWidths();

            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_OffsetUV", properties),
                new GUIContent("Offset UV", "The amount to offset the rect's UV\n1 unit matches the width / height"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Hexagons", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_HexScale", properties),
                new GUIContent("Scale", "The size of hexagons"));
            materialEditor.ShaderProperty(
                FindProperty("_Overlay", properties),
                new GUIContent("Overlay", "The intensity of how much the hexagons darken and lighten the color"));
            materialEditor.ShaderProperty(
                FindProperty("_Opacity", properties),
                new GUIContent("Opacity", "Defines how opaque the hexagons are"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Edge glow", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_GlowIntensity", properties),
                new GUIContent("Intensity", "The intensity of the glow effect"));
            materialEditor.ShaderProperty(
                FindProperty("_GlowClamp", properties),
                new GUIContent("Clamp", "The limiter for the glow (makes it not go brigther than the set value)"));
            materialEditor.ShaderProperty(
                FindProperty("_GlowOpacity", properties),
                new GUIContent("Opacity", "Defines how opaque the glow is"));

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Animations", EditorStyles.boldLabel);

            materialEditor.ShaderProperty(
                FindProperty("_ShiftSpeed", properties),
                new GUIContent("Luminance Shift Speed", "Changes how fast hexagons change color"));
            materialEditor.ShaderProperty(
                FindProperty("_ScrollSpeed", properties),
                new GUIContent("Scroll Speed", "Changes how fast hexagons scroll"));
        }
    }
}