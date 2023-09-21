using System;
using System.Collections.Generic;
using Abstract;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TypeSpriteLookup))]
    public class TypeSpriteLookupPropertyDrawer : PropertyDrawer
    {
        private const float Height = 19f;
        private const float Spacer = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            const float fHeight = Height + Spacer;
            // Setup
            SerializedProperty sprites = property.FindPropertyRelative("sprites");
            List<Type> types = TypeSpriteLookup.GetAllTypes();
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
            rect.y += Height / 2f;
            
            // Add a mini header to make it clear
            EditorGUI.LabelField(rect, property.displayName, EditorStyles.boldLabel);
            rect.y += fHeight;
            
            // Add null
            EditorGUI.PropertyField(rect, sprites.GetArrayElementAtIndex(0), new GUIContent("null"));
            rect.y += fHeight;
            
            // Display each of the types & sprites
            for (var i = 1; i < types.Count; ++i)
            {
                EditorGUI.PropertyField(rect, sprites.GetArrayElementAtIndex(i), new GUIContent(types[i].Name));
                rect.y += fHeight;
            }
            
            property.serializedObject.ApplyModifiedProperties();
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (TypeSpriteLookup.GetAllTypes().Count + 2) * (Height + Spacer);
        }
    }
}