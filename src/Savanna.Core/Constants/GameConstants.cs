namespace Savanna.Core.Constants
{
    public static class GameConstants
    {
        public const int DefaultFieldWidth = 20;
        public const int DefaultFieldHeight = 10;
        public const char FieldFill = ' ';

        public const int MessageDuration = 3;
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
        public const string DefaultSaveFileName = "savegame.json";
        public const string SaveFilePattern = "savegame_{0}.json";
        public const string SaveFileExtension = ".json";
        public const string NoSavesFoundMessage = "No saved games found.";
        public const string SelectSaveFileMessage = "Select a save file to load:";
        public const string GameSavedMessage = "Game saved to {0}";
        public const string GameSaveErrorMessage = "Error saving game: {0}";
        public const string GameLoadedMessage = "Game loaded from {0}";
        public const string GameLoadErrorMessage = "Error loading game: {0}";
        public const string SaveFileNotFoundMessage = "Save file not found: {0}";
        public const string InvalidSaveFormatMessage = "Invalid save file format";

        public const string LionName = "Lion";
        public const string AntelopeName = "Antelope";

        public const string InvalidAnimalName = "Invalid animal name";
        public const string LionSpecialActionMessage = "\n Lion at {0} eats antelope at {1} .";
    }
}