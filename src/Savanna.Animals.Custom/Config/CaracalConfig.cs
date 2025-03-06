using System.Text.Json;
using Savanna.Core.Config;
using Savanna.Animals.Custom.Constants;

namespace Savanna.Animals.Custom.Config
{
    public class CaracalConfig
    {
        private static CaracalConfig _instance;
        private static readonly object _lock = new object();

        public Dictionary<string, AnimalTypeConfig> Animals { get; set; } = new();

        public static AnimalTypeConfig CaracalTypeConfig => Instance.Animals[PluginConstants.CaracalName];

        public static CaracalConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = LoadConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        private static CaracalConfig LoadConfig()
        {
            try
            {
                string configPath = Path.Combine(PluginConstants.ConfigDirectory, PluginConstants.CaracalConfigFileName);

                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException(string.Format(PluginConstants.ConfigFileNotFoundMessage, configPath));
                }

                string jsonContent = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<CaracalConfig>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(PluginConstants.ConfigLoadErrorMessage, ex.Message));
                throw;
            }
        }
    }
}