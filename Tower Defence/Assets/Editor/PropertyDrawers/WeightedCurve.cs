using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(WeightedCurve<>))]
    public class WeightedCurveEditor : PropertyDrawer
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

            // The properties of WeightedPrefab
            SerializedProperty itemProp = property.FindPropertyRelative("item");
            SerializedProperty weightProp = property.FindPropertyRelative("weight");
            
            // Set the percentage of the width the prefab input will take
            const float prefabFieldPercent = 0.6f;
            // The buffer between the two input boxes
            const float bufferValue = 3;
            
            // Set the size of the input box for the prefab
            var itemPropInput = new Rect(position) { width = prefabFieldPercent * position.width };
            
            // Set the size of the input box for the weight
            position.xMin += prefabFieldPercent * position.width;
            var weightInputRect = new Rect(position);
            weightInputRect.xMin += bufferValue;

            // Add the fields to the gui
            EditorGUI.PropertyField(itemPropInput, itemProp, GUIContent.none);
            weightProp.animationCurveValue = EditorGUI.CurveField(weightInputRect, weightProp.animationCurveValue);

            EditorGUI.EndProperty();
        }
    }
}
