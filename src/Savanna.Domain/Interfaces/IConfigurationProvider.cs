using Savanna.Domain;

namespace Savanna.Domain.Interfaces
{
    /// <summary>
    /// Interface for accessing configuration settings without direct dependency on ConfigurationService
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the general configuration settings
        /// </summary>
        GeneralConfig GeneralConfig { get; }

        /// <summary>
        /// Gets the complete configuration object
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
    }
}