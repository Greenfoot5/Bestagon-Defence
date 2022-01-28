using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(TypeSpriteLookup))]
    public class TypeSpriteLookupPropertyDrawer : PropertyDrawer
    {
        private const float Height = 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Setup
            var sprites = property.FindPropertyRelative("sprites");
            var types = TypeSpriteLookup.GetAllTypes();
            // Make sure the arrays are the same length
            if (sprites.arraySize != types.Count)
            {
                sprites.ClearArray();
                for (var i = 0; i < types.Count; i++)
                {
                    sprites.InsertArrayElementAtIndex(i);
                }
            }

            var rect = new Rect(position.x, position.y, position.width, Height);
            
            // Add a mini header to make it clear
            EditorGUI.LabelField(rect, property.displayName, EditorStyles.boldLabel);
            rect.y += Height;
            
            // Add null
            EditorGUI.PropertyField(rect, sprites.GetArrayElementAtIndex(0), new GUIContent("null"));
            rect.y += Height;
            
            // Display each of the types & sprites
            for (var i = 1; i < types.Count; ++i)
            {
                //EditorGUI.LabelField(rect, types[i].Name, EditorStyles.boldLabel);
                //rect.y += Height;
                EditorGUI.PropertyField(rect, sprites.GetArrayElementAtIndex(i), new GUIContent(types[i].Name));
                rect.y += Height;
            }
            
            property.serializedObject.ApplyModifiedProperties();
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return TypeSpriteLookup.GetAllTypes().Count * Height * 2;
        }
    }
}