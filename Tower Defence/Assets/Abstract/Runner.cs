using UnityEngine;
using System.Collections;

namespace Abstract
{
    public class Runner : MonoBehaviour
    {
        public static Runner runner; 
    
        /// <summary>
        /// When the game loads, create an instance of this class, and make sure it isn't destroyed between scenes
        /// </summary>
        private static void CreateInstance()
        {
            runner = new GameObject ("Runner").AddComponent<Runner>();
            
            DontDestroyOnLoad (runner);
        }

        /// <summary>
        /// Run a coroutine
        /// </summary>
        public static void Run(IEnumerator coroutine)
        {
            if (runner == null)
                CreateInstance();
            runner.StartCoroutine(coroutine);
        }
    }
}