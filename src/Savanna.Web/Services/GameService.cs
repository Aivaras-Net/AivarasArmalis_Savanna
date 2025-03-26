using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Domain;
using Savanna.Core.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Web.Services.Interfaces;
using Savanna.Web.Constants;
using System.Timers;
using Savanna.Domain.Interfaces;
using Savanna.Web.Models;

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
        private readonly IAnimalFactory _animalFactory;

        private GameEngine _gameEngine;
        private bool _isGameRunning;
        private bool _isPaused;
        private bool _useLetterDisplay;
        private IAnimal? _selectedAnimal;
        private AnimalDetailViewModel? _selectedAnimalDetails;

        private const int TimerInterval = WebConstants.TimerInterval;

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

        public AnimalDetailViewModel? SelectedAnimalDetails => _selectedAnimalDetails;

        public event EventHandler GameStateChanged;

        public event EventHandler<AnimalDetailViewModel?> AnimalSelectionChanged;

        public GameService(
            ILogger<GameService> logger,
            IGameRenderer gameRenderer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameRenderer = gameRenderer ?? throw new ArgumentNullException(nameof(gameRenderer));
            _animalFactory = new AnimalFactory();

            _gameTimer = new System.Timers.Timer(TimerInterval);
            _gameTimer.Elapsed += GameTimerElapsed;
        }

        public void StartNewGame(IConsoleRenderer renderer)
        {
            _gameEngine = new GameEngine(renderer);
            _isGameRunning = true;
            _isPaused = false;
            _gameLogs.Clear();
            LogMessage(WebConstants.GameStartedMessage);

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
            LogMessage(WebConstants.GameStoppedMessage);

            OnGameStateChanged();
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            LogMessage(_isPaused ? WebConstants.GamePausedMessage : WebConstants.GameResumedMessage);

            OnGameStateChanged();
        }

        public void SpawnAntelope()
        {
            if (!_isGameRunning || _gameEngine == null)
                return;

            var position = GetRandomPosition();
            if (_animalFactory.TryCreateAnimal(GameConstants.AntelopeName, out var animal))
            {
                animal.Position = position;
                _gameEngine.AddAnimal(animal);
                LogMessage(string.Format(WebConstants.AntelopeSpawnedMessage, position.X, position.Y));
                OnGameStateChanged();
            }
            else
            {
                LogMessage(WebConstants.FailedToSpawnAntelopeMessage);
            }
        }

        public void SpawnLion()
        {
            if (!_isGameRunning || _gameEngine == null)
                return;

            var position = GetRandomPosition();
            if (_animalFactory.TryCreateAnimal(GameConstants.LionName, out var animal))
            {
                animal.Position = position;
                _gameEngine.AddAnimal(animal);
                LogMessage(string.Format(WebConstants.LionSpawnedMessage, position.X, position.Y));
                OnGameStateChanged();
            }
            else
            {
                LogMessage(WebConstants.FailedToSpawnLionMessage);
            }
        }

        /// <summary>
        /// Toggles between letter and icon display modes
        /// </summary>
        public void ToggleDisplayMode()
        {
            UseLetterDisplay = !UseLetterDisplay;
            LogMessage(UseLetterDisplay ? WebConstants.SwitchedToLetterDisplayMessage : WebConstants.SwitchedToIconDisplayMessage);
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
            _gameLogs.Add($"[{DateTime.Now.ToString(WebConstants.LogTimeFormat)}] {message}");
            if (_gameLogs.Count > WebConstants.MaxLogEntries)
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

        /// <summary>
        /// Selects an animal by its position in the field
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if an animal was selected, false otherwise</returns>
        public bool SelectAnimalAt(int x, int y)
        {
            if (!_isGameRunning) return false;

            try
            {
                if (_selectedAnimal != null)
                {
                    _selectedAnimal.IsSelected = false;
                    _selectedAnimal = null;
                    _selectedAnimalDetails = null;
                }

                var animal = _gameEngine.Animals.FirstOrDefault(a =>
                    a.Position.X == x && a.Position.Y == y);

                if (animal != null)
                {
                    _selectedAnimal = animal;
                    animal.IsSelected = true;
                    _selectedAnimalDetails = AnimalDetailViewModel.FromAnimal(animal);

                    _logger.LogInformation($"Selected animal: {animal.Name} at ({x}, {y})");

                    AnimalSelectionChanged?.Invoke(this, _selectedAnimalDetails);
                    OnGameStateChanged();
                    return true;
                }

                AnimalSelectionChanged?.Invoke(this, null);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error selecting animal: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deselects the currently selected animal (if any)
        /// </summary>
        public void DeselectAnimal()
        {
            if (_selectedAnimal != null)
            {
                _selectedAnimal.IsSelected = false;
                _selectedAnimal = null;
                _selectedAnimalDetails = null;

                AnimalSelectionChanged?.Invoke(this, null);
                OnGameStateChanged();
            }
        }

        /// <summary>
        /// Gets details for the animal at the specified position
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>AnimalDetailViewModel or null if no animal is at that position</returns>
        public AnimalDetailViewModel? GetAnimalDetailsAt(int x, int y)
        {
            if (!_isGameRunning) return null;

            var animal = _gameEngine.Animals.FirstOrDefault(a =>
                a.Position.X == x && a.Position.Y == y);

            return animal != null ? AnimalDetailViewModel.FromAnimal(animal) : null;
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