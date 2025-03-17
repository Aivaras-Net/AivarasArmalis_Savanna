using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Interfaces;
using Savanna.Web.Services.Interfaces;
using System.Timers;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Service for managing game state and logic
    /// </summary>
    public class GameService : IGameService, IDisposable
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRenderer _gameRenderer;
        private readonly Random _random = new Random();
        private readonly List<string> _gameLogs = new List<string>();
        private readonly System.Timers.Timer _gameTimer;

        private GameEngine _gameEngine;
        private bool _isGameRunning;
        private bool _isPaused;
        private bool _useLetterDisplay;

        private const int TimerInterval = 500;

        public bool IsGameRunning => _isGameRunning;

        public bool IsPaused => _isPaused;

        public GameEngine GameEngine => _gameEngine;

        public bool UseLetterDisplay
        {
            get => _useLetterDisplay;
            set
            {
                if (_useLetterDisplay != value)
                {
                    _useLetterDisplay = value;
                    OnGameStateChanged();
                }
            }
        }

        public IReadOnlyList<string> GameLogs => _gameLogs.AsReadOnly();

        public int FieldWidth => GameConstants.DefaultFieldWidth;

        public int FieldHeight => GameConstants.DefaultFieldHeight;

        public event EventHandler GameStateChanged;

        public GameService(
            ILogger<GameService> logger,
            IGameRenderer gameRenderer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameRenderer = gameRenderer ?? throw new ArgumentNullException(nameof(gameRenderer));

            _gameTimer = new System.Timers.Timer(TimerInterval);
            _gameTimer.Elapsed += GameTimerElapsed;
        }

        public void StartNewGame(IConsoleRenderer renderer)
        {
            _gameEngine = new GameEngine(renderer);
            _isGameRunning = true;
            _isPaused = false;
            _gameLogs.Clear();
            LogMessage("Game started");

            SpawnAntelope();
            SpawnAntelope();
            SpawnLion();

            OnGameStateChanged();
        }

        public void StopGame()
        {
            StopTimer();
            _isGameRunning = false;
            _isPaused = false;
            _gameEngine = null;
            LogMessage("Game stopped");

            OnGameStateChanged();
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            LogMessage(_isPaused ? "Game paused" : "Game resumed");

            OnGameStateChanged();
        }

        public void SpawnAntelope()
        {
            if (!_isGameRunning || _gameEngine == null)
                return;

            var position = GetRandomPosition();
            var animal = new Antelope(1.5, 5.0, position);
            _gameEngine.AddAnimal(animal);
            LogMessage($"Antelope spawned at ({position.X}, {position.Y})");
            OnGameStateChanged();
        }

        public void SpawnLion()
        {
            if (!_isGameRunning || _gameEngine == null)
                return;

            var position = GetRandomPosition();
            var animal = new Lion(2.0, 7.0, position);
            _gameEngine.AddAnimal(animal);
            LogMessage($"Lion spawned at ({position.X}, {position.Y})");
            OnGameStateChanged();
        }

        /// <summary>
        /// Toggles between letter and icon display modes
        /// </summary>
        public void ToggleDisplayMode()
        {
            UseLetterDisplay = !UseLetterDisplay;
            LogMessage(UseLetterDisplay ? "Switched to letter display" : "Switched to icon display");
        }

        public void Update()
        {
            if (!_isGameRunning || _isPaused || _gameEngine == null)
                return;

            _gameEngine.Update();

            OnGameStateChanged();
        }

        public void LogMessage(string message)
        {
            _gameLogs.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            if (_gameLogs.Count > 100)
                _gameLogs.RemoveAt(0);

            OnGameStateChanged();
        }

        public void StartTimer()
        {
            _gameTimer.Start();
        }

        public void StopTimer()
        {
            _gameTimer.Stop();
        }

        private Position GetRandomPosition()
        {
            return new Position(
                _random.Next(FieldWidth),
                _random.Next(FieldHeight)
            );
        }

        private void GameTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        protected virtual void OnGameStateChanged()
        {
            GameStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _gameTimer?.Stop();
            _gameTimer?.Dispose();
        }
    }
}