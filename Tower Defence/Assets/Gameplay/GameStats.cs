using Gameplay.Waves;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Gameplay
{
    /// <summary>
    /// Holds the stats for the current game
    /// Most are static for easy accessibility
    /// </summary>
    public class GameStats : MonoBehaviour
    {
        private static bool _active;

        public static int money;
        [Tooltip("How much money the player starts the level with")]
        public int startMoney = 200;

        private static int _lives;
        [Tooltip("How many lives the player starts the level with")]
        public int startLives;
        public static int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnLoseLife.Invoke();
                if (value == 0)
                {
                    OnGameOver?.Invoke();
                }
            }
        }

        private static int _rounds;
        public static int Rounds
        { 
            get => _rounds;
            set
            {
                _rounds = value;
                OnRoundProgress?.Invoke();
            }
        }

        public static GameControls controls;

        // Events
        public static event RoundProgressEvent OnRoundProgress;
        public static event GameOverEvent OnGameOver;
        public static event LoseLife OnLoseLife;
        
        /// <summary>
        /// Resets all stats and enables the game's controls at the start of the game
        /// </summary>
        private void Awake()
        {
            if (!_active)
            {
                money = startMoney;
                _lives = startLives;
            }

            // Controls
            controls = new GameControls();
            controls.Enable();
            EnhancedTouchSupport.Enable();
        }

        /// <summary>
        /// Clears the stats for the next level
        /// </summary>
        public static void ClearStats()
        {
            _active = false;
        }
    }

    public delegate void RoundProgressEvent();
    public delegate void GameOverEvent();

    public delegate void LoseLife();
}
