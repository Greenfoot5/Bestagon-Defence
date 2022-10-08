using System;
using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(WeightedCurvedReference<>))]
    public class WeightedCurvedReferenceEditor : PropertyDrawer
    {
        /// <summary>
        /// Called for rendering and handling the GUI events for WeightedPrefab
        /// </summary>
        /// <param name="position">The position in the Inspector</param>
        /// <param name="property">The property we're displaying</param>
        /// <param name="label">The GUI label for the property</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Location and label of the gui in the editor
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            // The properties of CurvedReference
            SerializedProperty itemProp = property.FindPropertyRelative("item");
            SerializedProperty useConstantProp = property.FindPropertyRelative("useConstant");
            
            SerializedProperty variableProp = property.FindPropertyRelative("variable");
            SerializedProperty curveProp = property.FindPropertyRelative("constantValue");

            // Set the percentage of the width the prefab input will take
            const float itemFieldPercent = 0.5f;
            // Set the width the constant dropdown menu will take
            const float constantFieldWidth = 22f;
            // The buffer between the two input boxes
            const float bufferValue = 3;

            // Set the size of the input box for the prefab
            var itemPropInput = new Rect(position) { width = itemFieldPercent * position.width };
            
            // Set the size of the menu button
            position.xMin += itemFieldPercent * position.width;
            var constantInputRect = new Rect(position) { width = constantFieldWidth };
            constantInputRect.xMin += bufferValue;

            // Set the size of the input box for the Value
            position.xMin += constantFieldWidth;
            var valueInputRect = new Rect(position);
            valueInputRect.xMin += bufferValue;
            
            

            // Add the fields to the gui
            EditorGUI.PropertyField(itemPropInput, itemProp, GUIContent.none);
            
            // Displays a dropdown menu if the user tries to select a new value
            if (EditorGUI.DropdownButton(constantInputRect, GUIContent.none, FocusType.Passive))
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Use Constant"), useConstantProp.boolValue, SetSerializedBool,
                    new Tuple<SerializedProperty, bool>(useConstantProp, true));
                menu.AddItem(new GUIContent("Use Variable"), !useConstantProp.boolValue, SetSerializedBool, 
                    new Tuple<SerializedProperty, bool>(useConstantProp, false));
                
                menu.DropDown(constantInputRect);
            }
            
            // Allows the user to input either an animation curve, or a curved variable
            if (property.FindPropertyRelative("useConstant").boolValue)
            {
                curveProp.animationCurveValue = EditorGUI.CurveField(valueInputRect, curveProp.animationCurveValue);
            }
            else
            {
                EditorGUI.PropertyField(valueInputRect, variableProp, GUIContent.none);
            }

            useConstantProp.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }
        
        /// <summary>
        /// We need to use a method to set the value based on what the user selected
        /// </summary>
        /// <param name="obj">A tuple with the Serialized Property and new boolean value</param>
        private static void SetSerializedBool(object obj)
        {
            var tuple = (Tuple<SerializedProperty,bool>)obj;
            tuple.Item1.boolValue = tuple.Item2;
            tuple.Item1.serializedObject.ApplyModifiedProperties();
        }
    }
}
