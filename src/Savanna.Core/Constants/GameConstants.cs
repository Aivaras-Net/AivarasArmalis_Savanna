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

        public const string LionName = "Lion";
        public const string AntelopeName = "Antelope";

        public const string InvalidAnimalName = "Invalid animal name";
        public const string LionSpecialActionMessage = "\n Lion at {0} eats antelope at {1} .";
    }
}