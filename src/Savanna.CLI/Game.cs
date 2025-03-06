using Savanna.CLI.Interfaces;
using Savanna.CLI.Services;
using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Interfaces;

namespace Savanna.CLI
{
    /// <summary>
    /// Main game class that coordinates game logic and UI
    /// </summary>
    public class Game
    {
        private readonly IRendererService _renderer;
        private readonly IMenuService _menuService;
        private readonly IConsoleRenderer _consoleRenderer;
        private readonly ILogService _logService;
        private readonly IGameInitializationService _gameInitService;

        private GameEngine _gameEngine;
        private string _pluginsFolder;

        public Game(IRendererService renderer, IMenuService menuService, IConsoleRenderer consoleRenderer, ILogService logService, IGameInitializationService gameInitService)
        {
            _renderer = renderer;
            _menuService = menuService;
            _consoleRenderer = consoleRenderer;
            _logService = logService;
            _gameInitService = gameInitService;

            _pluginsFolder = Path.Combine(ConsoleConstants.ProjectRootPath, ConsoleConstants.PluginsDirectory);
            _gameInitService.LoadPlugins(_pluginsFolder);
        }

        /// <summary>
        /// Main entry point to start the game
        /// </summary>
        public void Run()
        {
            Console.CursorVisible = false;

            bool exitApplication = false;

            while (!exitApplication)
            {
                exitApplication = StartGameFromMenu();
            }
        }

        /// <summary>
        /// Displays the main menu and starts the selected game mode
        /// </summary>
        /// <returns>True if the user chose to exit the game</returns>
        private bool StartGameFromMenu()
        {
            string[] menuOptions = new string[]
            {
                ConsoleConstants.StartNewGameOption,
                ConsoleConstants.LoadSavedGameOption,
                ConsoleConstants.ExitOption
            };

            int selection = _menuService.GetSelectionFromOptions(ConsoleConstants.SavannaSimulationTitle, menuOptions);
            MenuOption menuOption = (MenuOption)selection;

            switch (menuOption)
            {
                case MenuOption.StartNewGame:
                    _gameEngine = _gameInitService.StartNewGame();
                    if (_gameEngine != null)
                    {
                        RunGame(_gameEngine);
                    }
                    return false;

                case MenuOption.LoadSavedGame:
                    _gameEngine = _gameInitService.LoadSavedGame();
                    if (_gameEngine != null)
                    {
                        RunGame(_gameEngine);
                    }
                    return false;

                case MenuOption.Exit:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Runs the actual game loop
        /// </summary>
        /// <param name="engine">The game engine to use</param>
        private void RunGame(GameEngine engine)
        {
            bool isRunning = true;
            bool isPaused = false;
            DateTime lastUpdate = DateTime.Now;

            InitializeGameDisplay();

            while (isRunning)
            {
                if (!isPaused)
                {
                    UpdateGameState(engine, ref lastUpdate);
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    HandleUserInput(key, engine, ref isRunning, ref isPaused);
                }

                Thread.Sleep(ConsoleConstants.ThreadSleepDuration);
            }
        }

        /// <summary>
        /// Initializes the game display by setting up the header and command guide
        /// </summary>
        private void InitializeGameDisplay()
        {
            _menuService.ClearScreen();
            _renderer.RenderHeader(ConsoleConstants.Header);
            int commandGuideHeight = _menuService.DisplayCommandGuide();

            if (_renderer is RendererService rendererService)
            {
                rendererService.HeaderOffset = ConsoleConstants.HeaderHeight + commandGuideHeight;
            }
        }

        /// <summary>
        /// Updates the game state if enough time has passed since the last update
        /// </summary>
        private void UpdateGameState(GameEngine engine, ref DateTime lastUpdate)
        {
            if ((DateTime.Now - lastUpdate).TotalMilliseconds >= ConsoleConstants.IterationDuration)
            {
                engine.Update();
                engine.DrawField();
                lastUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// Handles user input and performs corresponding actions
        /// </summary>
        private void HandleUserInput(ConsoleKey key, GameEngine engine, ref bool isRunning, ref bool isPaused)
        {
            switch (key)
            {
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
                case ConsoleKey.Spacebar:
                    HandleGamePause(ref isPaused);
                    break;
                case ConsoleKey.S:
                    HandleGameSave(engine);
                    break;
                default:
                    HandleAnimalSpawn(key, engine);
                    break;
            }
        }

        /// <summary>
        /// Handles game pause/resume functionality
        /// </summary>
        private void HandleGamePause(ref bool isPaused)
        {
            isPaused = !isPaused;
            _renderer.ShowLog(isPaused ? ConsoleConstants.GamePaused : ConsoleConstants.GameResumed, ConsoleConstants.LogDurationShort);
        }

        /// <summary>
        /// Handles game save functionality
        /// </summary>
        private void HandleGameSave(GameEngine engine)
        {
            string saveResult = engine.SaveGame();
            string message = !string.IsNullOrEmpty(saveResult)
                ? ConsoleConstants.GameSavedSuccessfully
                : ConsoleConstants.GameSaveFailed;
            _renderer.ShowLog(message, ConsoleConstants.LogDurationMedium);
        }

        /// <summary>
        /// Handles animal spawning based on key press
        /// </summary>
        private void HandleAnimalSpawn(ConsoleKey key, GameEngine engine)
        {
            var animalKeyMappings = _gameInitService.AnimalKeyMappings;
            if (animalKeyMappings.TryGetValue(key, out string keyAnimalType))
            {
                SpawnAnimal(engine, keyAnimalType);
            }
        }

        /// <summary>
        /// Spawns an animal at a random position on the field
        /// </summary>
        private void SpawnAnimal(GameEngine engine, string animalType)
        {
            var field = GetFieldFromEngine(engine);
            if (field != null)
            {
                Random rnd = new Random();
                var position = new Position(rnd.Next(field.Width), rnd.Next(field.Height));
                var animal = _gameInitService.GetAnimalFactory().CreateAnimal(animalType, position);
                engine.AddAnimal(animal);
            }
        }

        /// <summary>
        /// Helper method to get the field from the engine using reflection
        /// to avoid direct access to private fields
        /// </summary>
        private Field GetFieldFromEngine(GameEngine engine)
        {
            var fieldProperty = typeof(GameEngine).GetField("_field",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return fieldProperty?.GetValue(engine) as Field;
        }
    }
}