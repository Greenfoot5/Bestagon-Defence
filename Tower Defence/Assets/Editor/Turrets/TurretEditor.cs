using Turrets;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(Turret), true)]
    public class TurretEditor : UnityEditor.Editor
    {
        // PROPERTIES
        private SerializedProperty _enemyTag;

        private SerializedProperty _damage;
        private SerializedProperty _range;
        private SerializedProperty _fireRate;

        private SerializedProperty _rangeDisplay;

        private SerializedProperty _awakeEvents;

        protected void OnEnable()
        {
            _enemyTag = serializedObject.FindProperty("enemyTag");

            _damage = serializedObject.FindProperty("damage");
            _range = serializedObject.FindProperty("range");
            _fireRate = serializedObject.FindProperty("fireRate");

            _rangeDisplay = serializedObject.FindProperty("rangeDisplay");

            _awakeEvents = serializedObject.FindProperty("awakeEvents");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("System", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_enemyTag);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_damage);
            EditorGUILayout.PropertyField(_range);
            EditorGUILayout.PropertyField(_fireRate);
            
            EditorGUILayout.PropertyField(_rangeDisplay);

            EditorGUILayout.PropertyField(_awakeEvents);

            serializedObject.ApplyModifiedProperties();
        }
    }
}