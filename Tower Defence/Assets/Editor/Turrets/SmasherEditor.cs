using Turrets;
using UnityEditor;

[CustomEditor(typeof(Smasher), true)]
public class SmasherEditor : TurretEditor
{
    // PROPERTIES

    private SerializedProperty SmashEffect;


    protected new void OnEnable()
    {
        base.OnEnable();

        SmashEffect = serializedObject.FindProperty("smashEffect");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Smasher", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(SmashEffect);

        serializedObject.ApplyModifiedProperties();
    }
}
