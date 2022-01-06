using UnityEngine;

public class EnvironmentVariableSetter : MonoBehaviour
{
    public EnvironmentVariables variables;
    private void Awake()
    {
        foreach (var variable in variables.variables)
        {
#if UNITY_EDITOR
            System.Environment.SetEnvironmentVariable(variable.name, variable.editor);
#elif DEVELOPMENT_BUILD
            System.Environment.SetEnvironmentVariable(variable.name, variable.devBuild);
#else
            System.Envirnment.SetEnvironmentVariable(variable.name, variable.stable);
#endif
        }
    }
}
