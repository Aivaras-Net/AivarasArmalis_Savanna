namespace Savanna.Core.Constants
{
    public static class GameConstants
    {
        public const int DefaultFieldWidth = 20;
        public const int DefaultFieldHeight = 10;
        public const char FieldFill = ' ';

        public const int LogDurationShort = 3;
        public const int LogDurationMedium = 5;
        public const int LogDurationLong = 8;

        public const string ConfigFileName = "animals.json";
        public const string ConfigFileDirectory = "Config";
        public const string ConfigFileNotFound = "Configuration file not found at: {0}";
        public const string ConfigFileEmpty = "Configuration file is empty or invalid";
        public const string ConfigParseError = "Error parsing configuration file: {0}";
        public const string AnimalTypeNotFound = "Configuration not found for animal type: {0}. Available types: {1}";

        public const string SaveGameDirectory = "Saves";
        public const string SaveFilePattern = "savegame_{0}.json";
        public const string SaveFileExtension = ".json";
        public const string GameSavedMessage = "Game saved to {0}";
        public const string GameSaveErrorMessage = "Error saving game: {0}";
        public const string GameLoadedMessage = "Game loaded from {0}";
        public const string GameLoadErrorMessage = "Error loading game: {0}";
        public const string SaveFileNotFoundMessage = "Save file not found: {0}";
        public const string InvalidSaveFormatMessage = "Invalid save file format";

        public const string LionName = "Lion";
        public const string AntelopeName = "Antelope";
        public const string FieldSizeMismatchMessage = "Field size mismatch. Save: {0}x{1}, Current: {2}x{3}";
        public const string CouldNotCreateAnimalMessage = "Could not create animal of type: {0}";
        public const string SaveFromDateTimeFormat = "Save from {0}-{1}-{2} {3}:{4}:{5}";
        public const string AnimalDiedMessage = "{0} died at position ({1},{2})";
        public const string AnimalBornMessage = "New {0} born at position ({1},{2})";
        public const string AnimalHuntedMessage = "{0} hunted {1} at position ({2},{3})";
        public const string AnimalSpawnedMessage = "{0} spawned at position ({1},{2})";
        public const string DateTimeFormat = "yyyyMMdd_HHmmss";
        public const string UnknownAnimalTypeMessage = "Unknown animal type: {0}";
    }
}