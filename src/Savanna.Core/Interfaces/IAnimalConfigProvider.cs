using Savanna.Core.Config;

namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Interface for animal types to provide their default configuration
    /// </summary>
    public interface IAnimalConfigProvider
    {
        /// <summary>
        /// Gets the name of the animal
        /// </summary>
        string AnimalName { get; }

        /// <summary>
        /// Gets the default configuration for this animal type
        /// </summary>
        /// <returns>The default configuration</returns>
        AnimalTypeConfig GetDefaultConfig();
    }
}