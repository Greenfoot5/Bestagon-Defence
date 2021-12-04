using Editor;
using Turrets;
using UnityEditor;

[CustomEditor(typeof(Shooter), true)]
public class ShooterEditor : DynamicTurretEditor
{
    // PROPERTIES

    private SerializedProperty BulletPrefab;


    protected new void OnEnable()
    {
        base.OnEnable();

        BulletPrefab = serializedObject.FindProperty("bulletPrefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Shooter", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(BulletPrefab);

        serializedObject.ApplyModifiedProperties();
    }
}
