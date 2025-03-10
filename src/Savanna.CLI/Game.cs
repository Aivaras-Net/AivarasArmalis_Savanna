using Savanna.CLI.Interfaces;
using Savanna.CLI.State;
using Savanna.CLI.UI;
using Savanna.Core;
using Savanna.Core.Constants;

namespace Savanna.CLI
{
    /// <summary>
    /// Main game class that coordinates game logic and UI
    /// </summary>
    public class Game
    {
        private readonly IRendererService _renderer;
        private readonly IMenuRenderer _menuRenderer;
        private readonly IMenuInteraction _menuInteraction;
        private readonly IGameInitializationService _gameInitService;
        private readonly IConsoleWrapper _console;
        private readonly GameStateManager _gameStateManager;
        private readonly string _pluginsFolder;

        public Game(
            IRendererService renderer,
            IMenuRenderer menuRenderer,
            IMenuInteraction menuInteraction,
            IGameInitializationService gameInitService,
            IConsoleWrapper console)
        {
            _renderer = renderer;
            _menuRenderer = menuRenderer;
            _menuInteraction = menuInteraction;
            _gameInitService = gameInitService;
            _console = console;
            _gameStateManager = new GameStateManager(renderer, gameInitService);
            _pluginsFolder = Path.Combine(ConsoleConstants.ProjectRootPath, ConsoleConstants.PluginsDirectory);
            _gameInitService.LoadPlugins(_pluginsFolder);
        }

        /// <summary>
        /// Main entry point to start the game
        /// </summary>
        public void Run()
        {
            _console.CursorVisible = false;

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

            int selection = _menuInteraction.GetSelectionFromOptions(ConsoleConstants.SavannaSimulationTitle, menuOptions);
            MenuOption menuOption = (MenuOption)selection;

            switch (menuOption)
            {
                case MenuOption.StartNewGame:
                    var newEngine = _gameInitService.StartNewGame();
                    if (newEngine != null)
                    {
                        RunGame(newEngine);
                    }
                    return false;

                case MenuOption.LoadSavedGame:
                    var loadedEngine = _gameInitService.LoadSavedGame();
                    if (loadedEngine != null)
                    {
                        RunGame(loadedEngine);
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
            InitializeGameDisplay();
            _gameStateManager.SetGameEngine(engine);

            while (_gameStateManager.IsRunning)
            {
                _gameStateManager.Update();

                if (_console.KeyAvailable)
                {
                    var key = _console.ReadKey(true);
                    _gameStateManager.HandleInput(key);
                }

                Thread.Sleep(ConsoleConstants.ThreadSleepDuration);
            }
        }

        /// <summary>
        /// Initializes the game display by setting up the header and command guide
        /// </summary>
        private void InitializeGameDisplay()
        {
            _menuRenderer.ClearScreen();
            _renderer.RenderHeader(ConsoleConstants.Header);
            int commandGuideHeight = _menuRenderer.DisplayCommandGuide();

            if (_renderer is RendererService rendererService)
            {
                rendererService.HeaderOffset = ConsoleConstants.HeaderHeight + commandGuideHeight;
            }
        }
    }
}