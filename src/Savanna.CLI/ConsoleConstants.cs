namespace Savanna.CLI
{
    public static class ConsoleConstants
    {
        public const int IterationDuration = 500;
        public const int HeaderHeight = 1;
        public const int TotalHeaderOffset = HeaderHeight;

        public const string Header = "Savanna simulation";

        public const int LogDurationShort = 3;
        public const int LogDurationMedium = 5;
        public const int LogDurationLong = 8;

        public const double DefaultSpeed = 1;
        public const double DefaultViewRange = 5;

        public const char BorderCorner = '+';
        public const char HorizontalBorder = '-';
        public const char VerticalBorder = '|';

        public const string ArrowPointer = " >> ";
        public const string NoArrowPrefix = "   ";

        public const ConsoleColor AntelopeColor = ConsoleColor.Green;
        public const ConsoleColor LionColor = ConsoleColor.Red;
        public const ConsoleColor DefaultFieldColor = ConsoleColor.White;
        public const ConsoleColor LogHeaderColor = ConsoleColor.Yellow;
    }
}
