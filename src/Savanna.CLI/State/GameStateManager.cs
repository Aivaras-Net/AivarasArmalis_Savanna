using Savanna.CLI.Interfaces;
using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Domain;

namespace Savanna.CLI.State
{
    /// <summary>
    /// Manages the game state and related operations
    /// </summary>
    public class GameStateManager
    {
        private readonly IRendererService _renderer;
        private readonly IGameInitializationService _gameInitService;
        private readonly Random _random;
        private bool _isPaused;
        private DateTime _lastUpdate;

        public bool IsRunning { get; private set; }
        public GameEngine CurrentEngine { get; private set; }

        public GameStateManager(IRendererService renderer, IGameInitializationService gameInitService)
        {
            _renderer = renderer;
            _gameInitService = gameInitService;
            _random = new Random();
            _lastUpdate = DateTime.Now;
            IsRunning = true;
            _isPaused = false;
        }

        public void SetGameEngine(GameEngine engine)
        {
            CurrentEngine = engine;
            IsRunning = true;
            _isPaused = false;
            _lastUpdate = DateTime.Now;
        }

        public void Update()
        {
            if (!_isPaused && IsRunning && CurrentEngine != null)
            {
                if ((DateTime.Now - _lastUpdate).TotalMilliseconds >= ConsoleConstants.IterationDuration)
                {
                    CurrentEngine.Update();
                    CurrentEngine.DrawField();
                    _lastUpdate = DateTime.Now;
                }
            }
        }

        public void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    IsRunning = false;
                    break;
                case ConsoleKey.Spacebar:
                    TogglePause();
                    break;
                case ConsoleKey.S:
                    SaveGame();
                    break;
                default:
                    HandleAnimalSpawn(key);
                    break;
            }
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
            _renderer.ShowLog(_isPaused ? ConsoleConstants.GamePaused : ConsoleConstants.GameResumed, ConsoleConstants.LogDurationShort);
        }

        private void SaveGame()
        {
            if (CurrentEngine == null) return;

            string saveResult = CurrentEngine.SaveGame();
            string message = !string.IsNullOrEmpty(saveResult)
                ? ConsoleConstants.GameSavedSuccessfully
                : ConsoleConstants.GameSaveFailed;
            _renderer.ShowLog(message, ConsoleConstants.LogDurationMedium);
        }

        private void HandleAnimalSpawn(ConsoleKey key)
        {
            if (CurrentEngine == null) return;

            var animalKeyMappings = _gameInitService.AnimalKeyMappings;
            if (animalKeyMappings.TryGetValue(key, out string keyAnimalType))
            {
                SpawnAnimal(keyAnimalType);
            }
        }

        private void SpawnAnimal(string animalType)
        {
            var field = GetFieldFromEngine();
            if (field != null)
            {
                var position = new Position(_random.Next(field.Width), _random.Next(field.Height));
                var animal = _gameInitService.GetAnimalFactory().CreateAnimal(animalType, position);
                CurrentEngine.AddAnimal(animal);
            }
        }

        private Field GetFieldFromEngine()
        {
            if (CurrentEngine == null) return null;

            var fieldProperty = typeof(GameEngine).GetField("_field",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return fieldProperty?.GetValue(CurrentEngine) as Field;
        }
    }
}