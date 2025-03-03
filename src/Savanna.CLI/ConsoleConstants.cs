namespace Savanna.Core.Constants
{
    public static class ConsoleConstants
    {
        public const int IterationDuration = 500;
        public const int HeaderHeight = 1;
        public const int TotalHeaderOffset = HeaderHeight;

        public const string Header = "Savanna simulation";
        public const string SavannaSimulationTitle = "Savanna Simulation";

        public const int LogDurationShort = 3;
        public const int LogDurationMedium = 5;
        public const int LogDurationLong = 8;

        public const int MaxLogCapacity = 15;

        public const char BorderCorner = '+';
        public const char HorizontalBorder = '-';
        public const char VerticalBorder = '|';

        public const string ArrowPointer = " >> ";
        public const string NoArrowPrefix = "   ";

        public const ConsoleColor AntelopeColor = ConsoleColor.Green;
        public const ConsoleColor LionColor = ConsoleColor.Red;
        public const ConsoleColor DefaultFieldColor = ConsoleColor.White;
        public const ConsoleColor LogHeaderColor = ConsoleColor.Yellow;
        public const ConsoleColor SelectedMenuItemColor = ConsoleColor.Cyan;

        public const string GameLogHeaderFormat = "Game Log (Current Frame: {0}):";
        public const string NoSaveFilesFound = "No save files found.";
        public const string NoSaveFilesAvailable = "No save files available.";
        public const string PressAnyKeyToContinue = "Press any key to continue...";
        public const string GamePaused = "Game paused";
        public const string GameResumed = "Game resumed";
        public const string GameSavedSuccessfully = "Game saved successfully";
        public const string GameSaveFailed = "Failed to save game";
        public const string ErrorLoadingPluginFormat = "Error loading plugin {0}: {1}";
        public const string ErrorLoadingPluginsFormat = "Error loading plugins: {0}";

        public const string StartNewGameOption = "Start New Game";
        public const string LoadSavedGameOption = "Load Saved Game";
        public const string ExitOption = "Exit";

        public const string EnterFieldWidthPrompt = "Enter field width";
        public const string EnterFieldHeightPrompt = "Enter field height";

        public const string AvailableAnimals = "Available animals:";
        public const string CommandsHeader = "Commands:";
        public const string SaveGameCommand = "[S] - Save game";
        public const string PauseResumeCommand = "[Space] - Pause/Resume";
        public const string ExitCommand = "[Esc] - Return to main menu";

        public const int DefaultMaxLogs = 5;
        public const int DefaultLogAreaHeight = 6; // Header + 5 log lines
        public const int MaxLogQueueSize = DefaultMaxLogs * 3;
        public const int ThreadSleepDuration = 50;
        public const int MinFieldDimension = 5;
        public const int MaxFieldWidth = 50;
        public const int MaxFieldHeight = 20;
        public const string LoadedGameFormat = "Loaded: {0}";
        public const string LoadedAnimalFormat = "Loaded animal: {0}";
        public const string SelectSaveFilePrompt = "Select a save file to load:";
        public const string NumericInputFormat = "{0} [{1}-{2}, default: {3}]: ";
        public const string NumericInputErrorFormat = "Please enter a number between {0} and {1}.";
        public const string FrameInfoFormat = "[Frame: {0}] {1}";
        public const string ImportsFolder = "Imports";

        public const string ProjectRootPath = @"..\..\..\..\..\";
        public const string DllSearchPattern = "*.dll";
        public const ConsoleKey AntelopeKey = ConsoleKey.A;
        public const ConsoleKey LionKey = ConsoleKey.L;
        public const string AddAnimalCommandFormat = "[{0}] - Add ";

        public const string ServiceNotRegisteredFormat = "Service of type {0} has not been registered.";
    }
}
