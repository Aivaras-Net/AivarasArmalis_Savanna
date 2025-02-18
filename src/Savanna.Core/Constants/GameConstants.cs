namespace Savanna.Core.Constants
{
    public static class GameConstants
    {
        public const int DefaultFieldWidth = 10;
        public const int DefaultFieldHeight = 10;

        public const string LionName = "Lion";
        public const string AntelopeName = "Antelope";

        public const double LionVisionRange = 10;
        public const double AntelopeVisionRange = 5;
        public const double LionSpeed = 2;
        public const double AntelopeSpeed = 1;

        public const string InvalidAnimalName = "Invalid animal name";
        public const string LionSpecialActionMessage = "\n Lion at {0} eats antelope at {1} .";

        public const double InitialHealth = 20.0;
        public const double HealthDecresePerTurn = 0.5;
        public const int RequiredMatingTurns = 3;
        public const double HealthGainFromKill = 5.0;
        public const double MaxHealth = 25.0;
    }
}