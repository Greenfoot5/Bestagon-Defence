using UnityEngine;

namespace Abstract.EnvironmentVariables
{
    /// <summary>
    /// Sets up the variable in the system environment at the start of the game
    /// </summary>
    public class EnvironmentVariableSetter : MonoBehaviour
    {
        public EnvironmentVariables variables;
        
        /// <summary>
        /// Called when the object is first created.
        /// Saves each variable into the system's environment variables
        /// </summary>
        private void Awake()
        {
            foreach (EnvironmentVariable variable in variables.variables)
            {
                variable.SetData();
            }
        }
    }
}