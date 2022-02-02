using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.Abstract
{
    [CustomPropertyDrawer(typeof(UpgradableStat))]
    public class UpgradableStatEditor : PropertyDrawer
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
            
            // The properties of UpgradableStat
            var statProp = property.FindPropertyRelative("stat");
            var modProp = property.FindPropertyRelative("modifier");

            // Get the value of the floats
            var statValue = statProp.floatValue;
            var modValue = modProp.floatValue;

            // Set the percentage of the width the stat input will take
            const float statFieldPercent = 0.4f;
            // The buffer between the two input boxes
            const float bufferValue = 3;

            // Set the size of the input box for the stat
            var statPropInput = new Rect(position) { width = statFieldPercent * position.width };

            // Set the size of the input box for the mod and final value
            position.xMin += statFieldPercent * position.width;
            var modInputRect = new Rect(position);
            modInputRect.xMin += bufferValue;

            // Add the fields to the gui
            statProp.floatValue = EditorGUI.FloatField(statPropInput, statValue);
            EditorGUI.LabelField(modInputRect, $"* {modValue:##0.0#} = {statValue * modValue:#,##0.##}");

            EditorGUI.EndProperty();
        }
    }
}