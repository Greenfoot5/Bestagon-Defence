using UnityEngine;
using System.Collections;

namespace Abstract
{
    /// <summary>
    /// Allows us to start coroutines outside MonoBehaviours
    /// </summary>
    public class Runner : MonoBehaviour
    {
        private static Runner _runner; 
    
        /// <summary>
        /// When the game loads, create an instance of this class, and make sure it isn't destroyed between scenes
        /// </summary>
        private static void CreateInstance()
        {
            _runner = new GameObject ("Runner").AddComponent<Runner>();
            
            DontDestroyOnLoad (_runner);
        }

        /// <summary>
        /// Run a coroutine
        /// </summary>
        public static void Run(IEnumerator coroutine)
        {
            if (_runner == null)
                CreateInstance();
            _runner.StartCoroutine(coroutine);
        }
    }
}