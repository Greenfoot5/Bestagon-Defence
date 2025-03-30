using System.Collections;

namespace Abstract
{
    /// <summary>
    /// Allows us to start coroutines outside MonoBehaviours
    /// </summary>
    // TODO - Is this needed?
    public class Runner
    {
        private static Runner _runner; 
    
        /// <summary>
        /// When the game loads, create an instance of this class, and make sure it isn't destroyed between scenes
        /// </summary>
        private static void CreateInstance()
        {
            //_runner = new GodotObject ("Runner").AddComponent<Runner>();
        }

        /// <summary>
        /// Run a coroutine
        /// </summary>
        public static void Run(IEnumerator coroutine)
        {
            if (_runner == null)
                CreateInstance();
            //_runner.StartCoroutine(coroutine);
        }
    }
}