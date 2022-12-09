using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ModuleChainHandler))]
    public class ModuleChainHandlerDrawer : PropertyDrawer
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
            SerializedProperty chainProp = property.FindPropertyRelative("chain");
            SerializedProperty tierProp = property.FindPropertyRelative("tier");

            // Get the value of the float (we don't need the value of the prefab)
            int tierValue = tierProp.intValue;

            // Set the percentage of the width the prefab input will take
            const float prefabFieldPercent = 0.75f;
            // The buffer between the two input boxes
            const float bufferValue = 3;

            // Set the size of the input box for the prefab
            var chainPropInput = new Rect(position) { width = prefabFieldPercent * position.width };

            // Set the size of the input box for the weight
            position.xMin += prefabFieldPercent * position.width;
            var tierInputRect = new Rect(position);
            tierInputRect.xMin += bufferValue;

            // Add the fields to the gui
            EditorGUI.PropertyField(chainPropInput, chainProp, GUIContent.none);
            tierProp.intValue = EditorGUI.IntField(tierInputRect, tierValue);

            EditorGUI.EndProperty();
        }
    }
}
