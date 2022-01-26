using System.Collections.Generic;
using UnityEngine;

namespace Abstract.Data
{
    /// <summary>
    /// An SO that allows us to easily store and reference some environment variables
    /// </summary>
    [CreateAssetMenu(fileName = "HiddenVariables", menuName = "EnvironmentVariables")]
    public class EnvironmentVariables : ScriptableObject
    {
        public List<EnvironmentVariable> variables;
    }
}
