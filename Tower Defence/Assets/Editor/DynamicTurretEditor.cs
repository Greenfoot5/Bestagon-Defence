using Turrets;
using UnityEditor;

[CustomEditor(typeof(DynamicTurret), true)]
public class DynamicTurretEditor : TurretEditor
{
    // PROPERTIES
    private SerializedProperty TargetingMethod;
    private SerializedProperty AggressiveRetargeting;

    private SerializedProperty TurnSpeed;
    private SerializedProperty PartToRotate;

    private SerializedProperty FirePoint;


    protected new void OnEnable()
    {
        base.OnEnable();

        TargetingMethod = serializedObject.FindProperty("targetingMethod");
        AggressiveRetargeting = serializedObject.FindProperty("aggressiveRetargeting");

        TurnSpeed = serializedObject.FindProperty("turnSpeed");
        PartToRotate = serializedObject.FindProperty("partToRotate");

        FirePoint = serializedObject.FindProperty("firePoint");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(TargetingMethod);
        EditorGUILayout.PropertyField(AggressiveRetargeting);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(TurnSpeed);
        EditorGUILayout.PropertyField(PartToRotate);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Reference", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(FirePoint);

        serializedObject.ApplyModifiedProperties();
    }
}
