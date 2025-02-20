using System.Text.Json;
using Savanna.Core.Constants;

namespace Savanna.Core.Config
{
    public class ConfigurationService
    {
        private static AnimalConfig? _config;

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

        private static void LoadConfig()
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