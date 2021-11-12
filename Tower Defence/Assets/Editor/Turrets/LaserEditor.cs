using Turrets;
using UnityEditor;

[CustomEditor(typeof(Laser), true)]
public class LaserEditor : DynamicTurretEditor
{
    // PROPERTIES

    private SerializedProperty LineRenderer;
    private SerializedProperty ImpactEffect;


    protected new void OnEnable()
    {
        base.OnEnable();

        LineRenderer = serializedObject.FindProperty("lineRenderer");
        ImpactEffect = serializedObject.FindProperty("impactEffect");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Laser", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(LineRenderer);
        EditorGUILayout.PropertyField(ImpactEffect);

        serializedObject.ApplyModifiedProperties();
    }
}
