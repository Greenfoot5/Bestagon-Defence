using Turrets;
using UnityEditor;

[CustomEditor(typeof(Turret), true)]
public class TurretEditor : Editor
{
    // PROPERTIES

    private SerializedProperty EnemyTag;

    private SerializedProperty Damage;
    private SerializedProperty Range;
    private SerializedProperty FireRate;


    protected void OnEnable()
    {
        EnemyTag = serializedObject.FindProperty("enemyTag");

        Damage = serializedObject.FindProperty("damage");
        Range = serializedObject.FindProperty("range");
        FireRate = serializedObject.FindProperty("fireRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("System", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(EnemyTag);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(Damage);
        EditorGUILayout.PropertyField(Range);
        EditorGUILayout.PropertyField(FireRate);

        serializedObject.ApplyModifiedProperties();
    }
}