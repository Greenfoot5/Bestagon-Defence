using Turrets;
using Turrets.Shooter;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(Shooter), true)]
    public class ShooterEditor : DynamicTurretEditor
    {
        // PROPERTIES

        private SerializedProperty _bulletPrefab;


        protected new void OnEnable()
        {
            base.OnEnable();

            _bulletPrefab = serializedObject.FindProperty("bulletPrefab");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Shooter", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_bulletPrefab);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
