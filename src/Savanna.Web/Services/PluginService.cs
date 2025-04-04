using System.Reflection;
using Savanna.Core.Config;
using Savanna.Core.Infrastructure;
using Savanna.Core.Interfaces;
using Savanna.Domain;
using Savanna.Domain.Interfaces;
using Savanna.Web.Constants;
using Savanna.Web.Services.Interfaces;

namespace Savanna.Web.Services
{
    /// <summary>
    /// Service for loading and managing plugins
    /// </summary>
    public class PluginService : IPluginService
    {
        private readonly ILogger<PluginService> _logger;
        private readonly IAnimalFactory _animalFactory;
        private readonly Dictionary<string, Type> _customAnimalTypes = new();
        private readonly Dictionary<string, Assembly> _animalAssemblies = new();

        public PluginService(ILogger<PluginService> logger, IAnimalFactory animalFactory)
        {
            _logger = logger;
            _animalFactory = animalFactory;
        }

        /// <summary>
        /// Loads plugins from the specified folder
        /// </summary>
        /// <param name="pluginsFolder">The folder containing plugins</param>
        public void LoadPlugins(string pluginsFolder)
        {
            if (!EnsurePluginFolderExists(pluginsFolder))
                return;

            string[] pluginFiles = Directory.GetFiles(pluginsFolder, WebConstants.PluginFileSearchPattern);
            if (pluginFiles.Length == 0)
                return;

            foreach (string pluginPath in pluginFiles)
            {
                ProcessPluginFile(pluginPath);
            }
        }

        /// <summary>
        /// Gets a list of all loaded animal types from plugins
        /// </summary>
        /// <returns>List of animal names</returns>
        public IEnumerable<string> GetAvailablePluginAnimals()
        {
            return _customAnimalTypes.Keys;
        }

        /// <summary>
        /// Attempts to create an animal of the given type
        /// </summary>
        /// <param name="animalType">The type of animal to create</param>
        /// <param name="animal">The created animal if successful</param>
        /// <returns>True if animal was created successfully</returns>
        public bool TryCreatePluginAnimal(string animalType, out IAnimal animal)
        {
            animal = null;

            if (!_customAnimalTypes.TryGetValue(animalType, out var type))
                return false;

            try
            {
                animal = (IAnimal)Activator.CreateInstance(type, Position.Null);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create animal of type {AnimalType}", animalType);
                return false;
            }
        }

        /// <summary>
        /// Gets detailed information about a plugin animal
        /// </summary>
        /// <param name="animalType">The type of animal</param>
        /// <returns>Information about the animal</returns>
        public PluginAnimalInfo GetPluginAnimalInfo(string animalType)
        {
            if (!_customAnimalTypes.TryGetValue(animalType, out var type))
                return new PluginAnimalInfo { Name = animalType };

            try
            {
                var info = new PluginAnimalInfo
                {
                    Name = animalType,
                    AnimalType = type.FullName,
                    SourceAssembly = type.Assembly.GetName().Name
                };

                if (_animalAssemblies.TryGetValue(animalType, out var assembly))
                {
                    info.SourceAssembly = assembly.GetName().Name;
                }

                if (TryCreatePluginAnimal(animalType, out var animal))
                {
                    info.IsPredator = animal is IPredator;
                    info.IsPrey = animal is IPrey;
                    info.Speed = animal.Speed;
                    info.VisionRange = animal.VisionRange;

                    // Get configuration for additional properties
                    var animalConfig = ConfigurationService.GetAnimalConfig(animalType);
                    info.SpecialActionChance = animalConfig.SpecialActionChance;

                    if (animal is IPredator predator)
                    {
                        info.HuntingRange = predator.HuntingRange;

                        // Get predator-specific properties from config
                        if (animalConfig.Predator != null)
                        {
                            info.HealthGainFromKill = animalConfig.Predator.HealthGainFromKill;

                            if (animalConfig.Predator.RoarRange > 0)
                            {
                                info.RoarRange = animalConfig.Predator.RoarRange;
                            }
                        }
                    }

                    // Get prey-specific properties from config
                    if (info.IsPrey && animalConfig.Prey != null)
                    {
                        info.HealthFromGrazing = animalConfig.Prey.HealthFromGrazing;
                    }
                }

                return info;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get plugin info for {AnimalType}", animalType);
                return new PluginAnimalInfo { Name = animalType };
            }
        }

        private bool EnsurePluginFolderExists(string pluginsFolder)
        {
            if (!Directory.Exists(pluginsFolder))
            {
                Directory.CreateDirectory(pluginsFolder);
                return false;
            }
            return true;
        }

        private void ProcessPluginFile(string pluginPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(pluginPath);
                ProcessAnimalTypes(assembly);
            }
            catch (Exception ex)
            {
                string fileName = Path.GetFileName(pluginPath);
                _logger.LogError(ex, WebConstants.PluginLoadFailedMessage, fileName, ex.Message);
            }
        }

        private void ProcessAnimalTypes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (IsValidAnimalType(type))
                {
                    RegisterAnimalType(type, assembly);
                }
            }
        }

        private bool IsValidAnimalType(Type type)
        {
            return typeof(IAnimal).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract;
        }

        private void RegisterAnimalType(Type type, Assembly assembly)
        {
            try
            {
                var animal = (IAnimal)Activator.CreateInstance(type, new Position(0, 0));
                string animalName = animal.Name;

                RegisterAnimalConfiguration(animal, animalName);

                _customAnimalTypes[animalName] = type;
                _animalAssemblies[animalName] = assembly;

                if (animal is IPredator)
                {
                    ConfigurationService.AddOrUpdateAnimalConfig(animalName, new AnimalTypeConfig
                    {
                        Speed = animal.Speed,
                        VisionRange = animal.VisionRange,
                        Predator = new PredatorConfig
                        {
                            HuntingRange = ((IPredator)animal).HuntingRange
                        }
                    });
                }
                else if (animal is IPrey)
                {
                    ConfigurationService.AddOrUpdateAnimalConfig(animalName, new AnimalTypeConfig
                    {
                        Speed = animal.Speed,
                        VisionRange = animal.VisionRange,
                        Prey = new PreyConfig()
                    });
                }

                _logger.LogInformation(WebConstants.PluginLoadedMessage, animalName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize animal type {AnimalType}", type.Name);
            }
        }

        private void RegisterAnimalConfiguration(IAnimal animal, string animalName)
        {
            if (animal is IAnimalConfigProvider configProvider)
            {
                ConfigurationService.AddOrUpdateAnimalConfig(animalName, configProvider.GetDefaultConfig());
                _logger.LogInformation("Registered configuration for animal: {AnimalName}", animalName);
            }
        }
    }
}