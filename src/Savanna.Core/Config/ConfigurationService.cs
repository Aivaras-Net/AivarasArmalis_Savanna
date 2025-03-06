using System.Text.Json;
using Savanna.Core.Constants;

namespace Savanna.Core.Config
{
    /// <summary>
    /// Service for loading and accessing game configuration settings
    /// </summary>
    public partial class ConfigurationService
    {
        private static AnimalConfig? _config;
        private static readonly string ConfigPath = GetConfigPath();

        /// <summary>
        /// Gets the path to the configuration file
        /// </summary>
        private static string GetConfigPath()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                GameConstants.ConfigFileDirectory,
                GameConstants.ConfigFileName);
        }

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
            if (!File.Exists(ConfigPath))
            {
                throw new FileNotFoundException(string.Format(GameConstants.ConfigFileNotFound, ConfigPath));
            }

            LoadConfigFromPath(ConfigPath);
        }

        /// <summary>
        /// Loads configuration from a specific file path
        /// </summary>
        /// <param name="configPath">The path to the configuration file</param>
        /// <exception cref="InvalidOperationException">Thrown when the configuration file is empty, invalid, or cannot be parsed</exception>
        private static void LoadConfigFromPath(string configPath)
        {
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
                try
                {
                    config = new AnimalTypeConfig();
                    Config.Animals.Add(animalType, config);
                    SaveConfig();
                }
                catch
                {
                    throw new ArgumentException(string.Format(
                        GameConstants.AnimalTypeNotFound,
                        animalType,
                        string.Join(", ", Config.Animals.Keys)));
                }
            }
            return config;
        }

        /// <summary>
        /// Adds or updates an animal configuration in the config file
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        /// <param name="config">The configuration to add or update</param>
        public static void AddOrUpdateAnimalConfig(string animalName, AnimalTypeConfig config)
        {
            if (Config.Animals.ContainsKey(animalName))
            {
                Config.Animals[animalName] = config;
            }
            else
            {
                Config.Animals.Add(animalName, config);
            }
            SaveConfig();
        }

        /// <summary>
        /// Saves the current configuration to the config file
        /// </summary>
        public static void SaveConfig()
        {
            var jsonString = JsonSerializer.Serialize(_config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var directory = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(ConfigPath, jsonString);
        }
    }
}