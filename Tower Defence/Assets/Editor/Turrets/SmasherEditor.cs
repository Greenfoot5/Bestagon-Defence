using Turrets;
using Turrets.Smasher;
using UnityEditor;

namespace Editor.Turrets
{
    [CustomEditor(typeof(Smasher), true)]
    public class SmasherEditor : TurretEditor
    {
        // PROPERTIES

        private SerializedProperty _smashEffect;


        protected new void OnEnable()
        {
            base.OnEnable();

            _smashEffect = serializedObject.FindProperty("smashEffect");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Smasher", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_smashEffect);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
