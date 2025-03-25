using Savanna.Domain;

namespace Savanna.Core.Config
{
    /// <summary>
    /// Bootstraps the configuration service and initializes the dependency for Domain layer
    /// </summary>
    public static class ConfigurationBootstrap
    {
        /// <summary>
        /// Initializes the configuration service and sets up the configuration provider for Domain layer
        /// </summary>
        public static void Initialize()
        {
            ConfigurationService.LoadConfig();

            Animal.InitializeConfigProvider(ConfigurationService.Instance);
        }
    }
}