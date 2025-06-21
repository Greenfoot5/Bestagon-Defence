using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    /// <summary>
    /// Draws a vector2 field for vector properties.
    /// Usage: [ShowAsVector2] _Vector2("Vector 2", Vector) = (0,0,0,0)
    /// </summary>
    public class ShowAsVector2Drawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            if (prop.type == MaterialProperty.PropType.Vector)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = prop.hasMixedValue;

                EditorGUI.LabelField(position, label);

                position.width = 65;

                position.x = EditorGUIUtility.currentViewWidth - 164;
                EditorGUI.LabelField(position, "X");

                position.x += 12;
                float x = EditorGUI.FloatField(position, 0);

                position.x += 70;
                EditorGUI.LabelField(position, "Y");

                position.x += 12;
                float y = EditorGUI.FloatField(position, 0);

                if (EditorGUI.EndChangeCheck())
                {
                    prop.vectorValue = new Vector4(x, y, 0, 0);
                }
            }
            else
                editor.DefaultShaderProperty(prop, label.text);

        }
    }
}