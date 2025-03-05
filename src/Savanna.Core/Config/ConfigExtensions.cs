namespace Savanna.Core.Config
{
    public partial class ConfigurationService
    {
        /// <summary>
        /// Extension methods for accessing configuration properties
        /// </summary>
        public static class ConfigExtensions
        {
            /// <summary>
            /// Gets the hunting range for a predator, with fallback to default value
            /// </summary>
            public static double GetHuntingRange(AnimalTypeConfig config, double defaultValue = 1.0)
            {
                return config.Predator?.HuntingRange ?? defaultValue;
            }

            /// <summary>
            /// Gets the roar range for a predator, with fallback to default value
            /// </summary>
            public static int GetRoarRange(AnimalTypeConfig config, int defaultValue = 0)
            {
                return config.Predator?.RoarRange ?? defaultValue;
            }

            /// <summary>
            /// Gets the special action chance for an animal, with fallback to default value
            /// </summary>
            public static double GetSpecialActionChance(AnimalTypeConfig config, double defaultValue = 0.0)
            {
                return config.SpecialActionChance > 0 ? config.SpecialActionChance : defaultValue;
            }

            /// <summary>
            /// Gets the health gain from kill for a predator, with fallback to default value
            /// </summary>
            public static double GetHealthGainFromKill(AnimalTypeConfig config, double defaultValue = 0.0)
            {
                return config.Predator?.HealthGainFromKill ?? defaultValue;
            }

            /// <summary>
            /// Gets the health from grazing for a prey, with fallback to default value
            /// </summary>
            public static double GetHealthFromGrazing(AnimalTypeConfig config, double defaultValue = 0.0)
            {
                return config.Prey?.HealthFromGrazing ?? defaultValue;
            }
        }
    }
}