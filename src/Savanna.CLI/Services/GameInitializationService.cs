using Savanna.CLI.Interfaces;
using Savanna.Core;
using Savanna.Core.Constants;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;
using System.Reflection;
using Savanna.Core.Config;
using Savanna.Domain.Interfaces;
using Savanna.Domain;

namespace Savanna.CLI.Services
{
    /// <summary>
    /// Service that handles game initialization operations
    /// </summary>
    public class GameInitializationService : IGameInitializationService
    {
        private readonly IMenuInteraction _menuInteraction;
        private readonly IMenuRenderer _menuRenderer;
        private readonly IRendererService _renderer;
        private readonly IConsoleRenderer _consoleRenderer;
        private readonly AnimalFactory _animalFactory;

        private readonly ConsoleKey[] _availableKeys = new[]
        {
            ConsoleKey.A, ConsoleKey.B, ConsoleKey.C, ConsoleKey.D, ConsoleKey.E, ConsoleKey.F, ConsoleKey.G,
            ConsoleKey.H, ConsoleKey.I, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.M, ConsoleKey.N,
            ConsoleKey.O, ConsoleKey.P, ConsoleKey.R, ConsoleKey.T, ConsoleKey.U, ConsoleKey.V,
            ConsoleKey.W, ConsoleKey.X, ConsoleKey.Y, ConsoleKey.Z
        };

        private Dictionary<ConsoleKey, string> _animalKeyMappings = new Dictionary<ConsoleKey, string>();

        /// <summary>
        /// Gets the dictionary of animal key mappings
        /// </summary>
        public Dictionary<ConsoleKey, string> AnimalKeyMappings => _animalKeyMappings;

        public GameInitializationService(
            IMenuInteraction menuInteraction,
            IMenuRenderer menuRenderer,
            IRendererService renderer,
            IConsoleRenderer consoleRenderer)
        {
            _menuInteraction = menuInteraction;
            _menuRenderer = menuRenderer;
            _renderer = renderer;
            _consoleRenderer = consoleRenderer;
            _animalFactory = new AnimalFactory();

            InitializeDefaultAnimalMappings();
            InitializeDefaultAnimalColors();
        }

        private void InitializeDefaultAnimalMappings()
        {
            _animalKeyMappings[ConsoleConstants.AntelopeKey] = GameConstants.AntelopeName;
            _animalKeyMappings[ConsoleConstants.LionKey] = GameConstants.LionName;
            _menuInteraction.UpdateAnimalKeyMappings(_animalKeyMappings);
        }

        private void InitializeDefaultAnimalColors()
        {
            _renderer.RegisterAnimalColor(GameConstants.AntelopeName);
            _renderer.RegisterAnimalColor(GameConstants.LionName);
        }

        /// <summary>
        /// Gets the AnimalFactory instance used by this service
        /// </summary>
        /// <returns>The AnimalFactory instance</returns>
        public AnimalFactory GetAnimalFactory()
        {
            return _animalFactory;
        }

        /// <summary>
        /// Starts a new game with user-defined dimensions
        /// </summary>
        /// <returns>A configured GameEngine instance</returns>
        public GameEngine StartNewGame()
        {
            int width = _menuInteraction.GetNumericInput(ConsoleConstants.EnterFieldWidthPrompt, GameConstants.DefaultFieldWidth, ConsoleConstants.MinFieldDimension, ConsoleConstants.MaxFieldWidth);
            int height = _menuInteraction.GetNumericInput(ConsoleConstants.EnterFieldHeightPrompt, GameConstants.DefaultFieldHeight, ConsoleConstants.MinFieldDimension, ConsoleConstants.MaxFieldHeight);

            return new GameEngine(_consoleRenderer, width, height);
        }

        /// <summary>
        /// Loads a saved game from available save files
        /// </summary>
        /// <returns>A loaded GameEngine instance or null if loading failed</returns>
        public GameEngine LoadSavedGame()
        {
            if (!GameEngine.SaveFilesExist())
            {
                ShowNoSaveFilesMessage(ConsoleConstants.NoSaveFilesFound);
                return null;
            }

            var saveFiles = GameEngine.GetSaveFilesDisplayNames();
            if (saveFiles.Count == 0)
            {
                ShowNoSaveFilesMessage(ConsoleConstants.NoSaveFilesAvailable);
                return null;
            }

            string[] displayNames = saveFiles.Keys.ToArray();
            string[] filePaths = saveFiles.Values.ToArray();

            int saveSelection = _menuInteraction.GetSelectionFromOptions(ConsoleConstants.SelectSaveFilePrompt, displayNames);
            string selectedFilePath = filePaths[saveSelection];

            GameEngine gameEngine = null;

            if (GameEngine.TryGetSaveFileDimensions(selectedFilePath, out int saveWidth, out int saveHeight))
            {
                gameEngine = new GameEngine(_consoleRenderer, saveWidth, saveHeight);
                if (gameEngine.LoadGame(selectedFilePath, _animalFactory))
                {
                    _renderer.ShowLog(string.Format(ConsoleConstants.LoadedGameFormat, Path.GetFileName(selectedFilePath)), ConsoleConstants.LogDurationMedium);
                }
                else
                {
                    gameEngine = null;
                }
            }

            return gameEngine;
        }

        /// <summary>
        /// Assigns a keyboard key for an animal
        /// </summary>
        /// <param name="animalName">The name of the animal to assign a key to</param>
        public void AssignKeyForAnimal(string animalName)
        {
            if (animalName == GameConstants.AntelopeName || animalName == GameConstants.LionName)
            {
                return;
            }

            if (_animalKeyMappings.ContainsValue(animalName))
            {
                return;
            }

            ConsoleKey preferredKey = (ConsoleKey)animalName[0];

            if (!_animalKeyMappings.ContainsKey(preferredKey) && _availableKeys.Contains(preferredKey))
            {
                _animalKeyMappings[preferredKey] = animalName;

                _menuInteraction.UpdateAnimalKeyMappings(_animalKeyMappings);

                return;
            }

            foreach (var key in _availableKeys)
            {
                if (!_animalKeyMappings.ContainsKey(key))
                {
                    _animalKeyMappings[key] = animalName;

                    _menuInteraction.UpdateAnimalKeyMappings(_animalKeyMappings);

                    return;
                }
            }
        }

        /// <summary>
        /// Loads plugins from the specified folder
        /// </summary>
        /// <param name="importsFolder">The folder containing plugins</param>
        public void LoadPlugins(string importsFolder)
        {
            if (!EnsurePluginFolderExists(importsFolder))
                return;

            string[] pluginFiles = Directory.GetFiles(importsFolder, ConsoleConstants.DllSearchPattern);
            if (pluginFiles.Length == 0)
                return;

            foreach (string pluginPath in pluginFiles)
            {
                ProcessPluginFile(pluginPath);
            }
        }

        private bool EnsurePluginFolderExists(string importsFolder)
        {
            if (!Directory.Exists(importsFolder))
            {
                Directory.CreateDirectory(importsFolder);
                return false;
            }
            return true;
        }

        private void ProcessPluginFile(string pluginPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(pluginPath);
                ProcessAnimalTypes(assembly.GetTypes());
                _menuInteraction.UpdateAnimalKeyMappings(_animalKeyMappings);
            }
            catch (Exception ex)
            {
                _renderer.ShowLog(string.Format(ConsoleConstants.ErrorLoadingPluginFormat, pluginPath, ex.Message), ConsoleConstants.LogDurationLong);
            }
        }

        private void ProcessAnimalTypes(Type[] types)
        {
            foreach (var type in types)
            {
                if (IsValidAnimalType(type))
                {
                    RegisterAnimalType(type);
                }
            }
        }

        private bool IsValidAnimalType(Type type)
        {
            return typeof(IAnimal).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract;
        }

        private void RegisterAnimalType(Type type)
        {
            try
            {
                var animal = (IAnimal)Activator.CreateInstance(type, new Position(0, 0));
                string animalName = animal.Name;

                RegisterAnimalConfiguration(animal, animalName);
                RegisterAnimalInFactory(type, animalName);
            }
            catch (Exception ex)
            {
                _renderer.ShowLog(string.Format(ConsoleConstants.FailedToInitializeAnimalFormat, ex.Message), ConsoleConstants.LogDurationLong);
            }
        }

        private void RegisterAnimalConfiguration(IAnimal animal, string animalName)
        {
            if (animal is IAnimalConfigProvider configProvider)
            {
                ConfigurationService.AddOrUpdateAnimalConfig(animalName, configProvider.GetDefaultConfig());
                _renderer.ShowLog(string.Format(ConsoleConstants.RegisteredAnimalConfigFormat, animalName), ConsoleConstants.LogDurationMedium);
            }
        }

        private void RegisterAnimalInFactory(Type type, string animalName)
        {
            _animalFactory.RegisterCustomAnimal(type);
            AssignKeyForAnimal(animalName);
            _renderer.RegisterAnimalColor(animalName);
            _renderer.ShowLog(string.Format(ConsoleConstants.LoadedAnimalFormat, animalName), ConsoleConstants.LogDurationMedium);
        }

        /// <summary>
        /// Shows a message when no save files are found
        /// </summary>
        /// <param name="message">The message to display</param>
        public void ShowNoSaveFilesMessage(string message)
        {
            _menuRenderer.ClearScreen();
            Console.WriteLine(message);
            Console.WriteLine(ConsoleConstants.PressAnyKeyToContinue);
            Console.ReadKey(true);
        }
    }
}