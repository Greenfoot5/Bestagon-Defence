using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiddenVariables", menuName = "EnvironmentVariables")]
public class EnvironmentVariables : ScriptableObject
{
    public List<EnvironmentVariable> variables;
}
