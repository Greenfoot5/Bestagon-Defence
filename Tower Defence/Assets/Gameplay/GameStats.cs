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

        private static int _lives;
        public int startLives = 20;
        public static int lives
        {
            get => _lives;
            set
            {
                _lives = value;
                if (value == 0)
                {
                    gameOver?.Invoke();
                }
            }
        }

        private static int _rounds;
        public static int rounds
        { 
            get => _rounds;
            set
            {
                _rounds = value;
                roundProgress?.Invoke();
            }
        }

        public static GameControls controls;

        // Events
        public static event RoundProgressEvent roundProgress;
        public static event GameOverEvent gameOver;
        
        /// <summary>
        /// Resets all stats and enables the game's controls at the start of the game
        /// </summary>
        private void Awake()
        {
            money = startMoney;
            _lives = startLives;
            _rounds = 0;
        
            // Controls
            controls = new GameControls();
            controls.Enable();
            EnhancedTouchSupport.Enable();
        }
    }

    public delegate void RoundProgressEvent();
    public delegate void GameOverEvent();
}
