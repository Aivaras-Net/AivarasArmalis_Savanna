using System.Text.Json;
using Savanna.Core.Constants;

namespace Savanna.Core.Config
{
    /// <summary>
    /// Service for loading and accessing game configuration settings
    /// </summary>
    public class ConfigurationService
    {
        private static AnimalConfig? _config;

        /// <summary>
        /// Gets the loaded configuration, loading it from file if not already loaded
        /// </summary>
        public static AnimalConfig Config
        {
            get
            {
                if (_config == null)
                {
                    LoadConfig();
                }
                return _config!;
            }
        }

        /// <summary>
        /// Loads the configuration from the JSON file
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file cannot be found</exception>
        /// <exception cref="InvalidOperationException">Thrown when the configuration file is empty or invalid</exception>
        public static void LoadConfig()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var configPath = Path.Combine(basePath, GameConstants.ConfigFileDirectory, GameConstants.ConfigFileName);

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(string.Format(GameConstants.ConfigFileNotFound, configPath));
            }

            var jsonString = File.ReadAllText(configPath);
            try
            {
                _config = JsonSerializer.Deserialize<AnimalConfig>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (_config == null)
                {
                    throw new InvalidOperationException(GameConstants.ConfigFileEmpty);
                }
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(string.Format(GameConstants.ConfigParseError, ex.Message));
            }
        }

        /// <summary>
        /// Gets the configuration for a specific animal type
        /// </summary>
        /// <param name="animalType">The type of animal to get configuration for</param>
        /// <returns>The configuration for the specified animal type</returns>
        /// <exception cref="ArgumentException">Thrown when the animal type is not found in configuration</exception>
        public static AnimalTypeConfig GetAnimalConfig(string animalType)
        {
            if (!Config.Animals.TryGetValue(animalType, out var config))
            {
                throw new ArgumentException(string.Format(
                    GameConstants.AnimalTypeNotFound,
                    animalType,
                    string.Join(", ", Config.Animals.Keys)));
            }
            return config;
        }
    }
}