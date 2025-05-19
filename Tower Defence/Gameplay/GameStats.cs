using Abstract.Data;
using Godot;
using UI.Shop;
using Environment = System.Environment;

namespace Gameplay
{
    /// <summary>
    /// Holds the stats for the current game
    /// Most are static for easy accessibility
    /// </summary>
    public partial class GameStats : Node
    {
        private static bool _active;

        private static int _energy;
        /// <summary>
        /// How much energy the player starts the level with
        /// </summary>
        [Export]
        public int StartEnergy = 200;
        public static int Energy
        {
            get => _energy;
            set
            {
                _energy = value;
                OnGainEnergy?.Invoke();
            }
        }
        
        private static int _powercells;
        public static int Powercells
        {
            get => _powercells;
            set
            {
                _powercells = value;
                OnGainPowercell?.Invoke();
            }
        }

        private static int _lives;
        /// <summary>
        /// How many lives the player starts the level with
        /// </summary>
        [Export]
        public int StartLives = 20;
        public static int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnLoseLife?.Invoke();
                if (value == 0)
                {
                    OnGameOver?.Invoke();
                }
            }
        }

        private static int _rounds = 1;
        public static int Rounds
        { 
            get => _rounds;
            set
            {
                _rounds = value;
                OnRoundProgress?.Invoke();
            }
        }

        public static void PopulateRounds(int rounds)
        {
            _rounds = rounds;
        }
        public static void PopulateLives(int lives)
        {
            _lives = lives;
        }

        // public static GameControls controls;
        private static int _randomSeed;

        // Events
        public static event RoundProgressEvent OnRoundProgress;
        public static event GameOverEvent OnGameOver;
        public static event LoseLife OnLoseLife;
        public static event GainMoney OnGainEnergy;
        public static event GainCell OnGainPowercell;
        
        /// <summary>
        /// Resets all stats and enables the game's controls at the start of the game
        /// </summary>
        public override void _Ready()
        {
            if (!_active)
            {
                _powercells = 0;
                _lives = StartLives;
                _rounds = 1;
                _randomSeed = Environment.TickCount;
                GD.Seed((ulong)_randomSeed);
                Shop.Random = new Squirrel3(_randomSeed);
                Energy = StartEnergy;
            }
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

    public delegate void GainMoney();

    public delegate void GainCell();
}
