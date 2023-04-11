using Turrets;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(DynamicTurret), true)]
    public class DynamicTurretEditor : TurretEditor
    {
        // PROPERTIES
        private SerializedProperty _targetingMethod;
        private SerializedProperty _aggressiveRetargeting;

        private SerializedProperty _rotationFrequency;
        private SerializedProperty _rotationZeta;
        private SerializedProperty _rotationInitialResponse;
        private SerializedProperty _partToRotate;

        private SerializedProperty _firePoint;

        protected new void OnEnable()
        {
            base.OnEnable();

            _targetingMethod = serializedObject.FindProperty("targetingMethod");
            _aggressiveRetargeting = serializedObject.FindProperty("aggressiveRetargeting");

            _rotationFrequency = serializedObject.FindProperty("rotationFrequency");
            _rotationZeta = serializedObject.FindProperty("rotationZeta");
            _rotationInitialResponse = serializedObject.FindProperty("rotationInitialResponse");
            _partToRotate = serializedObject.FindProperty("partToRotate");

            _firePoint = serializedObject.FindProperty("firePoint");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_targetingMethod);
            EditorGUILayout.PropertyField(_aggressiveRetargeting);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(_partToRotate);
            
            EditorGUILayout.PropertyField(_rotationFrequency);
            EditorGUILayout.PropertyField(_rotationZeta);
            EditorGUILayout.PropertyField(_rotationInitialResponse);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reference", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_firePoint);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
