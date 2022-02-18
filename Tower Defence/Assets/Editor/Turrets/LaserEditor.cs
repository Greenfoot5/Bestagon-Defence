using Turrets.Laser;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(Laser), true)]
    public class LaserEditor : DynamicTurretEditor
    {
        // PROPERTIES
        private SerializedProperty _lineRenderer;
        private SerializedProperty _impactEffect;

        protected new void OnEnable()
        {
            base.OnEnable();

            _lineRenderer = serializedObject.FindProperty("lineRenderer");
            _impactEffect = serializedObject.FindProperty("impactEffect");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Laser", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_lineRenderer);
            EditorGUILayout.PropertyField(_impactEffect);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
