using Abstract.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ModuleChainHandler))]
    public class ModuleChainHandlerDrawer : PropertyDrawer
    {
        /// <summary>
        /// Called for rendering and handling the GUI events for ModuleChainHandler
        /// </summary>
        /// <param name="position">The position in the Inspector</param>
        /// <param name="property">The property we're displaying</param>
        /// <param name="label">The GUI label for the property</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Location and label of the gui in the editor
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            // The properties of ModuleChainHandler
            SerializedProperty chainProp = property.FindPropertyRelative("chain");
            SerializedProperty tierProp = property.FindPropertyRelative("tier");

            // Get the value of the tier (we don't need the value of the chain)
            int tierValue = tierProp.intValue;

            // Set the percentage of the width the chain input will take
            const float prefabFieldPercent = 0.75f;
            // The buffer between the two input boxes
            const float bufferValue = 3;

            // Set the size of the input box for the chain
            var chainPropInput = new Rect(position) { width = prefabFieldPercent * position.width };

            // Set the size of the input box for the tier
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
