using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Abstract
{
    /// <summary>
    /// Holds the stats for the current game
    /// Most are static for easy accessibility
    /// </summary>
    public class GameStats : MonoBehaviour
    {
        public static int money;
        public int startMoney = 200;

        public static int lives;
        public int startLives = 20;

        public static int rounds;
    
        public static GameControls controls;
        
        /// <summary>
        /// Resets all stats and enables the game's controls at the start of the game
        /// </summary>
        private void Awake()
        {
            money = startMoney;
            lives = startLives;
            rounds = 0;
        
            // Controls
            controls = new GameControls();
            controls.Enable();
            EnhancedTouchSupport.Enable();
        }
    }
}
