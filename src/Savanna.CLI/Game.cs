using Savanna.CLI.Interfaces;
using Savanna.CLI.Services;
using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Domain;
using Savanna.Core.Domain.Interfaces;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;
using System.Reflection;

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

        private readonly AnimalFactory _animalFactory = new AnimalFactory();
        private readonly Dictionary<ConsoleKey, string> _animalKeyMappings = new();
        private readonly ConsoleKey[] _availableKeys = new[]
        {
            ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G,
            ConsoleKey.H, ConsoleKey.I, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.M, ConsoleKey.N,
            ConsoleKey.O, ConsoleKey.P, ConsoleKey.R, ConsoleKey.T, ConsoleKey.U, ConsoleKey.V,
            ConsoleKey.W, ConsoleKey.X, ConsoleKey.Y, ConsoleKey.Z
        };

        private GameEngine _gameEngine;
        private string _pluginsFolder;

        public Game(IRendererService renderer, IMenuService menuService, IConsoleRenderer consoleRenderer, ILogService logService)
        {
            _renderer = renderer;
            _menuService = menuService;
            _consoleRenderer = consoleRenderer;
            _logService = logService;

            _renderer.RegisterAnimalColor(GameConstants.AntelopeName);
            _renderer.RegisterAnimalColor(GameConstants.LionName);

            _animalKeyMappings[ConsoleKey.A] = GameConstants.AntelopeName;
            _animalKeyMappings[ConsoleKey.L] = GameConstants.LionName;

            string currentDirectory = Directory.GetCurrentDirectory();
            string projectRoot = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\.."));
            _pluginsFolder = Path.Combine(projectRoot, "Imports");

            if (!Directory.Exists(_pluginsFolder))
            {
                Directory.CreateDirectory(_pluginsFolder);
            }
        }

        /// <summary>
        /// Initializes and starts the game
        /// </summary>
        public void Run()
        {
            Console.CursorVisible = false;

            LoadPlugins(_pluginsFolder);

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
                "Start New Game",
                "Load Saved Game",
                "Exit"
            };

            int selection = _menuService.GetSelectionFromOptions("Savanna Simulation", menuOptions);

            switch (selection)
            {
                case 0: // Start New Game
                    int width = _menuService.GetNumericInput("Enter field width", GameConstants.DefaultFieldWidth, 5, 50);
                    int height = _menuService.GetNumericInput("Enter field height", GameConstants.DefaultFieldHeight, 5, 20);

                    _gameEngine = new GameEngine(_consoleRenderer, width, height);
                    RunGame(_gameEngine);
                    return false;

                case 1: // Load Saved Game
                    if (!GameEngine.SaveFilesExist())
                    {
                        _menuService.ClearScreen();
                        Console.WriteLine("No save files found.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        return false;
                    }

                    var saveFiles = GameEngine.GetSaveFilesDisplayNames();
                    if (saveFiles.Count == 0)
                    {
                        _menuService.ClearScreen();
                        Console.WriteLine("No save files available.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        return false;
                    }

                    string[] displayNames = saveFiles.Keys.ToArray();
                    string[] filePaths = saveFiles.Values.ToArray();

                    int saveSelection = _menuService.GetSelectionFromOptions("Select a save file to load:", displayNames);
                    string selectedFilePath = filePaths[saveSelection];

                    if (GameEngine.TryGetSaveFileDimensions(selectedFilePath, out int saveWidth, out int saveHeight))
                    {
                        _gameEngine = new GameEngine(_consoleRenderer, saveWidth, saveHeight);
                        if (_gameEngine.LoadGame(selectedFilePath, _animalFactory))
                        {
                            _renderer.ShowLog($"Loaded: {Path.GetFileName(selectedFilePath)}", ConsoleConstants.LogDurationMedium);
                            RunGame(_gameEngine);
                        }
                    }
                    return false;

                case 2: // Exit
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
            _menuService.ClearScreen();
            _renderer.RenderHeader(ConsoleConstants.Header);
            UpdateAnimalKeyMappings();
            _menuService.DisplayCommandGuide();

            bool isRunning = true;
            bool isPaused = false;
            DateTime lastUpdate = DateTime.Now;

            while (isRunning)
            {
                if (!isPaused && (DateTime.Now - lastUpdate).TotalMilliseconds >= ConsoleConstants.IterationDuration)
                {
                    engine.Update();
                    engine.DrawField();
                    lastUpdate = DateTime.Now;
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            isRunning = false;
                            break;
                        case ConsoleKey.Spacebar:
                            isPaused = !isPaused;
                            _renderer.ShowLog(isPaused ? "Game paused" : "Game resumed", ConsoleConstants.LogDurationShort);
                            break;
                        case ConsoleKey.S:
                            string saveResult = engine.SaveGame();
                            if (!string.IsNullOrEmpty(saveResult))
                            {
                                _renderer.ShowLog("Game saved successfully", ConsoleConstants.LogDurationMedium);
                            }
                            else
                            {
                                _renderer.ShowLog("Failed to save game", ConsoleConstants.LogDurationMedium);
                            }
                            break;
                        default:
                            if (_animalKeyMappings.TryGetValue(key, out string keyAnimalType))
                            {
                                Random rnd = new Random();
                                var keyField = GetFieldFromEngine(engine);
                                if (keyField != null)
                                {
                                    engine.AddAnimal(_animalFactory.CreateAnimal(keyAnimalType,
                                        new Position(rnd.Next(keyField.Width), rnd.Next(keyField.Height))));
                                }
                            }
                            break;
                    }
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Updates the menu service with current animal key mappings
        /// </summary>
        private void UpdateAnimalKeyMappings()
        {
            // Share the animal key mappings with the menu service
            if (_menuService is MenuService menuService)
            {
                menuService.UpdateAnimalKeyMappings(_animalKeyMappings);
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

        /// <summary>
        /// Loads animal plugins from the plugins folder
        /// </summary>
        /// <param name="importsFolder">The plugins folder path</param>
        private void LoadPlugins(string importsFolder)
        {
            try
            {
                string[] dllFiles = Directory.GetFiles(importsFolder, "*.dll");

                foreach (string dllFile in dllFiles)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllFile);

                        foreach (Type type in assembly.GetTypes())
                        {
                            if (!type.IsAbstract && typeof(IAnimal).IsAssignableFrom(type))
                            {
                                var animal = (IAnimal)Activator.CreateInstance(type, new Position(0, 0));
                                string animalName = animal.Name;

                                _animalFactory.RegisterCustomAnimal(type);

                                AssignKeyForAnimal(animalName);

                                _renderer.RegisterAnimalColor(animalName);

                                _renderer.ShowLog($"Loaded animal: {animalName}", ConsoleConstants.LogDurationMedium);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _renderer.ShowLog($"Error loading plugin {Path.GetFileName(dllFile)}: {ex.Message}", ConsoleConstants.LogDurationLong);
                    }
                }
            }
            catch (Exception ex)
            {
                _renderer.ShowLog($"Error loading plugins: {ex.Message}", ConsoleConstants.LogDurationLong);
            }
        }

        /// <summary>
        /// Assigns a keyboard key for a newly loaded animal
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        private void AssignKeyForAnimal(string animalName)
        {
            if (animalName == GameConstants.AntelopeName || animalName == GameConstants.LionName)
            {
                return;
            }

            ConsoleKey preferredKey = (ConsoleKey)animalName[0];

            if (!_animalKeyMappings.ContainsKey(preferredKey) && _availableKeys.Contains(preferredKey))
            {
                _animalKeyMappings[preferredKey] = animalName;
                return;
            }

            foreach (var key in _availableKeys)
            {
                if (!_animalKeyMappings.ContainsKey(key))
                {
                    _animalKeyMappings[key] = animalName;
                    return;
                }
            }
        }
    }
}