using System.Collections.Generic;
using UnityEngine;

namespace Abstract.EnvironmentVariables
{
    /// <summary>
    /// An SO that allows us to easily store and reference some environment variables
    /// </summary>
    [CreateAssetMenu(fileName = "HiddenVariables", menuName = "EnvironmentVariables", order = 3)]
    public class EnvironmentVariables : ScriptableObject
    {
        public List<EnvironmentVariable> variables;
    }
}
