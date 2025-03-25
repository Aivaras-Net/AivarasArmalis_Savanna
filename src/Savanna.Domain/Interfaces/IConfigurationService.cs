namespace Savanna.Domain.Interfaces
{
    /// <summary>
    /// Interface for accessing and managing game configuration
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the current game configuration
        /// </summary>
        AnimalConfig Config { get; }

        /// <summary>
        /// Gets the configuration for a specific animal type
        /// </summary>
        /// <param name="animalType">The type of animal to get configuration for</param>
        /// <returns>The configuration for the specified animal type</returns>
        AnimalTypeConfig GetAnimalConfig(string animalType);

        /// <summary>
        /// Adds or updates an animal configuration
        /// </summary>
        /// <param name="animalName">The name of the animal</param>
        /// <param name="config">The configuration to add or update</param>
        void AddOrUpdateAnimalConfig(string animalName, AnimalTypeConfig config);

        /// <summary>
        /// Loads the configuration from the default location
        /// </summary>
        void LoadConfig();

        /// <summary>
        /// Saves the current configuration to file
        /// </summary>
        void SaveConfig();

        /// <summary>
        /// Sets a custom configuration path
        /// </summary>
        /// <param name="configPath">The custom configuration file path, or null to reset to default</param>
        void SetConfigPath(string? configPath);
    }
}